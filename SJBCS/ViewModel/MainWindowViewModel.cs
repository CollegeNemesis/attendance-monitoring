using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MaterialDesignThemes.Wpf;
using SJBCS.View;
using SJBCS.Model;

namespace SJBCS.ViewModel
{
    class MainWindowViewModel
    {
        public MenuItem[] MenuItems { get; }

        public MainWindowViewModel()
        {
            MenuItems = new[]
            {
                new MenuItem("Attendance", "/SJBCS;component/Resources/Images/Attendance-icon.png", new AttendanceView{ DataContext = new AttendanceViewModel()}),
                new MenuItem("Student", "/SJBCS;component/Resources/Images/Student-icon.png", new StudentView{ DataContext = new StudentViewModel()}),
                new MenuItem("Section", "/SJBCS;component/Resources/Images/Section-icon.png", new EnrollStudentBiometrics{ DataContext = new EnrollStudentBiometricsViewModel()}),
                new MenuItem("Organization", "/SJBCS;component/Resources/Images/Organization-icon.png", new StudentView()),
                new MenuItem("Reports", "/SJBCS;component/Resources/Images/Report-icon.png", new StudentView()),
                new MenuItem("Settings", "/SJBCS;component/Resources/Images/Settings-icon.png", new StudentView())
            };
        }
    }
}
