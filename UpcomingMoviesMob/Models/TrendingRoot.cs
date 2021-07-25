using System;
using System.Collections.Generic;
using System.Text;

namespace UpcomingMoviesMob.Models
{
    public class TrendingRoot
    {
        public int page { get; set; }
        public List<TrendingItem> results { get; set; }
        public int total_Pages { get; set; }
        public int total_results { get; set; }
    }
}
