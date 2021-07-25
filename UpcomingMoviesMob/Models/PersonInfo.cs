using System;
using System.Collections.Generic;
using System.Text;

namespace UpcomingMoviesMob.Models
{
    public class PersonInfo
    {
        public bool adult { get; set; }
        public List<string> also_known_as { get; set; }
        public string biography { get; set; }
        public string birthday { get; set; }
        public string deathday { get; set; }
        public int gender { get; set; }
        public object homepage { get; set; }
        public int id { get; set; }
        public string imdb_id { get; set; }
        public string known_for_department { get; set; }
        public string name { get; set; }
        public string place_of_birth { get; set; }
        public double popularity { get; set; }
        public string profile_path { get; set; }
        ///person's bio and else
        ///

    }
    public class PersonMoviesRoot
    {
        public List<Cast> cast { get; set; }
        public List<Crew> crew { get; set; }
        public int id { get; set; }
    }
}
