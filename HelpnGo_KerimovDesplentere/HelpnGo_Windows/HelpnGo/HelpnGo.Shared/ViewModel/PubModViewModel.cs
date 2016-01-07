using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Views;
using HelpnGo.DAL;
using HelpnGo.Model;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.UI.Popups;
using Windows.UI.Xaml.Navigation;
using Windows.ApplicationModel.Resources.Core;

namespace HelpnGo.ViewModel
{
    public class PubModViewModel : ViewModelBase
    {
        

        private INavigationService _navigationService;

        private DataAccess dataAccess;


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


        private decimal _pubId;

        private string _title;

        private string _description;

        private string _email;

        private string _phone_Number;

        public decimal PubId
        {
            get
            {
                return _pubId;
            }

            set
            {
                _pubId = value;
                RaisePropertyChanged("PubId");
            }
        }

        public string Title
        {
            get
            {
                return _title;
            }

            set
            {
                _title = value;
                RaisePropertyChanged("Title");
            }
        }


        public string Description
        {
            get
            {
                return _description;
            }

            set
            {
                _description = value;
                RaisePropertyChanged("Description");
            }
        }

        public string Email
        {
            get
            {
                return _email;
            }

            set
            {
                _email = value;
                RaisePropertyChanged("Email");
            }
        }

        public string Phone_Number
        {
            get
            {
                return _phone_Number;
            }

            set
            {
                _phone_Number = value;
                RaisePropertyChanged("Phone_Number");
            }
        }

        


        public PubModViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
            dataAccess = new DataAccess();
            getCatsAndProvs();
        }

        public async void getCatsAndProvs()
        {
            Categories = await dataAccess.getCategoriesAsync();
            Provinces = await dataAccess.getProvincesAsync();
        }

        private ICommand _modifyPublication;

        public ICommand ModifyPublication
        {
            get
            {
                if (this._modifyPublication == null)
                {
                    this._modifyPublication = new RelayCommand(() => updatePublication());
                }
                return this._modifyPublication;
            }
        }

        public async void updatePublication()
        {
            if (fieldsOK())
            {
                Publication newPub = new Publication();
                newPub.Publication_id = PubId;
                newPub.Author_id = Globals.currentUserId;
                newPub.Category_label = SelectedCategory;
                newPub.Description = Description;
                newPub.Email = Email;
                newPub.Is_offer = OffersChecked;
                newPub.Phone_number = Phone_Number;
                newPub.Province_label = SelectedProvince;
                newPub.Title = Title;
                
                if (await dataAccess.updatePublicationAsync(newPub))
                {
                    new MessageDialog(ResourceManager.Current.MainResourceMap.GetValue("Resources/Publication_successfully_modified").ValueAsString).ShowAsync();
                    _navigationService.NavigateTo("AccMan");
                }
                else
                {
                    new MessageDialog(ResourceManager.Current.MainResourceMap.GetValue("Resources/An_error_occurred_during_publication_modification").ValueAsString).ShowAsync();
                }
            }
        }

        private ICommand _deletePublication;

        public ICommand DeletePublication
        {
            get
            {
                if (this._deletePublication == null)
                {
                    this._deletePublication = new RelayCommand(() => deletePub());
                }
                return this._deletePublication;
            }
        }

        public async void deletePub()
        {
            
                if (await dataAccess.deletePublicationAsync(PubId))
                {
                    new MessageDialog(ResourceManager.Current.MainResourceMap.GetValue("Resources/Publication_successfully_deleted").ValueAsString).ShowAsync();
                    _navigationService.NavigateTo("AccMan");
                }
                else
                {
                    new MessageDialog(ResourceManager.Current.MainResourceMap.GetValue("Resources/An_error_occurred_during_publication_removal").ValueAsString).ShowAsync();
                }
            
        }

        private bool fieldsOK()
        {
            if (Title == "" || Description == "" || Email == "" || SelectedCategory == "" || SelectedProvince == "")
            {
                new MessageDialog(ResourceManager.Current.MainResourceMap.GetValue("Resources/Please_fill_all_compulsory_fields").ValueAsString).ShowAsync();
                return false;
            }

            if (Title.Length > 50)
            {
                new MessageDialog(ResourceManager.Current.MainResourceMap.GetValue("Resources/Title_is_too_long").ValueAsString).ShowAsync();
                return false;
            }

            if (Description.Length > 300)
            {
                new MessageDialog(ResourceManager.Current.MainResourceMap.GetValue("Resources/Description_is_too_long").ValueAsString).ShowAsync();
                return false;
            }

            if (Phone_Number != null)
            {
                if (Phone_Number.Length > 20)
                {
                    new MessageDialog(ResourceManager.Current.MainResourceMap.GetValue("Resources/Phone_number_is_too_long").ValueAsString).ShowAsync();
                    return false;
                }
                if (Phone_Number.Length > 0 && Phone_Number.Length < 10)
                {
                    new MessageDialog(ResourceManager.Current.MainResourceMap.GetValue("Resources/Phone_number_is_too_short").ValueAsString).ShowAsync();
                    return false;
                }
            }

            if (Email.Length > 40)
            {
                new MessageDialog(ResourceManager.Current.MainResourceMap.GetValue("Resources/Email_is_too_long").ValueAsString).ShowAsync();
                return false;
            }

            Regex regex = new Regex("[\\w\\.-]*[a-zA-Z0-9_]@[\\w\\.-]*[a-zA-Z0-9]\\.[a-zA-Z][a-zA-Z\\.]*[a-zA-Z]");
            if (!regex.IsMatch(Email))
            {
                new MessageDialog(ResourceManager.Current.MainResourceMap.GetValue("Resources/Invalid_email_address").ValueAsString).ShowAsync();
                return false;
            }

            return true;
        }

        public void OnNavigatedTo(NavigationEventArgs e)
        {
            if (Globals.currentUserId < 0)
            {
                _navigationService.NavigateTo("Iden");
            }
            else {
                if (e != null)
                {
                    Publication pub = (Publication)e.Parameter;
                    PubId = pub.Publication_id;
                    SelectedCategory = pub.Category_label;
                    Description = pub.Description;
                    Email = pub.Email;
                    OffersChecked = pub.Is_offer;
                    DemandsChecked = !pub.Is_offer;
                    Phone_Number = pub.Phone_number;
                    SelectedProvince = pub.Province_label;
                    Title = pub.Title;
                }
            }
        }

    }
}
