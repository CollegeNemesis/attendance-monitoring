using SJBCS.GUI.Utilities;

namespace SJBCS.GUI.Settings
{
    public class UserManagementViewModel : BindableBase
    {
        private Data.User _activeUser;

        public Data.User ActiveUser
        {
            get { return _activeUser; }
            set { SetProperty(ref _activeUser, value); }
        }
    }
}
