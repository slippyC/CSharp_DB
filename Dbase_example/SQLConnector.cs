using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System;

namespace Dbase_example
{
    class SQLConnector
    {
        private SQLiteConnection mSqlite;
        private char pSep = Path.DirectorySeparatorChar; //Crosss-platform seperator
        private string appPath;

        public SQLConnector()
        {
            this.appPath = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
        }

        public List<ArtistData> getArtists()
        {
            return this.getArtistData();
        }

        // Artist Filtered - Use SQL wildcards for name(% and _)
        public List<ArtistData> getArtists(string artist)
        {
            return this.getArtistData(artist);
        }

        private List<ArtistData> getArtistData(string artist = "")
        {
            this.mSqlite = this.openConn();
            List<ArtistData> ret = new List<ArtistData>();
            SQLiteDataReader data;
            if (artist == "")
                data = this.exeCommand("SELECT * from Artist");
            else
                data = this.exeCommand($"SELECT * from Artist WHERE Name LIKE {artist}");// Filter Artist by name

            while (data.Read())
            {            
                ret.Add(new ArtistData { Name = data.GetValue(1).ToString(), ArtistId = data.GetInt32(0) });
            }
            this.mSqlite.Close();
            return ret;
        }

        public List<AlbumData> getAlbums(int ArtistId)
        {
            this.mSqlite = this.openConn();
            List<AlbumData> ret = new List<AlbumData>();
            SQLiteDataReader data = this.exeCommand($"SELECT AlbumId,Title from Album where ArtistId = {ArtistId}");
            while (data.Read())
            {
                ret.Add(new AlbumData
                { AlbumId = data.GetInt32(0), Title = (string)data.GetValue(1) }
                );
            }
            this.mSqlite.Close();
            return ret;
        }

        public List<TrackData> getTracks(int AlbumId)
        {
            this.mSqlite = this.openConn();
            List<TrackData> ret = new List<TrackData>();
            SQLiteDataReader data = this.exeCommand($"SELECT TrackId,Name,Milliseconds from Track where AlbumId = {AlbumId}");

            while (data.Read())
            {
                ret.Add(new TrackData
                { TrackId = data.GetInt32(0), Name = (string)data.GetValue(1), Time = data.GetInt32(2) }
                );                
            }
            this.mSqlite.Close();           

            return ret;
        }

        //Example of InnerJoin, not an efficient way to do it here. For example purposes only!!!
        public string getGenre(int AlbumId, int TrackId)
        {
            this.mSqlite = this.openConn();
            SQLiteDataReader data = this.exeCommand("SELECT Genre.Name FROM Genre " +
                   "INNER JOIN Track ON Genre.GenreId = Track.GenreId " +
                   $"WHERE Track.AlbumId = {AlbumId.ToString()} AND Track.TrackId = {TrackId.ToString()}");
            data.Read();
            string ret = data.GetValue(0).ToString();
            this.mSqlite.Close();
            return ret;
        }

        private SQLiteDataReader exeCommand(string query)
        {
            this.mSqlite.Open();
            SQLiteCommand cmd = this.mSqlite.CreateCommand();
            cmd.CommandText = query;
            return cmd.ExecuteReader();
        }

        private SQLiteConnection openConn()
        {
            if (this.mSqlite != null && this.mSqlite.State == System.Data.ConnectionState.Open)
            {
                this.mSqlite.Close();
            }

            return new SQLiteConnection($"Data Source={this.appPath}{this.pSep}Music.sqlite;Version=3");
        }
    }

    public class ArtistData
    {
        public string Name { get; set; }
        public int ArtistId { get; set; }
    }

    public class AlbumData
    {
        public int AlbumId { get; set; }
        public string Title { get; set; }
    }

    public class TrackData
    {
        public int TrackId { get; set; }
        public string Name { get; set; }
        public int Time { get; set; }
    }
}
