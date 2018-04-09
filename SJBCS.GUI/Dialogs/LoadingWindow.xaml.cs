using SJBCS.GUI.Utilities;
using System.Windows;
using System.Windows.Controls;
using Unity;

namespace SJBCS.GUI.Dialogs
{
    /// <summary>
    /// Interaction logic for LoadingWindow.xaml
    /// </summary>
    public partial class LoadingWindow : UserControl
    {
        public LoadingWindow()
        {
            InitializeComponent();
            DataContext = ContainerHelper.Container.Resolve<LoadingWindowViewModel>();
        }
    }
}
