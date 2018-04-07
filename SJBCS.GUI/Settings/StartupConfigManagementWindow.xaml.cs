using SJBCS.GUI.Utilities;
using Unity;

namespace SJBCS.GUI.Settings
{
    /// <summary>
    /// Interaction logic for ConfigurationWindow.xaml
    /// </summary>
    public partial class StartupConfigManagementWindow
    {
        public StartupConfigManagementWindow()
        {
            InitializeComponent();
            DataContext = ContainerHelper.Container.Resolve<StartupConfigManagementWindowViewModel>();
        }
    }
}
