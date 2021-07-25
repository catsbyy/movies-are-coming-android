using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using Newtonsoft.Json;
using System.Net;
using UpcomingMoviesMob.Models;
using UpcomingMoviesMob.Resources;
using UpcomingMoviesMob.Services;
using System.Globalization;

namespace UpcomingMoviesMob.Core
{
    public class TmdbAPI
    {
        public ObservableCollection<TheMovieDb> MoviesCollectionOnPage = new ObservableCollection<TheMovieDb>();
        public ObservableCollection<Series> SeriesCollectionOnPage = new ObservableCollection<Series>();

        int total_pages;

        public void CallAPI(bool AreMovies)
        {
            string link = "";
            if (AreMovies == true)
            {
                link = AllResources.AllMoviesLinkTemplate + ResourceAPI.api_key + AllResources.SortForLink;
            }
            else
            {
                link = AllResources.TemplatePopularLink + ResourceAPI.api_key;
            }

            HttpWebRequest apiRequest = WebRequest.Create(link) as HttpWebRequest;

            string apiResponse = "";
            using (HttpWebResponse response = apiRequest.GetResponse() as HttpWebResponse)
            {
                StreamReader reader = new StreamReader(response.GetResponseStream());
                apiResponse = reader.ReadToEnd();
            }

            if (AreMovies == true)
            {
                ResponseSearchMovies rootObject = JsonConvert.DeserializeObject<ResponseSearchMovies>(apiResponse);

                total_pages = rootObject.total_pages;
                GetMovies(1, false);
            }
            else
            {
                SeriesRoot rootObject = JsonConvert.DeserializeObject<SeriesRoot>(apiResponse);

                total_pages = rootObject.total_pages;
                GetSeries(1, false);
            }  
        }

        public ObservableCollection<TrendingItem> GetSearchResults(string query)
        {
            ObservableCollection<TrendingItem> searchResultsCollection = new ObservableCollection<TrendingItem>();

            query = query.Replace(" ", "%20");

            string searchLink = AllResources.SearchLinkTemplate + ResourceAPI.api_key + AllResources.LanguageAndQueryTemplateForLink + query + AllResources.PageForLink + "1";

            HttpWebRequest apiRequest = WebRequest.Create(searchLink) as HttpWebRequest;
            string apiResponse = "";
            try
            {
                using (HttpWebResponse response = apiRequest.GetResponse() as HttpWebResponse)
                {
                    StreamReader reader = new StreamReader(response.GetResponseStream());
                    apiResponse = reader.ReadToEnd();
                }
                TrendingRoot rootObject = JsonConvert.DeserializeObject<TrendingRoot>(apiResponse);
                foreach (TrendingItem result in rootObject.results)
                {
                    TrendingItem trendingItem = new TrendingItem();
                    if (result.media_type == "movie")
                    {
                        trendingItem.poster_path = CreatePosterPath(result.poster_path);
                        trendingItem.title = result.title;
                        trendingItem.release_date = RightDateFormat(result.release_date);
                    }
                    else if (result.media_type == "tv")
                    {
                        trendingItem.poster_path = CreatePosterPath(result.poster_path);
                        trendingItem.title = result.name;
                        trendingItem.release_date = RightDateFormat(result.first_air_date);
                    }
                    else if (result.media_type == "person")
                    {
                        trendingItem.poster_path = CreatePosterPath(result.profile_path);
                        trendingItem.title = result.name;
                        //trendingItem.release_date = RightDateFormat(result.first_air_date);
                    }
                    else 
                    {
                        continue;
                    }
                    trendingItem.id = result.id;
                    trendingItem.media_type = result.media_type;

                    searchResultsCollection.Add(trendingItem);
                }
            }
            catch
            { }
            return searchResultsCollection;
        }
        private string RightDateFormat(string date)
        {
            string new_date = "";
            if (date !="")
            {
                string[] subs = date.Split('-');
                new_date = subs[2] + "." + subs[1] + "." + subs[0];
            }
            return new_date;
        }
        public void GetSeries(int page_number, bool isInfiniteScrollEnabled)
        {
            if (isInfiniteScrollEnabled == false)
            {
                SeriesCollectionOnPage.Clear();
            }

            string popularSeriesLink = AllResources.TemplatePopularLink + ResourceAPI.api_key + AllResources.PageForLink + page_number + AllResources.RegionForLink;

            HttpWebRequest apiRequest = WebRequest.Create(popularSeriesLink) as HttpWebRequest;
            string apiResponse = "";
            try
            {
                using (HttpWebResponse response = apiRequest.GetResponse() as HttpWebResponse)
                {
                    StreamReader reader = new StreamReader(response.GetResponseStream());
                    apiResponse = reader.ReadToEnd();
                }
                SeriesRoot rootObject = JsonConvert.DeserializeObject<SeriesRoot>(apiResponse);
                foreach (Series result in rootObject.results)
                {
                    Series series = new Series();
                    series.name = result.name;
                    series.poster_path = CreatePosterPath(result.poster_path);
                    series.first_air_date = RightDateFormat(result.first_air_date);
                    series.id = result.id;

                    SeriesCollectionOnPage.Add(series);
                }
            }
            catch
            { }
        }
        public void GetMovies(int page_number, bool isInfiniteScrollEnabled)
        {
            if (isInfiniteScrollEnabled == false)
            {
                MoviesCollectionOnPage.Clear();
            }

            string upcomingMoviesLink = AllResources.AllMoviesLinkTemplate + ResourceAPI.api_key + AllResources.PageForLink + page_number + AllResources.RegionForLink;

            HttpWebRequest apiRequest = WebRequest.Create(upcomingMoviesLink) as HttpWebRequest;
            string apiResponse = "";
            try
            {
                using (HttpWebResponse response = apiRequest.GetResponse() as HttpWebResponse)
                {
                    StreamReader reader = new StreamReader(response.GetResponseStream());
                    apiResponse = reader.ReadToEnd();
                }
                ResponseSearchMovies rootObject = JsonConvert.DeserializeObject<ResponseSearchMovies>(apiResponse);
                foreach (Result result in rootObject.results)
                {
                    TheMovieDb movie = new TheMovieDb();
                    movie.title = result.title;
                    movie.poster_path = CreatePosterPath(result.poster_path);
                    movie.release_date = RightDateFormat(result.release_date);
                    movie.id = result.id;
                    
                    MoviesCollectionOnPage.Add(movie);
                }
            }
            catch
            {  }
        }

