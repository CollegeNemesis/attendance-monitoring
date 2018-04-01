using AMS.Utilities;
using Unity;

namespace SJBCS.GUI.Settings
{
    public class StartupConfigWindowViewModel : BindableBase
    {
        private DbManagementViewModel dbManagementViewModel;
        private SmsManagementViewModel smsManagementViewModel;

        private object _dbManagementViewModel;
        public object DbManagementViewModel
        {
            get { return _dbManagementViewModel; }
            set { SetProperty(ref _dbManagementViewModel, value); }
        }

        private object _smsManagementViewModel;
        public object SmsManagementViewModel
        {
            get { return _smsManagementViewModel; }
            set { SetProperty(ref _smsManagementViewModel, value); }
        }

        public StartupConfigWindowViewModel()
        {
            dbManagementViewModel = ContainerHelper.Container.Resolve<DbManagementViewModel>();
            smsManagementViewModel = ContainerHelper.Container.Resolve<SmsManagementViewModel>();
            DbManagementViewModel = dbManagementViewModel;
            SmsManagementViewModel = smsManagementViewModel;
        }
    }
}
