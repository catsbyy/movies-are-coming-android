using System;
using System.Diagnostics;
using System.Threading.Tasks;
using UpcomingMoviesMob.Models;
using UpcomingMoviesMob.Core;
using Xamarin.Forms;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using UpcomingMoviesMob.Views;

namespace UpcomingMoviesMob.ViewModels
{
    [QueryProperty(nameof(SeriesId), nameof(SeriesId))]
    public class SeriesDetailViewModel : INotifyPropertyChanged
    {
        TmdbAPI tmdb = new TmdbAPI();
        private Series series { get; set; }
        private ObservableCollection<Video> videoCollection { get; set; }
        private ObservableCollection<Cast> castCollection { get; set; }
        private ObservableCollection<Crew> crewCollection { get; set; }
        private int seriesId;
        public string Title { get; set; } = "Series Details";
        public Series TvSeries
        {
            get { return series; }
            set
            {
                if (series != value)
                {
                    series = value;
                    OnPropertyChanged("vSeries");
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
        public int SeriesId
        {
            get
            {
                return seriesId;
            }
            set
            {
                seriesId = value;
                series = tmdb.GetSeriesDetails(seriesId);
                videoCollection = tmdb.GetVideos(seriesId, false);
                castCollection = tmdb.GetCast(seriesId, false);

                OnPropertyChanged("SeriesId");
                OnPropertyChanged("TvSeries");
                OnPropertyChanged("VideoCollection");
                OnPropertyChanged("CastCollection");
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
