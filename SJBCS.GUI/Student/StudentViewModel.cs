using AMS.Utilities;
using MaterialDesignThemes.Wpf;
using SJBCS.Data;
using SJBCS.GUI.Dialogs;
using SJBCS.Services.Repository;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;
using Unity;

namespace SJBCS.GUI.Student
{
    public class StudentViewModel : BindableBase
    {
        private IStudentsRepository _studentsRepository;
        private IContactsRepository _contactsRepository;
        private IRelBiometricsRepository _relBiometricsRepository;
        private IBiometricsRepository _biometricsRepository;
        private ILevelsRepository _levelsRepository;
        private ISectionsRepository _sectionsRepository;

        private ObservableCollection<Level> _levels;
        public ObservableCollection<Level> Levels
        {
            get { return _levels; }
            set { SetProperty(ref _levels, value); }
        }

        private ObservableCollection<Section> _sections;
        public ObservableCollection<Section> Sections
        {
            get { return _sections; }
            set { SetProperty(ref _sections, value); }
        }

        private Guid _selectedLevelId;
        public Guid SelectedLevelId
        {
            get { return _selectedLevelId; }
            set
            {
                if (value != null)
                {
                    Sections = new ObservableCollection<Section>(_sectionsRepository.GetSections(value));
                    if (Sections.Any())
                        SelectedSectionId = Sections[0].SectionID;
                    else
                        SelectedSection = new Section();
                }
                SetProperty(ref _selectedLevelId, value);
                EditCommand.RaiseCanExecuteChanged();
                DeleteCommand.RaiseCanExecuteChanged();
            }
        }

        private Guid _selectedSectionId;
        public Guid SelectedSectionId
        {
            get { return _selectedSectionId; }
            set
            {
                if (value != null)
                    SelectedSection = _sectionsRepository.GetSection(value);
                SetProperty(ref _selectedSectionId, value);
            }
        }

        private Section _selectedSection;
        public Section SelectedSection
        {
            get
            {
                return _selectedSection;
            }

            set
            {
                SetProperty(ref _selectedSection, value);
            }
        }

        private DataGridRowDetailsVisibilityMode _rowDetailsVisible;
        public DataGridRowDetailsVisibilityMode RowDetailsVisible
        {
            get { return _rowDetailsVisible; }
            set { SetProperty(ref _rowDetailsVisible, value); }
        }

        public RelayCommand AddCommand { get; private set; }
        public RelayCommand<Data.Student> DeleteCommand { get; private set; }
        public RelayCommand<Data.Student> EditCommand { get; private set; }

        public event Action<Data.Student> AddRequested = delegate { };
        public event Action<Data.Student> EditRequested = delegate { };

        private ObservableCollection<Data.Student> _students;
        public ObservableCollection<Data.Student> Students
        {
            get { return _students; }
            set
            {
                SetProperty(ref _students, value);
            }
        }

        public StudentViewModel(IStudentsRepository studentsRepository, ILevelsRepository levelsRepository, ISectionsRepository sectionsRepository, IContactsRepository contactsRepository, IRelBiometricsRepository relBiometricsRepository, IBiometricsRepository biometricsRepository)
        {
            AddCommand = new RelayCommand(OnAdd);
            DeleteCommand = new RelayCommand<Data.Student>(OnDelete);
            EditCommand = new RelayCommand<Data.Student>(OnEdit);

            _studentsRepository = studentsRepository;
            _contactsRepository = contactsRepository;
            _biometricsRepository = biometricsRepository;
            _relBiometricsRepository = relBiometricsRepository;
            _levelsRepository = levelsRepository;
            _sectionsRepository = sectionsRepository;
        }

        public void Initialize()
        {
            LoadStudents();
            LoadComboBox();
        }

        public void LoadComboBox()
        {
            Levels = new ObservableCollection<Level>(_levelsRepository.GetLevels());
            if (Levels.Any())
            {
                if (SelectedLevelId == new Guid())
                    SelectedLevelId = Levels[0].LevelID;
                Sections = new ObservableCollection<Section>(_sectionsRepository.GetSections(SelectedLevelId));
                if (Sections.Any())
                    SelectedSectionId = Sections[0].SectionID;
            }
            OnPropertyChanged("SelectedLevelId");
            OnPropertyChanged("SelectedSectionId");
        }

        public void LoadStudents()
        {
            Students = null;
            Students = new ObservableCollection<Data.Student>(_studentsRepository.GetStudents());
            if (Students != null)
                Students = new ObservableCollection<Data.Student>(Students.AsEnumerable().OrderBy(student => student.Level.GradeLevel, new NaturalSortComparer<string>()).ToList());
        }

        public void OnAdd()
        {
            AddRequested(new Data.Student { StudentGuid = Guid.NewGuid() });
        }

        public void OnEdit(Data.Student selectedStudent)
        {
            EditRequested(selectedStudent);
        }

        private async void OnDelete(Data.Student student)
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
    }
}
