using System;
using System.IO;
using System.Net.Http;
using ReleaseInfo;
using Newtonsoft.Json;
using System.Windows.Media.Imaging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Dbase_example
{
    class AlbumArt
    {
        /* Recommended way of using HttpClient by multiple sources.
         * Reason being is to help cut down on opening so many ports on remote host/server.
         * In general this is also faster since you are not renegotiating a session with
         * the server for every use.
        */
        private static HttpClient webConn = new HttpClient();
        private string mBrainzApi,mCoverArtApi;
        private List<string> MBIDs;


        public AlbumArt()
        {
            this.mBrainzApi = "https://musicbrainz.org/ws/2/release/?query=release:";
            this.mCoverArtApi = "https://coverartarchive.org/release/";     
        }        
        
        public BitmapImage getFrontCover(string artist, string album)       {

            if(album.Contains("(") || album.Contains("["))
            {
                if (album.Contains("("))
                {
                    album = album.Replace("(", "");
                    album = album.Replace(")", "");
                }
                if (album.Contains("["))
                {
                    album = album.Replace("[", "");
                    album = album.Replace("]", "");
                }
            }
            Console.WriteLine(album);
            this.connBrainz(artist, album);
            if (this.MBIDs.Count < 1)
                return null;
            // Setting up HTTP Request for type image
            webConn.DefaultRequestHeaders.Clear();
            webConn.DefaultRequestHeaders.UserAgent.ParseAdd("Mozilla/5.0 (X11; Linux x86_64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/44.0.2403.157 Safari/537.36");

            BitmapImage bImg;

            MemoryStream ms;
            foreach (string m in this.MBIDs)
                Console.WriteLine(m);
            foreach (string mbid in this.MBIDs)
            {
                string addr = $"{this.mCoverArtApi}{mbid}/front";
                // CoverArt Archive api call
                string url = $"{this.mCoverArtApi}{mbid}/front";
                HttpResponseMessage msg = webConn.GetAsync(new Uri(url)).Result;
                if (msg.IsSuccessStatusCode)
                {
                    Console.WriteLine(url);
                    ms = new MemoryStream(msg.Content.ReadAsByteArrayAsync().Result);
                    bImg = new BitmapImage();
                    bImg.BeginInit();
                    bImg.StreamSource = ms;
                    bImg.EndInit();
                    bImg.Freeze();
                    return bImg;
                }
            }
            return null;     
        }

        // MusicBrainz api call
        private void connBrainz(string artist,string album)
        {
            this.MBIDs = new List<string>();
            // Setting up HTTP Request Header for JSON
            webConn.DefaultRequestHeaders.Clear();
            webConn.DefaultRequestHeaders.Accept.Add(
                new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

            // MusicBrainz requires a user agent, or blocked(it can be unique, actually it's suggested)
            webConn.DefaultRequestHeaders.UserAgent.ParseAdd("csharp_demo_app_0.1");

            // Actual request being made to MusicBrainz server
            string q = $"{this.mBrainzApi}\"{album}\" AND artist:\"{artist}\" &limit=3";
            Console.WriteLine(q);
            string ret = webConn.GetStringAsync(new Uri(q)).Result;

            // JSON response being deserialized into an Object
            Rootobject ro = JsonConvert.DeserializeObject<Rootobject>(ret);     
            
            // Getting multiple MBIDs to try and make sure cover art is available
            foreach (Release r in ro.releases)
                this.MBIDs.Add(r.id);
        }        
    }
}
