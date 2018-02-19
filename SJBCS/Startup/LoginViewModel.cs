using SJBCS.Converter;
using SJBCS.Data;
using SJBCS.Util;
using SJBCS.Services;
using System;
using System.Windows;

namespace SJBCS.Startup
{
    class LoginViewModel : BindableBase
    {
        private IUsersRepository _repo;

        private string _username;

        public string Username
        {
            get { return _username; }
            set { SetProperty(ref _username, value); }
        }


        public LoginViewModel(IUsersRepository repo)
        {
            _repo = repo;
            ValidateLoginCommand = new RelayCommand<IWrappedParameter<string>>(password => { Login(Username, (IWrappedParameter<string>)password); });
        }

        public RelayCommand<IWrappedParameter<string>> ValidateLoginCommand { get; private set; }
        public event Action<User> ValidateLoginRequested = delegate { };


        private async void Login(string username, IWrappedParameter<string> password)
        {
            User user = await _repo.GetUserAsync(_username);
            if (user != null)
            {
                if (user.Password.Equals(password.Value))
                {
                    ValidateLoginRequested(user);
                }
                else
                {
                    if (MessageDialogProperty._isMessageDialogOpen == false)
                    {
                        MessageDialogProperty._isMessageDialogOpen = true;
                        Application.Current.Dispatcher.Invoke((Action)delegate
                        {
                            MessageDialogProperty.OpenDialog(MessageType.Error, "Invalid password.");
                        });
                    }
                }
            }
            else
            {
                if (MessageDialogProperty._isMessageDialogOpen == false)
                {
                    MessageDialogProperty._isMessageDialogOpen = true;
                    Application.Current.Dispatcher.Invoke((Action)delegate
                    {
                        MessageDialogProperty.OpenDialog(MessageType.Error, "User doesn't exist.");
                    });
                }
            }
        }
    }
}
