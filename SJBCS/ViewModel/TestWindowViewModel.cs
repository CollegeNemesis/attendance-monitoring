using SJBCS.Model;
using SJBCS.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SJBCS.ViewModel
{
    class TestWindowViewModel
    {
        private Object _content;
        private AMSEntities DBContext;

        public TestWindowViewModel()
        {
            DBContext = new AMSEntities();
            //_content = new AddStudentView { DataContext = new AddStudentViewModel(DBContext) };
            //_content = new ManageBiometricsView { DataContext = new ManageBiometricsViewModel("2009100134") };
            _content = new AttendanceMonitoringView { DataContext = new AttendanceMonitoringViewModel() };
        }
        public Object Content => _content;
    }
}
