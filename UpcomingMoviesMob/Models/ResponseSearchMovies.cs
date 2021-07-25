using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UpcomingMoviesMob.Models
{
    class ResponseSearchMovies
    {
        public List<Result> results { get; set; }
        public int total_pages { get; set; }
    }
}
