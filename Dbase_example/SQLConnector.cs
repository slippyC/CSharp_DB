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

        public List<object> getArtists()
        {
            this.mSqlite = this.openConn();            
            List<object> ret = new List<object>();
            SQLiteDataReader data = this.exeCommand("SELECT * from Artist");
            while (data.Read())
            {                
                ret.Add(new { Name = data.GetValue(1),  ArtistId = data.GetValue(0)});                
            }
            this.mSqlite.Close();
            return ret;
        }

        public List<object> getAlbums(int ArtistId)
        {
            this.mSqlite = this.openConn();
            List<object> ret = new List<object>();
            SQLiteDataReader data = this.exeCommand($"SELECT AlbumId,Title from Album where ArtistId = {ArtistId}");
            while (data.Read())
            {
                ret.Add(new
                    { AlbumId = data.GetValue(0), Title = data.GetValue(1) }
                );
            }
            this.mSqlite.Close();
            return ret;
        }

        public List<object> getTracks(int AlbumId)
        {
            this.mSqlite = this.openConn();
            List<object> ret = new List<object>();
            SQLiteDataReader data = this.exeCommand($"SELECT TrackId,Name,Milliseconds from Track where AlbumId = {AlbumId}");

            while (data.Read())
            {
                ret.Add(new
                { TrackId = data.GetValue(0), Name = data.GetValue(1), Time = data.GetValue(2) }
                );
            }               
            this.mSqlite.Close();

            return ret;
        }      

        //Example of InnerJoin, not an efficient way to do it here. For example purposes only!!!
        public string getGenre(int AlbumId,int TrackId)
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
            if(this.mSqlite != null && this.mSqlite.State == System.Data.ConnectionState.Open)
            {
                this.mSqlite.Close();
            }
           
            return new SQLiteConnection($"Data Source={this.appPath}{this.pSep}Music.sqlite;Version=3");
        }
    }    
}
