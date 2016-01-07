using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Views;
using HelpnGo.DAL;
using HelpnGo.Model;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.UI.Popups;
using Windows.ApplicationModel.Resources.Core;
using Windows.Storage.Streams;
using Windows.Security.Cryptography;
using Windows.Security.Cryptography.Core;

namespace HelpnGo.ViewModel
{
    public class AccCreaViewModel : ViewModelBase, INotifyPropertyChanged
    {
       

        private DataAccess dataAccess;

        private INavigationService _navigationService;

        private string lastname;
        private string firstname;
        private string phone_number;
        private string diplomas;
        private string jobs;
        private string email;
        private string password;
        private string confirmPassword;
        private string street_name;
        private string street_number;

        public string Lastname
        {
            get
            {
                return lastname;
            }

            set
            {
                lastname = value;
                RaisePropertyChanged("Lastname");
            }
        }


        public string Firstname
        {
            get
            {
                return firstname;
            }

            set
            {
                firstname = value;
                RaisePropertyChanged("Firstname");
            }
        }

        public string Phone_Number
        {
            get
            {
                return phone_number;
            }

            set
            {
                phone_number = value;
                RaisePropertyChanged("Phone_Number");
            }
        }

        public string Diplomas
        {
            get
            {
                return diplomas;
            }

            set
            {
                diplomas = value;
                RaisePropertyChanged("Diplomas");
            }
        }

        public string Jobs
        {
            get
            {
                return jobs;
            }

            set
            {
                jobs = value;
                RaisePropertyChanged("Jobs");
            }
        }

        public string Email
        {
            get
            {
                return email;
            }

            set
            {
                email = value;
                RaisePropertyChanged("Email");
            }
        }

        public string Password
        {
            get
            {
                return password;
            }

            set
            {
                password = value;
                RaisePropertyChanged("Password");
            }
        }

        public string ConfirmPassword
        {
            get
            {
                return confirmPassword;
            }

            set
            {
                confirmPassword = value;
                RaisePropertyChanged("ConfirmPassword");
            }
        }

        public string Street_name
        {
            get
            {
                return street_name;
            }

            set
            {
                street_name = value;
                RaisePropertyChanged("Street_name");
            }
        }

        public string Street_number
        {
            get
            {
                return street_number;
            }

            set
            {
                street_number = value;
                RaisePropertyChanged("Street_number");
            }
        }


        private ICommand createUs;

