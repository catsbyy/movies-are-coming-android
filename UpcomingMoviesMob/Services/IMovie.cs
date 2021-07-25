using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UpcomingMoviesMob.Models;

namespace UpcomingMoviesMob.Services
{
    interface IMovie
    {
        List<Genre> genres { get; set; }
        string all_genres { get; set; }
        int id { get; set; }
        string original_title { get; set; }
        string overview { get; set; }
        string poster_path { get; set; }
        List<Country> production_countries { get; set; }
        string all_countries { get; set; }
        List<Company> production_companies { get; set; }
        string all_companies { get; set; }
        string release_date { get; set; }
        string days_left { get; set; }
        int runtime { get; set; }
        List<Language> spoken_languages { get; set; }
        string all_languages { get; set; }
        string status { get; set; }
        string tagline { get; set; }
        string title { get; set; }
        double vote_average { get; set; }
    }
}
