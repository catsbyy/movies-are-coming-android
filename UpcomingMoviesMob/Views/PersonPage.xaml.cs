using System.ComponentModel;
using UpcomingMoviesMob.Models;
using UpcomingMoviesMob.ViewModels;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace UpcomingMoviesMob.Views
{
    public partial class PersonPage : ContentPage
    {
        public PersonPage()
        {
            InitializeComponent();
            BindingContext = new PersonViewModel();
        }

    }
}