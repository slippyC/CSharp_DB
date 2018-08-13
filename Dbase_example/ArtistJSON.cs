using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtistJSON
{

    public class Rootobject
    {
        public DateTime created { get; set; }
        public int count { get; set; }
        public int offset { get; set; }
        public Artist[] artists { get; set; }
    }

    public class Artist
    {
        public string id { get; set; }
        public string type { get; set; }
        public string score { get; set; }
        public string name { get; set; }
        public string sortname { get; set; }
        public string country { get; set; }
        public Area area { get; set; }
        public BeginArea beginarea { get; set; }
        public string disambiguation { get; set; }
        public LifeSpan lifespan { get; set; }
        public Alias[] aliases { get; set; }
        public Tag[] tags { get; set; }
    }

    public class Area
    {
        public string id { get; set; }
        public string name { get; set; }
        public string sortname { get; set; }
    }

    public class BeginArea
    {
        public string id { get; set; }
        public string name { get; set; }
        public string sortname { get; set; }
    }

    public class LifeSpan
    {
        public string begin { get; set; }
        public string end { get; set; }
        public bool? ended { get; set; }
    }

    public class Alias
    {
        public string sortname { get; set; }
        public string name { get; set; }
        public object locale { get; set; }
        public object type { get; set; }
        public object primary { get; set; }
        public object begindate { get; set; }
        public object enddate { get; set; }
    }

    public class Tag
    {
        public int count { get; set; }
        public string name { get; set; }
    }

}
