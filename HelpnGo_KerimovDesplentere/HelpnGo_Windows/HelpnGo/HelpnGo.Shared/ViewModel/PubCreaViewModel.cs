using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Views;
using HelpnGo.DAL;
using HelpnGo.Model;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text.RegularExpressions;
using System.Windows.Input;
using Windows.UI.Popups;
using Windows.UI.Xaml.Navigation;
using Windows.ApplicationModel.Resources.Core;

namespace HelpnGo.ViewModel
{
    
        public class PubCreaViewModel : ViewModelBase, INotifyPropertyChanged
        {
            

            private DataAccess dataAccess;

            private INavigationService _navigationService;

            public PubCreaViewModel(INavigationService navigationService)
            {
                _navigationService = navigationService;
                dataAccess = new DataAccess();
                getCatsAndProvs();
                OffersChecked = true;
            }


            private ICommand _createPublication;

            public ICommand CreatePublication
            {
                get
                {
                    if (this._createPublication == null)
                    {
                        this._createPublication = new RelayCommand(() => setPublication());
                    }
                    return this._createPublication;
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

            public async void getCatsAndProvs()
            {
                Categories = await dataAccess.getCategoriesAsync();
                Provinces = await dataAccess.getProvincesAsync();
            }


            public async void setPublication()
            {
              if (fieldsOK())
              {
                Publication newPub = new Publication();
                newPub.Author_id = Globals.currentUserId;
                newPub.Category_label = SelectedCategory;
                newPub.Description = Description;
                newPub.Email = Email;
                newPub.Is_offer = OffersChecked;
                newPub.Phone_number = Phone_Number;
                newPub.Province_label = SelectedProvince;
                newPub.Publication_id = await dataAccess.getNewPublicationID();
                newPub.Title = Title;

                

                if (await dataAccess.setPublicationAsync(newPub))
                {
                    new MessageDialog(ResourceManager.Current.MainResourceMap.GetValue("Resources/Publication_successfully_created").ValueAsString).ShowAsync();
                    _navigationService.NavigateTo("AccMan");
                }
                else
                {
                    new MessageDialog(ResourceManager.Current.MainResourceMap.GetValue("Resources/An_error_occurred_during_publication_creation").ValueAsString).ShowAsync();
                }

              }  
            }

            private bool fieldsOK()
            {
                if (Title == null || Description == null || Email == null || SelectedCategory == null || SelectedProvince == null)
                {
                    new MessageDialog(ResourceManager.Current.MainResourceMap.GetValue("Resources/Please_fill_all_compulsory_fields").ValueAsString).ShowAsync();
                    return false;
                }

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
                    new MessageDialog(ResourceManager.Current.MainResourceMap.GetValue("Resources/Email_address_is_too_long").ValueAsString).ShowAsync();
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

            private string _title;

            private string _description;

            private string _email;

            private string _phone_Number;



        public void OnNavigatedTo(NavigationEventArgs e)
        {
            if (Globals.currentUserId < 0)
            {
                _navigationService.NavigateTo("Iden");
            }
        }

    }
    }

