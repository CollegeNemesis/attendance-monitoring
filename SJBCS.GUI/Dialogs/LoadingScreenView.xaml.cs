using SJBCS.GUI.Utilities;
using Unity;

namespace SJBCS.GUI.Dialogs
{
    /// <summary>
    /// Interaction logic for LoadingScreenView.xaml
    /// </summary>
    public partial class LoadingScreenView
    {
        public LoadingScreenView()
        {
            InitializeComponent();
            DataContext = ContainerHelper.Container.Resolve<LoadingScreenViewModel>();
        }
    }
}
