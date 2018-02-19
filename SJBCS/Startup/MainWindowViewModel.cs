using SJBCS.Data;
using SJBCS.Menu;
using SJBCS.Startup;
using SJBCS.Util;
using Unity;

namespace SJBCS
{
    class MainWindowViewModel : BindableBase
    {
       #region UI Setup
        public LoginViewModel _loginViewModel;
        public UserMenuViewModel _userMenuViewModel;
        public AdminMenuViewModel _adminMenuViewModel;
        #endregion

        public MainWindowViewModel()
        {
            NavigateCommand = new RelayCommand<string>(OnLoginValidation);

            _loginViewModel = ContainerHelper.Container.Resolve<LoginViewModel>();
            _userMenuViewModel = ContainerHelper.Container.Resolve<UserMenuViewModel>();
            _adminMenuViewModel = ContainerHelper.Container.Resolve<AdminMenuViewModel>();

            CurrentViewModel = _loginViewModel;
            _loginViewModel.ValidateLoginRequested += NavToMenu;

        }
        private BindableBase _currentViewModel;
        public BindableBase CurrentViewModel
        {
            get { return _currentViewModel; }
            set { SetProperty(ref _currentViewModel, value); }
        }

        public RelayCommand<string> NavigateCommand { get; set; }

        private void NavToMenu(User user)
        {
            if (user.Type.ToLower().Equals("admin"))
            {
                CurrentViewModel = _adminMenuViewModel;
            }
            else if (user.Type.ToLower().Equals("user"))
            {
                CurrentViewModel = _userMenuViewModel;
            }
            else
            {
                CurrentViewModel = null;
            }

        }

        private void OnLoginValidation(string userType)
        {
            userType = userType.ToLower();
            switch (userType)
            {
                case "admin":
                    CurrentViewModel = _adminMenuViewModel;
                    break;
                case "user":
                    CurrentViewModel = _userMenuViewModel;
                    break;

                default:
                    break;
            }
        }

    }
}
