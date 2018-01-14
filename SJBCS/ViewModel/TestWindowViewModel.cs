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
        private EnrollStudentBiometrics _content;

        public TestWindowViewModel()
        {
            _content = new EnrollStudentBiometrics { DataContext = new EnrollStudentBiometricsViewModel() };
        }
        public Object Content => _content;
    }
}
