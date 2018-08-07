using System;
using System.Net.Http;
using ReleaseInfo;
using Newtonsoft.Json;

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

        public AlbumArt()
        {
            this.mBrainzApi = "https://musicbrainz.org/ws/2/release/?query=limit:3&release:";
            this.mCoverArtApi = "https://coverartarchive.org/release/";

            //Setting up HTTP Request headers, MusicBrainz requires a client name string
            webConn.DefaultRequestHeaders.Accept.Add(
                new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            webConn.DefaultRequestHeaders.UserAgent.ParseAdd("csharp_demo_app_0.1");
        }

        // Token for requesting album art from coverartarchive.org
        public string getMBIDToken(string artist,string album)
        {
            this.connBrainz(artist, album);
            return null;
        }

        private string connBrainz(string artist,string album)
        {
            string ret = webConn.GetStringAsync(new Uri($"{this.mBrainzApi}\"{album}\" AND artist:\"{artist}\"")).Result;
            Rootobject ro = JsonConvert.DeserializeObject<Rootobject>(ret);
            return ro.releases[0].id;
        }
    }
}
