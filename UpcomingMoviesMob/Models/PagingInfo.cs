using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UpcomingMoviesMob.Models
{
    public class PagingInfo
    {
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public int FirstPage { get; set; }
        public int MiddlePage { get; set; }
    }
}
