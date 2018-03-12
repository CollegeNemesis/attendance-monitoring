using AMS.Utilities;
using SJBCS.Services.Repository;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SJBCS.GUI.Student
{
    public class StudentListViewModel : BindableBase
    {
        private IStudentsRepository _studentsRepository;
        private ObservableCollection<Data.Student> _students;

        public ObservableCollection<Data.Student> Students
        {
            get { return _students; }
            set { SetProperty(ref _students, value); }
        }

        private Data.Student _selectedStudent;

        public Data.Student SelectedStudent
        {
            get { return _selectedStudent; }
            set { SetProperty(ref _selectedStudent, value); }
        }

        public StudentListViewModel(IStudentsRepository studentsRepository)
        {
            _studentsRepository = studentsRepository;
            ViewStudentCommand = new RelayCommand<Data.Student>(OnViewStudent);
        }
        public void LoadStudents()
        {
            Students = new ObservableCollection<Data.Student>(_studentsRepository.GetStudents());
        }

        public RelayCommand<Data.Student> ViewStudentCommand { get; private set; }

        public event Action<Data.Student> ViewStudentRequested = delegate { };

        public void OnViewStudent(Data.Student selectedStudent)
        {
            ViewStudentRequested(selectedStudent);
        }

        
    }
}
