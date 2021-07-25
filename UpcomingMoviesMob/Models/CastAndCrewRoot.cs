using System;
using System.Collections.Generic;
using System.Text;

namespace UpcomingMoviesMob.Models
{
    public class CastAndCrewRoot
    {
        public int id { get; set; }
        public List<Cast> cast { get; set; }
        public List<Crew> crew { get; set; }
    }
}
