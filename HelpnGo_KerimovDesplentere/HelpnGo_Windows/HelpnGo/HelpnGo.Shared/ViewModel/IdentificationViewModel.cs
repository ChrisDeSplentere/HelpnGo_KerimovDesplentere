using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Views;
using HelpnGo.DAL;
using HelpnGo.Model;
using System.Windows.Input;
using Windows.UI.Popups;
using Windows.ApplicationModel.Resources.Core;
using Windows.Storage.Streams;
using Windows.Security.Cryptography;
using Windows.Security.Cryptography.Core;

namespace HelpnGo.ViewModel
{
    public class IdentificationViewModel : ViewModelBase
    {
       

        private DataAccess dataAccess;

        private INavigationService _navigationService;

        private string _enteredEmail;

        public string EnteredEmail
        {
            get { return _enteredEmail; }
            set
            {
                _enteredEmail = value;
                RaisePropertyChanged("EnteredEmail");
            }
        }

        private string _enteredPassword;

        public string EnteredPassword
        {
            get { return _enteredPassword; }
            set
            {
                _enteredPassword = value;
                RaisePropertyChanged("EnteredPassword");
            }
        }

        private ICommand _logIn;

        public ICommand LogIn
        {
            get
            {
                if (this._logIn == null)
                {
                    this._logIn = new RelayCommand(() => CheckAndLog());
                }
                return this._logIn;
            }
        }

        private async void CheckAndLog()
        {
            if(EnteredEmail == null || EnteredEmail == "")
            {
                new MessageDialog(ResourceManager.Current.MainResourceMap.GetValue("Resources/Wrong_email_address").ValueAsString).ShowAsync();
            }
            else
            {
                User user = await dataAccess.getUserByEmail(EnteredEmail);
                if (user == null)
                {
                    new MessageDialog(ResourceManager.Current.MainResourceMap.GetValue("Resources/Wrong_email_address").ValueAsString).ShowAsync();
                }
                else
                {
                    string codedPassword = user.password;
                    if (EnteredPassword != null && EnteredPassword != "" && encrypt(EnteredPassword) == codedPassword)
                    {
                        Globals.currentUserId = user.User_id;
                        _navigationService.NavigateTo("AccMan");
                    }
                    else
                    {
                        new MessageDialog(ResourceManager.Current.MainResourceMap.GetValue("Resources/Wrong_password").ValueAsString).ShowAsync();
                    }
                }
            }
        }

        public IdentificationViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
            dataAccess = new DataAccess();
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

        private ICommand createAccount;

        public ICommand CreateAccount
        {
            get
            {
                if (this.createAccount == null)
                {
                    this.createAccount = new RelayCommand(() => GoToAccCreaPage());
                }
                return this.createAccount;
            }
        }

        private void GoToAccCreaPage()
        {

            _navigationService.NavigateTo("AccCrea");

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
