using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Input;
using UpcomingMoviesMob.Core;
using UpcomingMoviesMob.Models;
using UpcomingMoviesMob.Resources;
using UpcomingMoviesMob.Views;
using Xamarin.Forms;


namespace UpcomingMoviesMob.ViewModels
{
    public class SearchViewModel : BaseViewModel
    {
        private ObservableCollection<TrendingItem> searchCollectionOnPage { get; set; }
        public Command<TrendingItem> ItemTapped { get; }
        private TrendingItem _selectedItem;
        TmdbAPI tmdb = new TmdbAPI();
        private string query { get; set; }
        public string Query
        {
            get { return query; }
            set
            {
                if (query != value)
                {
                    query = value;
                    OnPropertyChanged("Query");
                }
            }
        }
        public ObservableCollection<TrendingItem> SearchCollectionOnPage
        {
            get { return searchCollectionOnPage; }
            set
            {
                if (searchCollectionOnPage != value)
                {
                    searchCollectionOnPage = value;
                    OnPropertyChanged("SearchCollectionOnPage");
                }
            }
        }
        public SearchViewModel()
        {
            Title = "Search";
            //SearchCollectionOnPage = tmdb.GetSearchResults();

            //paging_info = tmdb.GetPagingInfo();
            //IsWorking = false;
            //IsNavigationButtonsVisible = true;

            ItemTapped = new Command<TrendingItem>(OnItemSelected);
        }
        public void OnAppearing()
        {
            IsBusy = true;
            SelectedItem = null;
        }

        public TrendingItem SelectedItem
        {
            get => _selectedItem;
            set
            {
                SetProperty(ref _selectedItem, value);
                OnItemSelected(value);
            }
        }

        async void OnItemSelected(TrendingItem item)
        {
            if (item == null)
                return;

            if (item.media_type == "movie")
            {
                await Shell.Current.GoToAsync($"{nameof(MovieDetailPage)}?{nameof(MovieDetailViewModel.MovieId)}={item.id}");
            }
            else if (item.media_type == "tv")
            {
                await Shell.Current.GoToAsync($"{nameof(SeriesDetailPage)}?{nameof(SeriesDetailViewModel.SeriesId)}={item.id}");
            }
            else if (item.media_type == "person")
            {
                await Shell.Current.GoToAsync($"{nameof(PersonPage)}?{nameof(PersonViewModel.PersonId)}={item.id}");
            }
            //await Shell.Current.GoToAsync($"{nameof(SeriesDetailPage)}?{nameof(SeriesDetailViewModel.SeriesId)}={series.id}");
        }
        public ICommand SearchForSomething
        {
            get
            {
                return new Command(o =>
                {
                    if (query != null)
                    {
                        searchCollectionOnPage = tmdb.GetSearchResults(query);
                        OnPropertyChanged("SearchCollectionOnPage");
                        //App.Current.MainPage.DisplayAlert("", query, "OK");
                    }
                    else
                    {
                        App.Current.MainPage.DisplayAlert("", AllResources.NoMorePagesMessage, "OK");
                    }
                });
            }
        }
    }
}
