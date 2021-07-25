using System;
using System.Diagnostics;
using System.Threading.Tasks;
using UpcomingMoviesMob.Models;
using UpcomingMoviesMob.Core;
using Xamarin.Forms;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using UpcomingMoviesMob.Resources;
using UpcomingMoviesMob.Views;

namespace UpcomingMoviesMob.ViewModels
{
    [QueryProperty(nameof(PersonId), nameof(PersonId))]
    public class PersonViewModel : INotifyPropertyChanged
    {
        TmdbAPI tmdb = new TmdbAPI();
        private PersonInfo person { get; set; }

        private int personId;
        public string Title { get; set; } = "Person's Biography";
        private bool isDead { get; set; }
        private bool isLooped { get; set; } = true;
        private ObservableCollection<PersonImages> personImagesCollection { get; set; }
        public ObservableCollection<PersonImages> PersonImagesCollection
        {
            get { return personImagesCollection; }
            set
            {
                if (personImagesCollection != value)
                {
                    personImagesCollection = value;
                    OnPropertyChanged("PersonImagesCollection");
                }
            }
        }
        public bool IsDead 
        {
            get { return isDead; }
            set
            {
                if (isDead != value)
                {
                    isDead = value;
                    OnPropertyChanged("IsDead");
                }
            }
        }
        public bool IsLooped
        {
            get { return isLooped; }
            set
            {
                if (isLooped != value)
                {
                    isLooped = value;
                    OnPropertyChanged("IsLooped");
                }
            }
        }
        public PersonInfo Person
        {
            get { return person; }
            set
            {
                if (person != value)
                {
                    person = value;
                    OnPropertyChanged("Person");
                }
            }
        }
        
        public int PersonId
        {
            get
            {
                return personId;
            }
            set
            {
                personId = value;
                person = tmdb.GetPersonBio(personId);
                IsDead = tmdb.IsDead(person);
                personImagesCollection = tmdb.GetPersonImages(personId);
                IsLooped = tmdb.IsLooped(personImagesCollection);

                OnPropertyChanged("Person");
                OnPropertyChanged("IsDead");
                OnPropertyChanged("PersonImagesCollection");
                OnPropertyChanged("IsLooped");
                
                OnPropertyChanged("PersonId");
                

            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }

    }
}
