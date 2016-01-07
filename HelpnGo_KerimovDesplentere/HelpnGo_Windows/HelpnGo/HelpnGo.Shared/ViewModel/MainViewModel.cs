
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Views;
using HelpnGo.DAL;
using HelpnGo.Model;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Input;

namespace HelpnGo.ViewModel
{
    /// <summary>
    /// This class contains properties that the main View can data bind to.
    /// <para>
    /// Use the <strong>mvvminpc</strong> snippet to add bindable properties to this ViewModel.
    /// </para>
    /// <para>
    /// You can also use Blend to data bind with the tool's support.
    /// </para>
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class MainViewModel : ViewModelBase, INotifyPropertyChanged
    {

       

        private DataAccess dataAccess;

        private INavigationService _navigationService;

        private bool _offersChecked;
        
        public bool OffersChecked
        {
            get { return _offersChecked; }
            set
            {
                _offersChecked = value;
                RaisePropertyChanged("OffersChecked");
            }
        }

        private bool _demandsChecked;

        public bool DemandsChecked
        {
            get { return _demandsChecked; }
            set
            {
                _demandsChecked = value;
                RaisePropertyChanged("DemandsChecked");
            }
        }

        private string _selectedCategory;

        public string SelectedCategory
        {
            get { return _selectedCategory; }
            set
            {
                _selectedCategory = value;
                RaisePropertyChanged("SelectedCategory");
            }
        }

        private string _selectedProvince;

        public string SelectedProvince
        {
            get { return _selectedProvince; }
            set
            {
                _selectedProvince = value;
                RaisePropertyChanged("SelectedProvince");
            }
        }

        private ObservableCollection<Publication> _publications;

        public ObservableCollection<Publication> Publications
        {
            get { return _publications; }
            set {
                _publications = value;
                RaisePropertyChanged("Publications");
            }
        }

        private ObservableCollection<string> _categories;

        public ObservableCollection<string> Categories
        {
            get { return _categories; }
            set
            {
                _categories = value;
                RaisePropertyChanged("Categories");
            }
        }

        private ObservableCollection<string> _provinces;

        public ObservableCollection<string> Provinces
        {
            get { return _provinces; }
            set
            {
                _provinces = value;
                RaisePropertyChanged("Provinces");
            }
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

        private ICommand _searchPubs;

        public ICommand SearchPubs
        {
            get
            {
                if (this._searchPubs == null)
                {
                    this._searchPubs = new RelayCommand(() => searchPublications());
                }
                return this._searchPubs;
            }
        }

        private ICommand _seePubDetails;

        public ICommand SeePubDetails
        {
            get
            {
                if (this._seePubDetails == null)
                {
                    this._seePubDetails = new RelayCommand(() => GoToPubDetailsPage());
                }
                return this._seePubDetails;
            }
        }

        private void GoToPubDetailsPage()
        {
            if (SelectedPub != null)
            {
                _navigationService.NavigateTo("PubDetails", SelectedPub);
            }
        }

        private ICommand _toAccountManagement;

        public ICommand ToAccountManagement
        {
            get
            {
                if (this._toAccountManagement == null)
                {
                    this._toAccountManagement = new RelayCommand(() => GoToAccountManagement());
                }
                return this._toAccountManagement;
            }
        }

        private void GoToAccountManagement()
        {
            if(Globals.currentUserId < 0)
            {
                _navigationService.NavigateTo("Iden");
            }
            else
            {
                _navigationService.NavigateTo("AccMan");
            }
            
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

        public MainViewModel(INavigationService navigationService)
        {
            Globals.currentUserId = -1;
            _navigationService = navigationService;
            dataAccess = new DataAccess();
            getCatsAndProvs();
            OffersChecked = true;
            DemandsChecked = true;
            SelectedCategory = "(All)";
            SelectedProvince = "(All)";
        }

        public async void getCatsAndProvs()
        {
            Categories = await dataAccess.getCategoriesAsync();
            Categories.Add("(All)");
            Provinces = await dataAccess.getProvincesAsync();
            Provinces.Add("(All)");
        }

        public async void searchPublications()
        {
            Publications = await dataAccess.getPublicationsAsync(OffersChecked,DemandsChecked,SelectedCategory,SelectedProvince);
        }
        
    }
}