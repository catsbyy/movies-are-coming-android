using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UpcomingMoviesMob.Services;

namespace UpcomingMoviesMob.Models
{
    public class TheMovieDb : IMovie
    {
        public List<Genre> genres { get; set; }
        public string all_genres { get; set; }
        public int id { get; set; }
        public string original_title { get; set; }
        public string overview { get; set; }
        public string poster_path { get; set; }
        public List<Country> production_countries { get; set; }
        public string all_countries { get; set; }
        public List<Company> production_companies { get; set; }
        public string all_companies { get; set; }
        public string release_date { get; set; }
        public string days_left { get; set; }
        public int runtime { get; set; }
        public List<Language> spoken_languages { get; set; }
        public string all_languages { get; set; }
        public string status { get; set; }
        public string tagline { get; set; }
        public string title { get; set; }
        public double vote_average { get; set; }
    }
}
