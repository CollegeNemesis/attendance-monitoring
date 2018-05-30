using System.Windows.Controls;

namespace SJBCS.GUI.Student
{
    /// <summary>
    /// Interaction logic for AddSectionView.xaml
    /// </summary>
    public partial class AddEditSectionView : UserControl
    {
        public AddEditSectionView()
        {
            InitializeComponent();
        }

        private void TimePicker_TextChanged(object sender, TextChangedEventArgs e)
        {
            Schedule.BindingGroup.CommitEdit();
        }
    }
}
