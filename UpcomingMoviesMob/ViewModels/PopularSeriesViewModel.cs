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
    public class PopularSeriesViewModel : BaseViewModel
    {
        private Series _selectedSeries;
        public TmdbAPI tmdb = new TmdbAPI();
        PagingInfo paging_info;
        public bool IsWorking { get; set; } = true;
        public ObservableCollection<Series> SeriesCollectionOnPage { get; set; }
        public Command<Series> SeriesTapped { get; }

        public PopularSeriesViewModel()
        {
            Title = "Popular Series";
            tmdb.CallAPI(false);
            SeriesCollectionOnPage = tmdb.SeriesCollectionOnPage;

            paging_info = tmdb.GetPagingInfo();
            IsWorking = false;
            IsNavigationButtonsVisible = true;

            SeriesTapped = new Command<Series>(OnSeriesSelected);
        }

        public void OnAppearing()
        {
            IsBusy = true;
            SelectedSeries = null;
        }

        public Series SelectedSeries
        {
            get => _selectedSeries;
            set
            {
                SetProperty(ref _selectedSeries, value);
                OnSeriesSelected(value);
            }
        }

        async void OnSeriesSelected(Series series)
        {
            if (series == null)
                return;

            // This will push the ItemDetailPage onto the navigation stack
            await Shell.Current.GoToAsync($"{nameof(SeriesDetailPage)}?{nameof(SeriesDetailViewModel.SeriesId)}={series.id}");
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
                        tmdb.GetSeries(paging_info.CurrentPage, IsInfiniteScrollEnabled);
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
                        tmdb.GetSeries(paging_info.CurrentPage, IsInfiniteScrollEnabled);
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
                    tmdb.GetSeries(paging_info.CurrentPage, IsInfiniteScrollEnabled);
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
                    tmdb.GetSeries(paging_info.CurrentPage, IsInfiniteScrollEnabled);
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
                    tmdb.GetSeries(paging_info.CurrentPage, IsInfiniteScrollEnabled);
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
        private Command loadMoreSeriesCommand;
        public Command LoadMoreSeriesCommand
        {
            get { return loadMoreSeriesCommand; }
            set
            {
                loadMoreSeriesCommand = value;
                OnPropertyChanged("LoadMoreSeriesCommand");
            }
        }
        public ICommand EnableInfiniteScroll
        {
            get
            {
                return new Command(o =>
                {
                    paging_info.CurrentPage = 1;
                    SeriesCollectionOnPage.Clear();
                    LoadMoreSeriesAsync();
                    LoadMoreSeriesCommand = new Command(LoadMoreSeriesAsync);
                    IsInfiniteScrollEnabled = !IsInfiniteScrollEnabled;
                    IsNavigationButtonsVisible = !IsNavigationButtonsVisible;
                });
            }
        }
        private async void LoadMoreSeriesAsync()
        {
            if (IsWorking)
                return;

            IsWorking = true;
            await Task.Delay(1000);

            tmdb.GetSeries(paging_info.CurrentPage, IsInfiniteScrollEnabled);
            paging_info.CurrentPage++;
            IsWorking = false;
        }
    }
}
