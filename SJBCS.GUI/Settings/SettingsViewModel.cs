using AMS.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SJBCS.GUI.Settings
{
    public class SettingsViewModel : BindableBase
    {
        public RelayCommand NavToHomeCommand { get; private set; }
        public RelayCommand NavToConfigCommand { get; private set; }
        public RelayCommand NavToUserCommand { get; private set; }

        public event Action NavToHomeRequested = delegate { };
        public event Action NavToConfigRequested = delegate { };
        public event Action NavToUserRequested = delegate { };

        public SettingsViewModel()
        {
            NavToHomeCommand = new RelayCommand(OnNavToHome);
            NavToConfigCommand = new RelayCommand(OnNavToConfig);
            NavToUserCommand = new RelayCommand(OnNavToUser);
        }

        private void OnNavToHome()
        {
            NavToHomeRequested();
        }

        private void OnNavToConfig()
        {
            NavToConfigRequested();
        }

        private void OnNavToUser()
        {
            NavToUserRequested();
        }
    }
}