        public PagingInfo GetPagingInfo()
        {
            PagingInfo paging_info = new PagingInfo();
            paging_info.CurrentPage = 1;
            paging_info.TotalPages = total_pages;
            paging_info.FirstPage = 1;
            paging_info.MiddlePage = GetMiddlePage();
            return paging_info;
        }

        private int GetMiddlePage()
        {
            int middlePage;
            if (total_pages%2==0)
            {
                middlePage = total_pages / 2;
            }
            else
            {
                middlePage = (total_pages + 1) / 2;
            }
            return middlePage;
        }

        private string CreatePosterPath(string poster_path)
        {
            string full_posterpath = "";
            if (poster_path != null)
            {
                full_posterpath = AllResources.ForImageURL + poster_path;
            }
            else
            {
                full_posterpath = "no_image.png";
            }
            return full_posterpath;
        }

        private string GetAllCompanies(List<Company> production_companies)
        {
            string all_companies = "";
            if (production_companies.Count != 0)
            {
                foreach (Company company in production_companies)
                {
                    all_companies += company.name + ", ";
                }
                all_companies = all_companies.Substring(0, all_companies.Length - 2);
            }
            return all_companies;
        }

        private string GetDaysLeft(string release_date)
        {
            string daysLeft = "";
            string dateToday = (DateTime.Today).ToString("dd.MM.yyyy");
            if (release_date!="")
            {
                if (dateToday != release_date)
                {
                    daysLeft = (DateTime.ParseExact(release_date, "dd.MM.yyyy", CultureInfo.InvariantCulture) - DateTime.ParseExact(dateToday, "dd.MM.yyyy", CultureInfo.InvariantCulture)).ToString();
                    daysLeft = daysLeft.Substring(0, daysLeft.IndexOf('.'));
                }
                else
                {
                    daysLeft = AllResources.NoDaysLeft;
                }
                if (int.Parse(daysLeft) < 0)
                {
                    daysLeft = daysLeft.Remove(0, 1) + AllResources.ForDaysLeft;

                }
            }
            return daysLeft;
        }

