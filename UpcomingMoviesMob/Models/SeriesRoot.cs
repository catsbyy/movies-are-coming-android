using System;
using System.Collections.Generic;
using System.Text;

namespace UpcomingMoviesMob.Models
{
    public class SeriesRoot
    {
        public int page { get; set; }
        public List<Series> results { get; set; }
        public int total_pages { get; set; }
        public int total_results { get; set; }
    }
}
