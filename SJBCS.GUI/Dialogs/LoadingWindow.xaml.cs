using SJBCS.GUI.Utilities;
using System.Windows;
using Unity;

namespace SJBCS.GUI.Dialogs
{
    /// <summary>
    /// Interaction logic for LoadingWindow.xaml
    /// </summary>
    public partial class LoadingWindow : Window
    {
        public LoadingWindow()
        {
            InitializeComponent();
            DataContext = ContainerHelper.Container.Resolve<LoadingWindowViewModel>();
        }

        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            ((LoadingWindowViewModel)DataContext).CloseTrigger = true;
        }
    }
}