        private string GetAllGenres(List<Genre> genres)
        {
            string all_genres="";
            if (genres.Count != 0)
            {
                foreach (Genre genre in genres)
                {
                    all_genres += genre.name + ", ";
                }
                all_genres = all_genres.Substring(0, all_genres.Length - 2);
            }
            return all_genres;
        }

        private string GetAllCountries(List<Country> countries)
        {
            string all_countries = "";
            if (countries.Count !=0)
            {
                foreach (Country country in countries)
                {
                    all_countries += country.name + ", ";
                }
                all_countries = all_countries.Substring(0, all_countries.Length - 2);
            }
            return all_countries;

        }

        private string GetAllLanguages(List<Language> spoken_languages)
        {
            string all_languages = "";
            if (spoken_languages.Count != 0)
            {
                foreach (Language language in spoken_languages)
                {
                    all_languages += language.english_name + ", ";
                }
                all_languages = all_languages.Substring(0, all_languages.Length - 2);
            }
            return all_languages;
        }

        private string CreateVideoUrl(Video video)
        {
            string url = "";
            if (video.site == "YouTube")
            {
                url = AllResources.YouTubeLinkTemplate + video.key;
            }
            else
            {
                url = AllResources.GoogleLink + video.name;
            }
            return url;
        }
        public ObservableCollection<Video> GetVideos(int id, bool IsMovie)
        {
            ObservableCollection<Video> videoCollection = new ObservableCollection<Video>();
            string link = "";
        
            if (IsMovie ==true)
            {
                link = AllResources.MovieLinkTemplate + id + AllResources.VideoLinkTemplate + ResourceAPI.api_key;
            }
            else
            {
                link = AllResources.SeriesLinkTemplate + id + AllResources.VideoLinkTemplate + ResourceAPI.api_key;
            }

            HttpWebRequest apiRequest = WebRequest.Create(link) as HttpWebRequest;
            string apiResponse = "";
            try
            {
                using (HttpWebResponse response = apiRequest.GetResponse() as HttpWebResponse)
                {
                    StreamReader reader = new StreamReader(response.GetResponseStream());
                    apiResponse = reader.ReadToEnd();
                }
                VideoRoot rootObject = JsonConvert.DeserializeObject<VideoRoot>(apiResponse);
                if (rootObject.results.Count != 0)
                {
                    foreach (Video result in rootObject.results)
                    {
                        Video video = new Video();
                        video.id = result.id;
                        video.iso_3166_1 = result.iso_3166_1;
                        video.iso_639_1 = result.iso_639_1;
                        video.key = result.key;
                        video.name = result.name;
                        video.site = result.site;
                        video.size = result.size;
                        video.type = result.type;

                        video.url = CreateVideoUrl(video);
                        video.thumbnail = AllResources.YouTubeThumbnailTemplate + video.key + AllResources.ThumbnailQuality;

                        videoCollection.Add(video);
                    }
                }
                else
                {
                    Video video = new Video();
                    video.name = AllResources.NoAvailableVideosMessage;

                    video.thumbnail = "no_video.jpg";

                    videoCollection.Add(video);
                }
            }
            catch
            { }
            return videoCollection;
        }

        public ObservableCollection<Cast> GetCast(int id, bool IsMovie)
        {
            ObservableCollection<Cast> castCollection = new ObservableCollection<Cast>();
            string castLink = "";
            if (IsMovie == true)
            {
                castLink = AllResources.MovieLinkTemplate + id + AllResources.CastAndCrewLinkTemplate + ResourceAPI.api_key;
            }
            else
            {
                castLink = AllResources.SeriesLinkTemplate + id + AllResources.CastAndCrewLinkTemplate + ResourceAPI.api_key;
            }

            HttpWebRequest apiRequest = WebRequest.Create(castLink) as HttpWebRequest;
            string apiResponse = "";
            try
            {
                using (HttpWebResponse response = apiRequest.GetResponse() as HttpWebResponse)
                {
                    StreamReader reader = new StreamReader(response.GetResponseStream());
                    apiResponse = reader.ReadToEnd();
                }
                CastAndCrewRoot rootObject = JsonConvert.DeserializeObject<CastAndCrewRoot>(apiResponse);
                foreach (Cast result in rootObject.cast)
                {
                    Cast cast = new Cast();
                    cast.adult = result.adult;
                    cast.cast_id = result.cast_id;
                    cast.character = result.character;
                    cast.credit_id = result.credit_id;
                    cast.gender = result.gender;
                    cast.id = result.id;
                    cast.known_for_department = result.known_for_department;
                    cast.name = result.name;
                    cast.order = result.order;
                    cast.original_name = result.original_name;
                    cast.popularity = result.popularity;
                    cast.profile_path = CreatePosterPath(result.profile_path);

                    castCollection.Add(cast);
                }
            }
            catch
            { }
            return castCollection;
        }

