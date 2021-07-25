using System;
using System.Diagnostics;
using System.Threading.Tasks;
using UpcomingMoviesMob.Models;
using UpcomingMoviesMob.Core;
using Xamarin.Forms;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using UpcomingMoviesMob.Resources;
using UpcomingMoviesMob.Views;

namespace UpcomingMoviesMob.ViewModels
{
    [QueryProperty(nameof(MovieId), nameof(MovieId))]
    public class MovieDetailViewModel : INotifyPropertyChanged
    {
        TmdbAPI tmdb = new TmdbAPI();
        private TheMovieDb movie { get; set; }
        private ObservableCollection<Video> videoCollection { get; set; }
        private ObservableCollection<Cast> castCollection { get; set; }
        private ObservableCollection<Crew> crewCollection { get; set; }
        private int movieId;
        public string Title { get; set; } = "Movie Details";
        public TheMovieDb Movie
        {
            get { return movie; }
            set
            {
                if (movie != value)
                {
                    movie = value;
                    OnPropertyChanged("Movie");
                }
            }
        }
        public ObservableCollection<Video> VideoCollection
        {
            get { return videoCollection; }
            set
            {
                if (videoCollection != value)
                {
                    videoCollection = value;
                    OnPropertyChanged("VideoCollection");
                }
            }
        }
        public ObservableCollection<Cast> CastCollection
        {
            get { return castCollection; }
            set
            {
                if (castCollection != value)
                {
                    castCollection = value;
                    OnPropertyChanged("CastCollection");
                }
            }
        }
        public ObservableCollection<Crew> CrewCollection
        {
            get { return crewCollection; }
            set
            {
                if (crewCollection != value)
                {
                    crewCollection = value;
                    OnPropertyChanged("CrewCollection");
                }
            }
        }
        public int MovieId
        {
            get
            {
                return movieId;
            }
            set
            {
                movieId = value;
                movie = tmdb.GetMovieDetails(movieId);
                videoCollection = tmdb.GetVideos(MovieId, true);
                castCollection = tmdb.GetCast(MovieId, true);
                crewCollection = tmdb.GetDirector(MovieId);
                OnPropertyChanged("MovieId");
                OnPropertyChanged("Movie");
                OnPropertyChanged("VideoCollection");
                OnPropertyChanged("CastCollection");
                OnPropertyChanged("CrewCollection");
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }

        public ICommand PersonTapped
        {
            get
            {
                return new Command(o =>
                {
                    if (o != null)
                    {
                        int id;
                        try
                        {
                            id = Convert.ToInt32(((Cast)o).id);
                        }
                        catch
                        {
                            id = Convert.ToInt32(((Crew)o).id);
                        }

                        Shell.Current.GoToAsync($"{nameof(PersonPage)}?{nameof(PersonViewModel.PersonId)}={id}");
                    }
                });
            }
        }
    }
}
