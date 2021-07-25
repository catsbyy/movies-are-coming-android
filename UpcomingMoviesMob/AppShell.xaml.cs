using System;
using System.Collections.Generic;
using UpcomingMoviesMob.ViewModels;
using UpcomingMoviesMob.Views;
using Xamarin.Forms;

namespace UpcomingMoviesMob
{
    public partial class AppShell : Xamarin.Forms.Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(MovieDetailPage), typeof(MovieDetailPage));
            Routing.RegisterRoute(nameof(SeriesDetailPage), typeof(SeriesDetailPage));
            Routing.RegisterRoute(nameof(PersonPage), typeof(PersonPage));
        }

        private async void OnMenuItemClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("//LoginPage");
        }
    }
}
