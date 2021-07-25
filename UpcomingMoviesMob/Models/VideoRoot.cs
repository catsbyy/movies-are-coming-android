using System;
using System.Collections.Generic;
using System.Text;

namespace UpcomingMoviesMob.Models
{
    public class VideoRoot
    {
        public int id { get; set; }
        public List<Video> results { get; set; }
    }
}
