using MaterialDesignThemes.Wpf;
using System;
using System.Windows.Controls;

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
            try
            {
                ((AddEditStudentViewModel)DataContext).CurrentViewModel.SwitchOn();
                ((AddEditStudentViewModel)DataContext).CurrentViewModel.Biometrics = ((AddEditStudentViewModel)DataContext).Student.Biometrics;
            }
            catch(Exception error)
            {
                return;
            }
        }

        public void DialogClosingEventHandler(object sender, DialogClosingEventArgs eventArgs)
        {
            if (((AddEditStudentViewModel)DataContext).CurrentViewModel.IsDone)
                ((AddEditStudentViewModel)DataContext).OnEnrollBiometric(((AddEditStudentViewModel)DataContext).CurrentViewModel.Biometric);

            ((AddEditStudentViewModel)DataContext).CurrentViewModel.SwitchOff();
        }
    }
}
