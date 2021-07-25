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
    public class DiscoveryViewModel : BaseViewModel
    {
        private TheMovieDb _selectedMovie;
        public TmdbAPI tmdb = new TmdbAPI();
        PagingInfo paging_info;
        public bool IsWorking { get; set; } = true;
        public ObservableCollection<TheMovieDb> MoviesCollectionOnPage { get; set;}
        public Command<TheMovieDb> MovieTapped { get; }

        public DiscoveryViewModel()
        {
            Title = "Discover";
            tmdb.CallAPI(true);
            MoviesCollectionOnPage = tmdb.MoviesCollectionOnPage;
            
            paging_info = tmdb.GetPagingInfo();
            IsWorking = false;
            IsNavigationButtonsVisible = true;
            
            MovieTapped = new Command<TheMovieDb>(OnMovieSelected);
        }

        public void OnAppearing()
        {
            IsBusy = true;
            SelectedMovie = null;
        }

        public TheMovieDb SelectedMovie
        {
            get => _selectedMovie;
            set
            {
                SetProperty(ref _selectedMovie, value);
                OnMovieSelected(value);
            }
        }

        async void OnMovieSelected(TheMovieDb theMovieDb)
        {
            if (theMovieDb == null)
                return;

            // This will push the ItemDetailPage onto the navigation stack
            await Shell.Current.GoToAsync($"{nameof(MovieDetailPage)}?{nameof(MovieDetailViewModel.MovieId)}={theMovieDb.id}");
        }
        public ICommand GoToNextPage
        {
            get
            {
                return new Command(o =>
                {
                    if (paging_info.CurrentPage != paging_info.TotalPages)
                    {
                        paging_info.CurrentPage++;
                        tmdb.GetMovies(paging_info.CurrentPage, IsInfiniteScrollEnabled);
                    }
                    else
                    {
                        App.Current.MainPage.DisplayAlert("", AllResources.NoMorePagesMessage, "OK");
                    }
                });
            }
        }
        public ICommand GoToPreviousPage
        {
            get
            {
                return new Command(o =>
                {
                    if (paging_info.CurrentPage != paging_info.FirstPage)
                    {
                        paging_info.CurrentPage--;
                        tmdb.GetMovies(paging_info.CurrentPage, IsInfiniteScrollEnabled);
                    }
                    else
                    {
                        App.Current.MainPage.DisplayAlert("", AllResources.NoMorePagesMessage, "OK");
                    }
                });
            }
        }

        public ICommand GoToFirstPage
        {
            get
            {
                return new Command(o =>
                {
                    paging_info.CurrentPage = paging_info.FirstPage;
                    tmdb.GetMovies(paging_info.CurrentPage, IsInfiniteScrollEnabled);
                });
            }
        }
        public ICommand GoToLastPage
        {
            get
            {
                return new Command(o =>
                {
                    paging_info.CurrentPage = paging_info.TotalPages;
                    tmdb.GetMovies(paging_info.CurrentPage, IsInfiniteScrollEnabled);
                });
            }
        }
        public ICommand GoToMiddlePage
        {
            get
            {
                return new Command(o =>
                {
                    paging_info.CurrentPage = paging_info.MiddlePage;
                    tmdb.GetMovies(paging_info.CurrentPage, IsInfiniteScrollEnabled);
                });
            }
        }

        private bool IsInfiniteScrollEnabled { get; set; } = false;
        private bool isNavigationButtonsVisible { get; set; }
        public bool IsNavigationButtonsVisible
        {
            get { return isNavigationButtonsVisible; }
            set
            {
                isNavigationButtonsVisible = value;
                OnPropertyChanged("IsNavigationButtonsVisible");
            }
        }
        private Command loadMoreMoviesCommand;
        public Command LoadMoreMoviesCommand 
        {
            get { return loadMoreMoviesCommand; }
            set
            {
                loadMoreMoviesCommand = value;
                OnPropertyChanged("LoadMoreMoviesCommand");
            }
        }
        public ICommand EnableInfiniteScroll
        {
            get
            {
                return new Command(o =>
                {
                    paging_info.CurrentPage = 1;
                    MoviesCollectionOnPage.Clear();
                    LoadMoreMoviesAsync();
                    LoadMoreMoviesCommand = new Command(LoadMoreMoviesAsync);
                    IsInfiniteScrollEnabled = !IsInfiniteScrollEnabled;
                    IsNavigationButtonsVisible = !IsNavigationButtonsVisible;
                });
            }
        }
        private async void LoadMoreMoviesAsync()
        {
            if (IsWorking)
                return;

            IsWorking = true;
            await Task.Delay(1000);

            tmdb.GetMovies(paging_info.CurrentPage, IsInfiniteScrollEnabled);
            paging_info.CurrentPage++;
            IsWorking = false;
        }
    }
}