using System;
using System.Collections.Generic;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;
using UpcomingMoviesMob.Core;
using UpcomingMoviesMob.Models;
using System.Collections.ObjectModel;
using UpcomingMoviesMob.Views;

namespace UpcomingMoviesMob.ViewModels
{
    public class AboutViewModel : BaseViewModel
    {
        public string Greeting { get; set; } = "Hello!";
        TmdbAPI tmdb = new TmdbAPI();
        public ObservableCollection<TrendingItem> TrendingTodayCollection { get; set; }
        public ObservableCollection<TrendingItem> TrendingWeekCollection { get; set; }
        public Command<TrendingItem> ItemTapped { get; }
        public AboutViewModel()
        {
            Title = "Home";
            CreateGreeting();
            TrendingTodayCollection = tmdb.GetTrendingToday();
            TrendingWeekCollection = tmdb.GetTrendingWeek();

            ItemTapped = new Command<TrendingItem>(OnItemSelected);
        }

        async void OnItemSelected(TrendingItem trendingItem)
        {
            if (trendingItem == null)
                return;

            // This will push the ItemDetailPage onto the navigation stack
            if (trendingItem.media_type == "movie")
            {
                await Shell.Current.GoToAsync($"{nameof(MovieDetailPage)}?{nameof(MovieDetailViewModel.MovieId)}={trendingItem.id}");
            }
            else if (trendingItem.media_type == "tv")
            {
                await Shell.Current.GoToAsync($"{nameof(SeriesDetailPage)}?{nameof(SeriesDetailViewModel.SeriesId)}={trendingItem.id}");
            }
        }
        private void CreateGreeting()
        {
            if ((DateTime.Now.Hour >= 21) && (DateTime.Now.Hour < 5))
            {
                Greeting = "Good night!";
            }
            if ((DateTime.Now.Hour >= 5) && (DateTime.Now.Hour < 12))
            {
                Greeting = "Good morning!";
            }
            if ((DateTime.Now.Hour >= 12) && (DateTime.Now.Hour < 17))
            {
                Greeting = "Good afternoon!";
            }
            if ((DateTime.Now.Hour >= 17) && (DateTime.Now.Hour < 21))
            {
                Greeting = "Good evening!";
            }
        }
        public ICommand OpenTmdbWebsite
        {
            get
            {
                return new Command(OpenTmdbWebsiteAsync);
            }
        }
        private async void OpenTmdbWebsiteAsync()
        {
            try
            {
                string url = "https://www.themoviedb.org/";
                await Browser.OpenAsync(url, BrowserLaunchMode.SystemPreferred);
            }
            catch
            {
                // An unexpected error occured. No browser may be installed on the device.
            }

        }
    }
}