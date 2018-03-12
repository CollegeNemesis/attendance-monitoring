using AMS.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SJBCS.GUI.Student
{
    public class StudentDetailsViewModel : BindableBase
    {
        private Data.Student _selectedStudent;

        public Data.Student SelectedStudent
        {
            get { return _selectedStudent; }
            set { SetProperty(ref _selectedStudent, value); }
        }
    }
}
