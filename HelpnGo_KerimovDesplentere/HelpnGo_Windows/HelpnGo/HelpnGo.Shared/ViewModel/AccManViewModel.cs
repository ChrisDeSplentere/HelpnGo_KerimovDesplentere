using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Views;
using HelpnGo.DAL;
using HelpnGo.Model;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using Windows.UI.Xaml.Navigation;

namespace HelpnGo.ViewModel
{
    public class AccManViewModel : ViewModelBase, INotifyPropertyChanged
    {
       

        private DataAccess dataAccess;

        private INavigationService _navigationService;

        private ObservableCollection<Publication> _publications;

        public ObservableCollection<Publication> Publications
        {
            get { return _publications; }
            set
            {
                _publications = value;
                RaisePropertyChanged("Publications");
            }
        }

        public AccManViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
            dataAccess = new DataAccess();
        }

        public async void getUsersPublications()
        {
            Publications = await dataAccess.getUsersPublicationsAsync(Globals.currentUserId);
        }

        private Publication _selectedPub;

        public Publication SelectedPub
        {
            get { return _selectedPub; }
            set
            {
                _selectedPub = value;
                RaisePropertyChanged("SelectedPub");
            }
        }

        private ICommand _seePubDetails;

        public ICommand SeePubDetails
        {
            get
            {
                if (this._seePubDetails == null)
                {
                    this._seePubDetails = new RelayCommand(() => GoToPubModPage());
                }
                return this._seePubDetails;
            }
        }

        private void GoToPubModPage()
        {
            if (SelectedPub != null)
            {
                _navigationService.NavigateTo("PubMod", SelectedPub);
            }
        }

        private ICommand goBackHome;

        public ICommand GoBackHome
        {
            get
            {
                if (this.goBackHome == null)
                {
                    this.goBackHome = new RelayCommand(() => GoToHomePage());
                }
                return this.goBackHome;
            }
        }

        private void GoToHomePage()
        {

            _navigationService.NavigateTo("MainPage");

        }

        private ICommand goToPubCreation;

        public ICommand GoToPubCreation
        {
            get
            {
                if (this.goToPubCreation == null)
                {
                    this.goToPubCreation = new RelayCommand(() => toPubCreation());
                }
                return this.goToPubCreation;
            }
        }

        private void toPubCreation()
        {

            _navigationService.NavigateTo("PubCrea");

        }

        private ICommand logOut;

        public ICommand LogOut
        {
            get
            {
                if (this.logOut == null)
                {
                    this.logOut = new RelayCommand(() => logOutAndGoToIden());
                }
                return this.logOut;
            }
        }

        private void logOutAndGoToIden()
        {

            Globals.currentUserId = -1;
            _navigationService.NavigateTo("Iden");

        }

        public void OnNavigatedTo(NavigationEventArgs e)
        {
            if (Globals.currentUserId < 0)
            {
                _navigationService.NavigateTo("Iden");
            }
            else
            {
                getUsersPublications();
            }
        }

    }
}