        public PersonInfo GetPersonBio(int id)
        {
            PersonInfo personInfo = new PersonInfo();
            string personBioLink = AllResources.TemplatePersonBio + id + AllResources.ApiKeyForLink + ResourceAPI.api_key;

            HttpWebRequest apiRequest = WebRequest.Create(personBioLink) as HttpWebRequest;
            string apiResponse = "";
            try
            {
                using (HttpWebResponse response = apiRequest.GetResponse() as HttpWebResponse)
                {
                    StreamReader reader = new StreamReader(response.GetResponseStream());
                    apiResponse = reader.ReadToEnd();
                }
                PersonInfo rootObject = JsonConvert.DeserializeObject<PersonInfo>(apiResponse);

                personInfo.adult = rootObject.adult;
                personInfo.also_known_as = rootObject.also_known_as;
                personInfo.biography = rootObject.biography;
                personInfo.birthday = RightDateFormat(rootObject.birthday);
                if (rootObject.deathday != null)
                {
                    personInfo.deathday = RightDateFormat(rootObject.deathday);
                }
                else
                {
                    personInfo.deathday = "";
                }
                personInfo.gender = rootObject.gender;
                personInfo.homepage = rootObject.homepage;
                personInfo.id = rootObject.id;
                personInfo.imdb_id = rootObject.imdb_id;
                personInfo.known_for_department = rootObject.known_for_department;
                personInfo.name = rootObject.name;
                personInfo.place_of_birth = rootObject.place_of_birth;
                personInfo.popularity = rootObject.popularity;
                personInfo.profile_path = CreatePosterPath(rootObject.profile_path);
            }
            catch
            { }
            return personInfo;
        }
        public bool IsDead(PersonInfo person)
        {
            if (person.deathday == "")
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public ObservableCollection<PersonImages> GetPersonImages(int id)
        {
            ObservableCollection<PersonImages> personImagesCollection = new ObservableCollection<PersonImages>();
            string link = AllResources.TemplatePersonBio + id + AllResources.ForImageLinkTemplate + ResourceAPI.api_key;

            HttpWebRequest apiRequest = WebRequest.Create(link) as HttpWebRequest;
            string apiResponse = "";
            try
            {
                using (HttpWebResponse response = apiRequest.GetResponse() as HttpWebResponse)
                {
                    StreamReader reader = new StreamReader(response.GetResponseStream());
                    apiResponse = reader.ReadToEnd();
                }
                PersonImagesRoot rootObject = JsonConvert.DeserializeObject<PersonImagesRoot>(apiResponse);
                if (rootObject.profiles.Count != 0)
                {
                    foreach (PersonImages result in rootObject.profiles)
                    {
                        PersonImages person = new PersonImages();
                        person.aspect_ratio = result.aspect_ratio;
                        person.file_path = CreatePosterPath(result.file_path);
                        person.height = result.height;
                        person.iso_639_1 = result.iso_639_1;
                        person.vote_average = result.vote_average;
                        person.vote_count = result.vote_count;
                        person.width = result.width;

                        personImagesCollection.Add(person);
                    }
                }
                else
                {
                    PersonImages person = new PersonImages();

                    //person.thumbnail = "no_video.jpg";

                    personImagesCollection.Add(person);
                }
            }
            catch
            { }
            return personImagesCollection;
        }

        public bool IsLooped(ObservableCollection<PersonImages> personImages)
        {
            if (personImages.Count > 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public ObservableCollection<Crew> GetDirector(int id)
        {
            ObservableCollection<Crew> crewCollection = new ObservableCollection<Crew>();
            string crewLink = AllResources.MovieLinkTemplate + id + AllResources.CastAndCrewLinkTemplate + ResourceAPI.api_key;

            HttpWebRequest apiRequest = WebRequest.Create(crewLink) as HttpWebRequest;
            string apiResponse = "";
            try
            {
                using (HttpWebResponse response = apiRequest.GetResponse() as HttpWebResponse)
                {
                    StreamReader reader = new StreamReader(response.GetResponseStream());
                    apiResponse = reader.ReadToEnd();
                }
                CastAndCrewRoot rootObject = JsonConvert.DeserializeObject<CastAndCrewRoot>(apiResponse);
                foreach (Crew result in rootObject.crew)
                {
                    if (result.job == "Director")
                    {
                        Crew crew = new Crew();
                        crew.adult = result.adult;
                        crew.credit_id = result.credit_id;
                        crew.department = result.department;
                        crew.gender = result.gender;
                        crew.id = result.id;
                         crew.job = result.job;
                        crew.known_for_department = result.known_for_department;
                        crew.name = result.name;
                        crew.original_name = result.original_name;
                        crew.profile_path = CreatePosterPath(result.profile_path);
                        crew.popularity = result.popularity;

                        crewCollection.Add(crew);
                    }
                    else
                    {
                        continue;
                    }
                }
            }
            catch
            { }
            return crewCollection;
        }

        private TheMovieDb CreateMovie(TheMovieDb movie, TheMovieDb rootObject)
        {
            movie.genres = rootObject.genres;
            movie.all_genres = GetAllGenres(movie.genres);
            movie.title = rootObject.title;
            movie.original_title = rootObject.original_title;
            movie.overview = rootObject.overview;
            movie.poster_path = CreatePosterPath(rootObject.poster_path);
            movie.production_countries = rootObject.production_countries;

            movie.all_countries = GetAllCountries(movie.production_countries);
            movie.production_companies = rootObject.production_companies;
            movie.all_companies = GetAllCompanies(movie.production_companies);
            movie.release_date = RightDateFormat(rootObject.release_date);
            movie.runtime = rootObject.runtime;
            movie.tagline = rootObject.tagline;
            movie.vote_average = rootObject.vote_average;
            movie.spoken_languages = rootObject.spoken_languages;
            movie.all_languages = GetAllLanguages(movie.spoken_languages);
            movie.status = rootObject.status;
            movie.days_left = GetDaysLeft(movie.release_date);
            
            return movie;
        }

        public ObservableCollection<TrendingItem> GetTrendingToday()
        {
            ObservableCollection<TrendingItem> trendingItemsCollection = new ObservableCollection<TrendingItem>();

            string trendingTodayLink = AllResources.TemplateTrendingToday + ResourceAPI.api_key;

            HttpWebRequest apiRequest = WebRequest.Create(trendingTodayLink) as HttpWebRequest;
            string apiResponse = "";
            try
            {
                using (HttpWebResponse response = apiRequest.GetResponse() as HttpWebResponse)
                {
                    StreamReader reader = new StreamReader(response.GetResponseStream());
                    apiResponse = reader.ReadToEnd();
                }
                TrendingRoot rootObject = JsonConvert.DeserializeObject<TrendingRoot>(apiResponse);
                foreach (TrendingItem result in rootObject.results)
                {
                    TrendingItem trendingItem = new TrendingItem();
                    if (result.media_type == "movie")
                    {
                        trendingItem.poster_path = CreatePosterPath(result.poster_path);
                        trendingItem.title = result.title;
                        trendingItem.release_date = RightDateFormat(result.release_date);
                    }
                    else if (result .media_type == "tv")
                    {
                        trendingItem.poster_path = CreatePosterPath(result.poster_path);
                        trendingItem.title = result.name;
                        trendingItem.release_date = RightDateFormat(result.first_air_date);
                    }
                    else
                    {
                        continue;
                    }
                    trendingItem.id = result.id;
                    trendingItem.media_type = result.media_type;

                    trendingItemsCollection.Add(trendingItem);
                }
            }
            catch
            { }
            return trendingItemsCollection;
        }
        public ObservableCollection<TrendingItem> GetTrendingWeek()
        {
            ObservableCollection<TrendingItem> trendingItemsCollection = new ObservableCollection<TrendingItem>();

            string trendingWeekLink = AllResources.TemplateTrendingWeek + ResourceAPI.api_key;

            HttpWebRequest apiRequest = WebRequest.Create(trendingWeekLink) as HttpWebRequest;
            string apiResponse = "";
            try
            {
                using (HttpWebResponse response = apiRequest.GetResponse() as HttpWebResponse)
                {
                    StreamReader reader = new StreamReader(response.GetResponseStream());
                    apiResponse = reader.ReadToEnd();
                }
                TrendingRoot rootObject = JsonConvert.DeserializeObject<TrendingRoot>(apiResponse);
                foreach (TrendingItem result in rootObject.results)
                {
                    TrendingItem trendingItem = new TrendingItem();
                    if (result.media_type == "movie")
                    {
                        trendingItem.poster_path = CreatePosterPath(result.poster_path);
                        trendingItem.title = result.title;
                        trendingItem.release_date = RightDateFormat(result.release_date);
                    }
                    else if (trendingItem.media_type == "tv")
                    {
                        trendingItem.poster_path = CreatePosterPath(result.poster_path);
                        trendingItem.title = result.name;
                        trendingItem.release_date = RightDateFormat(result.first_air_date);
                    }
                    else
                    {
                        continue;
                    }
                    trendingItem.id = result.id;
                    trendingItem.media_type = result.media_type;

                    trendingItemsCollection.Add(trendingItem);
                }
            }
            catch
            { }
            return trendingItemsCollection;
        }
        public TheMovieDb GetMovieDetails(int movieId)
        {
            TheMovieDb movie = new TheMovieDb();

            string movieLink = AllResources.MovieLinkTemplate + movieId + AllResources.ApiKeyForLink + ResourceAPI.api_key + AllResources.RegionForLink;
            HttpWebRequest apiRequest = WebRequest.Create(movieLink) as HttpWebRequest;

            string apiResponse = "";
            try
            {
                using (HttpWebResponse response = apiRequest.GetResponse() as HttpWebResponse)
                {
                    StreamReader reader = new StreamReader(response.GetResponseStream());
                    apiResponse = reader.ReadToEnd();
                }

                TheMovieDb rootObject = JsonConvert.DeserializeObject<TheMovieDb>(apiResponse);
                
                movie = CreateMovie(movie, rootObject);

                return movie;
            }
            catch
            {
                return movie;
            }
        }
        public Series GetSeriesDetails(int seriesId)
        {
            Series series = new Series();

            string seriesLink = AllResources.TemplateSeriesLink + seriesId + AllResources.ApiKeyForLink + ResourceAPI.api_key + AllResources.RegionForLink;
            HttpWebRequest apiRequest = WebRequest.Create(seriesLink) as HttpWebRequest;

            string apiResponse = "";
            try
            {
                using (HttpWebResponse response = apiRequest.GetResponse() as HttpWebResponse)
                {
                    StreamReader reader = new StreamReader(response.GetResponseStream());
                    apiResponse = reader.ReadToEnd();
                }

                Series rootObject = JsonConvert.DeserializeObject<Series>(apiResponse);

                series = CreateSeries(series, rootObject);

                return series;
            }
            catch
            {
                return series;
            }
        }
        private Series CreateSeries(Series series, Series rootObject)
        {
            series.first_air_date = RightDateFormat(rootObject.first_air_date);
            series.genre_ids = rootObject.genre_ids;
            series.id = rootObject.id;
            series.name = rootObject.name;
            series.original_language = new CultureInfo(rootObject.original_language).DisplayName;
            series.original_name = rootObject.original_name;
            series.overview = rootObject.overview;
            series.popularity = rootObject.popularity;
            series.poster_path = CreatePosterPath(rootObject.poster_path);
            series.vote_average = rootObject.vote_average;

            series.origin_country = rootObject.origin_country;
            string countryCode = rootObject.origin_country[0];
            series.all_countries = new RegionInfo(countryCode).DisplayName;
            
            return series;
        }
    }
}
