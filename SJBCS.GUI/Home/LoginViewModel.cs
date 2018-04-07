using SJBCS.GUI.Utilities;
using MaterialDesignThemes.Wpf;
using SJBCS.Data;
using SJBCS.GUI.Converters;
using SJBCS.GUI.Dialogs;
using SJBCS.GUI.Home;
using SJBCS.Services.Repository;
using System;
using System.Security.Cryptography;
using SJBCS.GUI.Settings;
using System.Text;
using System.IO;

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

        public RelayCommand<IWrappedParameter<string>> LoginCommand { get; private set; }
        public RelayCommand EntryCommand { get; private set; }
        public event Action<Data.User> LoginRequested = delegate { };
        public event Action EntryRequested = delegate { };

        public LoginViewModel(IUsersRepository repo)
        {
            _repo = repo;
            LoginCommand = new RelayCommand<IWrappedParameter<string>>(password => { Login(Username, password); });
            EntryCommand = new RelayCommand(OnEntry);
        }

        private void OnEntry()
        {
            EntryRequested();
        }

        public string DecryptText(string strText)
        {
            return Decrypt(strText, "&%#@?,:*");
        }
        

        private string Decrypt(string strText, string sDecrKey)
        {
            byte[] byKey = { };
            byte[] IV = { 18, 52, 86, 120, 144, 171, 205, 239 };
            byte[] inputByteArray;
            try
            {
                byKey = System.Text.Encoding.UTF8.GetBytes(StringExtensions.Left(sDecrKey, 8));
                DESCryptoServiceProvider des = new DESCryptoServiceProvider();
                inputByteArray = Convert.FromBase64String(strText);
                MemoryStream ms = new MemoryStream();
                CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(byKey, IV), CryptoStreamMode.Write);
                cs.Write(inputByteArray, 0, inputByteArray.Length);
                cs.FlushFinalBlock();
                System.Text.Encoding encoding = System.Text.Encoding.UTF8;
                return encoding.GetString(ms.ToArray());
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }


        private async void Login(string username, IWrappedParameter<string> password)
        {
            Data.User user =  _repo.GetUser(_username);

            if (user != null)
            {
                string _password = DecryptText(user.Password);

                if (_password.Equals(password.Value))
                {
                    LoginRequested(user);
                }
                else
                {
                    var result = await DialogHelper.ShowDialog(DialogType.Error, "Invalid username/password.");
                }
            }
            else
            {
                var result = await DialogHelper.ShowDialog(DialogType.Error, "Invalid username/password.");
            }
        }
    }
}
