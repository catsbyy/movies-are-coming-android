using System;
using System.Collections.Generic;
using System.Text;

namespace UpcomingMoviesMob.Models
{
    public class PersonImagesRoot
    {
        public int id { get; set; }
        public List<PersonImages> profiles { get; set; }
    }
}
