using MaterialDesignThemes.Wpf;
using System.Windows.Controls;

namespace SJBCS.GUI.Student
{
    /// <summary>
    /// Interaction logic for SectionView.xaml
    /// </summary>
    public partial class SectionView : UserControl
    {
        public SectionView()
        {
            InitializeComponent();
        }

        public void DialogClosingEventHandler(object sender, DialogClosingEventArgs eventArgs)
        {
            ((SectionViewModel)DataContext).Initialize();
        }
    }
}
