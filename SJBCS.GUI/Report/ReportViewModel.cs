using SJBCS.Data;
using SJBCS.GUI.Utilities;

namespace SJBCS.GUI.Report
{
    public class ReportViewModel : BindableBase
    {
        private bool _isLoading;

        public bool IsLoading
        {
            get { return _isLoading; }
            set { SetProperty(ref _isLoading, value); }
        }

        private User _activeUser;

        public User ActiveUser
        {
            get { return _activeUser; }
            set { SetProperty(ref _activeUser, value); }
        }
    }
}