        public ICommand CreateUs
        {
            get
            {
                if (this.createUs == null)
                {
                    this.createUs = new RelayCommand(() => createUser());
                }
                return this.createUs;
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


        private ObservableCollection<City> listVilles;

        public ObservableCollection<City> ListVilles
        {
            get { return listVilles; }
            set
            {
                listVilles = value;
                RaisePropertyChanged("ListVilles");
            }
        }

        private City selectedVille;

        public City SelectedVille
        {
            get { return selectedVille; }
            set
            {
                selectedVille = value;
                RaisePropertyChanged("SelectedVille");
            }
        }


        public AccCreaViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
            dataAccess = new DataAccess();
            initVilles();

        }

        private async void initVilles()
        {
            ListVilles = await dataAccess.getCitiesAsync();
            foreach(City ville in ListVilles)
            {
                ville.Postal_code_to_display = (int) ville.Postal_code;
            }
        }
        

        private async void createUser()
        {
            if (await fieldsOK())
            {
                User user = new User();
                
                user.Date_of_birth = null;
                user.Diplomas = Diplomas;
                user.Email = Email;
                user.Firstname = Firstname;
                user.Jobs = Jobs;
                user.Lastname = Lastname;
                user.Living_city_id = SelectedVille.City_id;

                user.password = encrypt(Password);

                user.Phone_number = Phone_Number;
                user.Street_name = Street_name;
                user.Street_number = Street_number;
                user.User_id = await dataAccess.getNewUserId();

                if(await dataAccess.setNewUserAsync(user))
                {
                    new MessageDialog(ResourceManager.Current.MainResourceMap.GetValue("Resources/Account_successfully_created").ValueAsString).ShowAsync();
                    _navigationService.NavigateTo("Iden");
                }
                else
                {
                    new MessageDialog(ResourceManager.Current.MainResourceMap.GetValue("Resources/An_error_occurred_during_account_creation").ValueAsString).ShowAsync();
                }
            }
        }

        private async Task<bool> fieldsOK()
        {

            if (Firstname == null || Lastname == null || Street_number == null || Street_name == null || Email == null || Password == null || SelectedVille == null)
            {
                new MessageDialog(ResourceManager.Current.MainResourceMap.GetValue("Resources/Please_fill_all_compulsory_fields").ValueAsString).ShowAsync();
                return false;
            }

            if (Firstname == "" || Lastname == "" || Street_number == "" || Street_name == "" || Email == "" || Password == "")
            {
                new MessageDialog(ResourceManager.Current.MainResourceMap.GetValue("Resources/Please_fill_all_compulsory_fields").ValueAsString).ShowAsync();
                return false;
            }

            if (Password != ConfirmPassword)
            {
                new MessageDialog(ResourceManager.Current.MainResourceMap.GetValue("Resources/Wrong_password_confirmation").ValueAsString).ShowAsync();
                return false;
            }

            if(Lastname.Length > 30)
            {
                new MessageDialog(ResourceManager.Current.MainResourceMap.GetValue("Resources/Lastname_is_too_long").ValueAsString).ShowAsync();
                return false;
            }

            if (Firstname.Length > 30)
            {
                new MessageDialog(ResourceManager.Current.MainResourceMap.GetValue("Resources/Firstname_is_too_long").ValueAsString).ShowAsync();
                return false;
            }

            if (Street_number.Length > 10)
            {
                new MessageDialog(ResourceManager.Current.MainResourceMap.GetValue("Resources/Street_number_is_too_long").ValueAsString).ShowAsync();
                return false;
            }

            if (Street_name.Length > 40)
            {
                new MessageDialog(ResourceManager.Current.MainResourceMap.GetValue("Resources/Street_name_is_too_long").ValueAsString).ShowAsync();
                return false;
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

            if (await dataAccess.getUserByEmail(Email) != null)
            {
                new MessageDialog(ResourceManager.Current.MainResourceMap.GetValue("Resources/Email_already_used").ValueAsString).ShowAsync();
                return false;
            }

            if (Password.Length > 30)
            {
                new MessageDialog(ResourceManager.Current.MainResourceMap.GetValue("Resources/Password_is_too_long").ValueAsString).ShowAsync();
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

            if (Jobs != null)
            {
                if (Jobs.Length > 100)
                {
                    new MessageDialog(ResourceManager.Current.MainResourceMap.GetValue("Resources/Jobs_description_is_too_long").ValueAsString).ShowAsync();
                    return false;
                }
            }

            if (Diplomas != null)
            {
                if (Diplomas.Length > 100)
                {
                    new MessageDialog(ResourceManager.Current.MainResourceMap.GetValue("Resources/Diplomas_description_is_too_long").ValueAsString).ShowAsync();
                    return false;
                }
            }

            return true;

        }


        private string encrypt(string pswd)
        {
            string base64Encoded = string.Empty;
            //convertit le string en binary (IBuffer = lecture et écriture de flux d'octets)
            IBuffer buffer = CryptographicBuffer.ConvertStringToBinary(pswd, BinaryStringEncoding.Utf8);
            //crée le HASH
            HashAlgorithmProvider provider = HashAlgorithmProvider.OpenAlgorithm(HashAlgorithmNames.Sha512);
            IBuffer hash = provider.HashData(buffer);
            //crée un string en base64
            base64Encoded = CryptographicBuffer.EncodeToBase64String(hash);
            return base64Encoded;
        }

    }
}
