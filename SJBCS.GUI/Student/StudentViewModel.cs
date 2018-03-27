using AMS.Utilities;
using SJBCS.Services.Repository;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity;

namespace SJBCS.GUI.Student
{
    public class StudentViewModel : BindableBase
    {
        private IStudentsRepository _studentsRepository;
        private ObservableCollection<Data.Student> _students;

        public ObservableCollection<Data.Student> Students
        {
            get { return _students; }
            set { SetProperty(ref _students, value); }
        }

        public StudentViewModel(IStudentsRepository studentsRepository)
        {
            AddStudentCommand = new RelayCommand(OnAddStudent);
            DeleteStudentCommand = new RelayCommand<Data.Student> (OnDeleteStudent);
            EditStudentCommand = new RelayCommand<Data.Student>(OnEditStudent);
            _studentsRepository = studentsRepository;
        }

        public void LoadStudents()
        {
            Students = null;
            Students = new ObservableCollection<Data.Student>(_studentsRepository.GetStudents());
        }

        public RelayCommand AddStudentCommand { get; private set; }
        public RelayCommand<Data.Student> DeleteStudentCommand { get; private set; }
        public RelayCommand<Data.Student> EditStudentCommand { get; private set; }

        public event Action<Data.Student> AddStudentRequested = delegate { };
        public event Action<Data.Student> EditStudentRequested = delegate { };



        private void OnDeleteStudent(Data.Student student)
        {
            //_studentsRepository.DeleteStudent(student.StudentGuid);
        }

        public void OnAddStudent()
        {
            AddStudentRequested(new Data.Student { StudentGuid = Guid.NewGuid() });
        }

        public void OnEditStudent(Data.Student selectedStudent)
        {
            EditStudentRequested(selectedStudent);
        }
    }
}
