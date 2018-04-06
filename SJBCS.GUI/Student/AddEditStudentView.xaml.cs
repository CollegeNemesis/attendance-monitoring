using SJBCS.GUI.Utilities;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
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
using Unity;

namespace SJBCS.GUI.Student
{
    /// <summary>
    /// Interaction logic for AddEditStudentView.xaml
    /// </summary>
    public partial class AddEditStudentView : UserControl
    {
        public AddEditStudentView()
        {
            InitializeComponent();
        }

        public void DialogOpeningEventHandler(object sender, DialogOpenedEventArgs eventargs)
        {
            ((AddEditStudentViewModel)DataContext).CurrentViewModel.SwitchOn();
        }

        public void DialogClosingEventHandler(object sender, DialogClosingEventArgs eventArgs)
        {
            if (((AddEditStudentViewModel)DataContext).CurrentViewModel.IsDone)
                ((AddEditStudentViewModel)DataContext).OnEnrollBiometric(((AddEditStudentViewModel)DataContext).CurrentViewModel.Biometric);

            ((AddEditStudentViewModel)DataContext).CurrentViewModel.SwitchOff();
            Console.WriteLine("Closed.");
        }
    }
}
