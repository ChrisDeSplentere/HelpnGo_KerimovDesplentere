using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Views;
using HelpnGo.Model;
using Windows.UI.Xaml.Navigation;

namespace HelpnGo.ViewModel
{
    public class PubDetViewModel : ViewModelBase
    {
        

        private INavigationService _navigationService;

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

        public PubDetViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
        }

        public void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e != null)
            {
                SelectedPub = (Publication)e.Parameter;
            }
        }

    }
}
