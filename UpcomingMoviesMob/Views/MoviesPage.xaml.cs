using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UpcomingMoviesMob.Models;
using UpcomingMoviesMob.ViewModels;
using UpcomingMoviesMob.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace UpcomingMoviesMob.Views
{
    public partial class MoviesPage : ContentPage
    {
        DiscoveryViewModel _viewModel;

        public MoviesPage()
        {
            InitializeComponent();

            BindingContext = _viewModel = new DiscoveryViewModel();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            _viewModel.OnAppearing();
        }
    }
}