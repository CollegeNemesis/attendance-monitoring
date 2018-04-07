using SJBCS.Services.Repository;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

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
