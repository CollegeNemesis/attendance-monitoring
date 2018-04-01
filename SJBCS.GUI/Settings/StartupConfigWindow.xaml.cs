using AMS.Utilities;
using System.Windows;
using Unity;

namespace SJBCS.GUI.Settings
{
    /// <summary>
    /// Interaction logic for ConfigurationWindow.xaml
    /// </summary>
    public partial class StartupConfigWindow : Window
    {
        public StartupConfigWindow()
        {
            InitializeComponent();
            DataContext = ContainerHelper.Container.Resolve<StartupConfigWindowViewModel>();
        }
    }
}
