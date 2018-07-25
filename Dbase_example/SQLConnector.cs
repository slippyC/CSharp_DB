using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.IO;

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
            this.mSqlite = this.openConn();
            List<ArtistData> ret = new List<ArtistData>();
            SQLiteDataReader data = this.exeCommand("SELECT * from Artist");
            while (data.Read())
            {
                ret.Add(new ArtistData { Name = (string)data.GetValue(1), ArtistId = (int)data.GetValue(0) });
            }
            this.mSqlite.Close();
            return ret;
        }

        public List<AlbumData> getAlbums(int ArtistId)
        {
            this.mSqlite = this.openConn();
            List<AlbumData> ret = new List<AlbumData>();
            SQLiteDataReader data = this.exeCommand("SELECT AlbumId,Title from Album where ArtistId = @ArtistId");
            while (data.Read())
            {
                ret.Add(new AlbumData
                { AlbumId = (int)data.GetValue(0), Title = (string)data.GetValue(1) }
                );
            }
            this.mSqlite.Close();
            return ret;
        }

        public List<TrackData> getTracks(int AlbumId)
        {
            this.mSqlite = this.openConn();
            List<TrackData> ret = new List<TrackData>();
            SQLiteDataReader data = this.exeCommand("SELECT TrackId,Name,Milliseconds from Track where AlbumId = @AlbumId");

            while (data.Read())
            {
                ret.Add(new TrackData
                { TrackId = (int)data.GetValue(0), Name = (string)data.GetValue(1), Time = (int)data.GetValue(2) }
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
                   "INNER JOIN Track ON Genre.Id = Track.GenreId " +
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
