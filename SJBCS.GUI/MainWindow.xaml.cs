using AMS.Utilities;
using System.Windows;
using Unity;

namespace SJBCS.GUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            InitializeComponent();
            DataContext = ContainerHelper.Container.Resolve<MainWindowViewModel>();
        }
    }
}
