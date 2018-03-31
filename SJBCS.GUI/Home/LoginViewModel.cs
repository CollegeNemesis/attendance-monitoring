using AMS.Utilities;
using MaterialDesignThemes.Wpf;
using SJBCS.Data;
using SJBCS.GUI.Converters;
using SJBCS.GUI.Dialogs;
using SJBCS.GUI.Home;
using SJBCS.Services.Repository;
using System;

namespace SJBCS.GUI.Home
{
    public class LoginViewModel : BindableBase
    {
        IUsersRepository _repo;
        private string _username;

        public string Username
        {
            get { return _username; }
            set { SetProperty(ref _username, value); }
        }

        public LoginViewModel(IUsersRepository repo)
        {
            _repo = repo;
            LoginCommand = new RelayCommand<IWrappedParameter<string>>(password => { Login(Username, password); });
        }
        public RelayCommand<IWrappedParameter<string>> LoginCommand { get; private set; }
        public event Action<User> LoginRequested = delegate { };

        private void Login(string username, IWrappedParameter<string> password)
        {
            User user =  _repo.GetUser(_username);
            if (user != null)
            {
                if (user.Password.Equals(password.Value))
                {
                    LoginRequested(user);
                }
                else
                {
                    if (MessageDialog.isDialogOpen == false)
                    {
                        MessageDialog.isDialogOpen = true;
                        MessageDialog.OpenDialog(MessageType.Error, "Invalid password.");
                    }
                }
            }
            else
            {
                if (MessageDialog.isDialogOpen == false)
                {
                    MessageDialog.isDialogOpen = true;
                    MessageDialog.OpenDialog(MessageType.Error, "User doesn't exist.");
                }
            }
        }
    }
}
