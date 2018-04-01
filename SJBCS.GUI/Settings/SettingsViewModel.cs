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
        public RelayCommand NavToUserManagementCommand { get; private set; }
        public RelayCommand NavToDbManagementCommand { get; private set; }
        public RelayCommand NavToSmsManagementCommand { get; private set; }

        public event Action NavToUserManagementRequested = delegate { };
        public event Action NavToDbManagementRequested = delegate { };
        public event Action NavToSmsManagementRequested = delegate { };

        public SettingsViewModel()
        {
            NavToUserManagementCommand = new RelayCommand(OnNavtoUserManagement);
            NavToDbManagementCommand = new RelayCommand(OnNavToDbManagement);
            NavToSmsManagementCommand = new RelayCommand(OnNavToSmsManagement);
        }

        private void OnNavToSmsManagement()
        {
            NavToSmsManagementRequested();
        }

        private void OnNavtoUserManagement()
        {
            NavToUserManagementRequested();
        }

        private void OnNavToDbManagement()
        {
            NavToDbManagementRequested();
        }
    }
}
