using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UpcomingMoviesMob.Models;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace UpcomingMoviesMob.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SeriesDetailPage : ContentPage
    {
        public SeriesDetailPage()
        {
            InitializeComponent();
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