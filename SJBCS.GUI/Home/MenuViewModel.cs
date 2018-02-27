using AMS.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SJBCS.GUI.Home
{
    public class MenuViewModel : BindableBase
    {
        public RelayCommand NavToAttendanceCommand { get; private set; }
        public RelayCommand NavToSmsCommand { get; private set; }
        public RelayCommand NavToStudentCommand { get; private set; }
        public RelayCommand NavToSectionCommand { get; private set; }
        public RelayCommand NavToGroupCommand { get; private set; }
        public RelayCommand NavToReportCommand { get; private set; }

        public event Action NavToAttendanceRequested = delegate { };
        public event Action NavToSmsRequested = delegate { };
        public event Action NavToStudentRequested = delegate { };
        public event Action NavToSectionRequested = delegate { };
        public event Action NavToGroupRequested = delegate { };
        public event Action NavToReportRequested = delegate { };

        public MenuViewModel()
        {
            NavToAttendanceCommand = new RelayCommand(OnNavToAttendance);
            NavToSmsCommand = new RelayCommand(OnNavToSms);
            NavToStudentCommand = new RelayCommand(OnNavToStudent);
            NavToSectionCommand = new RelayCommand(OnNavToSection);
            NavToReportCommand = new RelayCommand(OnNavToReport);
            NavToGroupCommand = new RelayCommand(OnNavToGroup);
        }

        private void OnNavToGroup()
        {
            NavToGroupRequested();
        }

        private void OnNavToReport()
        {
            NavToReportRequested();
        }

        private void OnNavToSection()
        {
            NavToSectionRequested();
        }

        private void OnNavToStudent()
        {
            NavToStudentRequested();
        }

        private void OnNavToSms()
        {
            NavToSmsRequested();
        }

        private void OnNavToAttendance()
        {
            NavToAttendanceRequested();
        }
    }
}
