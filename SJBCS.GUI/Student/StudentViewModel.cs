using AMS.Utilities;
using MaterialDesignThemes.Wpf;
using SJBCS.Data;
using SJBCS.GUI.Dialogs;
using SJBCS.Services.Repository;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using Unity;

namespace SJBCS.GUI.Student
{
    public class StudentViewModel : BindableBase
    {
        private IStudentsRepository _studentsRepository;
        private IContactsRepository _contactsRepository;
        private IRelBiometricsRepository _relBiometricsRepository;
        private IBiometricsRepository _biometricsRepository;
        private ObservableCollection<Data.Student> _students;

        private DataGridRowDetailsVisibilityMode _rowDetailsVisible;

        public DataGridRowDetailsVisibilityMode RowDetailsVisible
        {
            get { return _rowDetailsVisible; }
            set { SetProperty(ref _rowDetailsVisible, value); }
        }

        public ObservableCollection<Data.Student> Students
        {
            get { return _students; }
            set { SetProperty(ref _students, value); }
        }

        public StudentViewModel(IStudentsRepository studentsRepository, IContactsRepository contactsRepository, IRelBiometricsRepository relBiometricsRepository, IBiometricsRepository biometricsRepository)
        {
            AddStudentCommand = new RelayCommand(OnAddStudent);
            DeleteStudentCommand = new RelayCommand<Data.Student>(OnDeleteStudent);
            EditStudentCommand = new RelayCommand<Data.Student>(OnEditStudent);
            _studentsRepository = studentsRepository;
            _contactsRepository = contactsRepository;
            _biometricsRepository = biometricsRepository;
            _relBiometricsRepository = relBiometricsRepository;
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



        private async void OnDeleteStudent(Data.Student student)
        {
            try
            {
                if (student.Attendances.Count == 0)
                {
                    _studentsRepository.DeleteStudent(student.StudentGuid);
                    LoadStudents();
                }
                else
                    throw new ArgumentException("Cannot delete an active student");
            }
            catch (Exception error)
            {
                var view = new DialogBoxView
                {
                    DataContext = new DialogBoxViewModel(MessageType.Warning, "You cannot delete an active student.")
                };

                //show the dialog
                var result = await DialogHost.Show(view, "RootDialog", ClosingEventHandler);
            }
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
