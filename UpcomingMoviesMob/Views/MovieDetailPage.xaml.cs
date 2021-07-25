using System.ComponentModel;
using UpcomingMoviesMob.Models;
using UpcomingMoviesMob.ViewModels;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace UpcomingMoviesMob.Views
{
    public partial class MovieDetailPage : ContentPage
    {
        public MovieDetailPage()
        {
            InitializeComponent();
            BindingContext = new MovieDetailViewModel();
        }


        private async void OnButtonTapped(object sender, System.EventArgs e)
        {
            try
            {
                StackLayout temp = (StackLayout)sender;
                Video newvideo = (Video)temp.BindingContext;
                await Browser.OpenAsync(newvideo.url, BrowserLaunchMode.SystemPreferred);
            }
            catch
            {
                // An unexpected error occured. No browser may be installed on the device.
            }
        }
    }
}