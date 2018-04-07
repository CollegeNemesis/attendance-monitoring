using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace SJBCS.GUI.Student
{
    /// <summary>
    /// Interaction logic for StudentView.xaml
    /// </summary>
    public partial class StudentView : UserControl
    {
        public StudentView()
        {
            InitializeComponent();
        }

        private void OnFilter(object sender, RoutedEventArgs e)
        {
            var studentViewSource = FindResource("studentViewSource") as CollectionViewSource;

            ICollectionView view = studentViewSource.View as ICollectionView;
            view.Filter = (item) =>
            {
                Data.Student student = item as Data.Student;
                return student.Section.SectionName == "Mahogany" ? true : false;
            };
        }
    }
}
