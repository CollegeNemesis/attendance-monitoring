using SJBCS.Model;
using SJBCS.View;
using SJBCS.Wrapper;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SJBCS.ViewModel
{
    public class StudentViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private static AMSEntities DBContext = new AMSEntities();
        private Student _student;

        private ObservableCollection<Object> _levelList;
        private ObservableCollection<Object> _sectionList;
        private ObservableCollection<Object> _studentList;
        private StudentWrapper _studentWrapper;
        



        public StudentViewModel()
        {
            _student = new Student();
            _studentWrapper = new StudentWrapper();
            _studentList = _studentWrapper.RetrieveAll(DBContext, _student);
        }

        public ObservableCollection<Object> StudentList
        {
            get
            {
                return _studentList;
            }
            set
            {
                _studentList = value;
            }
        }
        public ObservableCollection<Object> SectionList
        {
            get
            {
                return _sectionList;
            }
            set
            {
                _sectionList = value;
            }
        }
        public ObservableCollection<Object> LevelList
        {
            get
            {
                return _levelList;
            }
            set
            {
                _levelList = value;
            }
        }

        private void RaisePropertyChanged(string v)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(v));
            }
        }

    }
}
