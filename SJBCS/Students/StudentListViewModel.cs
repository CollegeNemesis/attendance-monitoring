using SJBCS.Data;
using SJBCS.Services;
using System.Collections.ObjectModel;
using SJBCS.Util;

namespace SJBCS.Students
{
    class StudentListViewModel : BindableBase
    {
        private IStudentsRepository _repo = new StudentsRepository();

        private ObservableCollection<Student> _students;
        public ObservableCollection<Student> Students
        {
            get { return _students; }
            set { SetProperty(ref _students, value); }
        }

        public async void LoadStudents()
        {
            Students = new ObservableCollection<Student>(await _repo.GetStudentsAsync());
        }

    }
}
