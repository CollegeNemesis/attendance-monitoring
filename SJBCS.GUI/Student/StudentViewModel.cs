using SJBCS.Data;
using SJBCS.GUI.Dialogs;
using SJBCS.GUI.Utilities;
using SJBCS.Services.Repository;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Controls;

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
                if (value != null | value != new Guid())
                    Sections = new ObservableCollection<Section>(_levelsRepository.GetLevel(value).Sections.OrderBy(section => section.SectionName));
                if (Sections.Any())
                    SelectedSectionId = Sections.FirstOrDefault().SectionID;
                SetProperty(ref _selectedLevelId, value);
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
        public RelayCommand ClearCommand { get; private set; }
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

        private string _searchInput;
        private ObservableCollection<Data.Student> _allStudents;
        public string SearchInput
        {
            get { return _searchInput; }
            set
            {
                SetProperty(ref _searchInput, value);
                FilterStudents(_searchInput);
            }
        }

        private string _selectedSearch;
        public string SelectedSearch
        {
            get { return _selectedSearch; }
            set
            {
                SetProperty(ref _selectedSearch, value);
                OnClear();
            }
        }

        private List<string> _searchCriteria;
        public List<string> SearchCriteria
        {
            get { return _searchCriteria; }
            set
            {
                SetProperty(ref _searchCriteria, value);
            }
        }

        public StudentViewModel(IStudentsRepository studentsRepository, ILevelsRepository levelsRepository, ISectionsRepository sectionsRepository, IContactsRepository contactsRepository, IRelBiometricsRepository relBiometricsRepository, IBiometricsRepository biometricsRepository)
        {
            AddCommand = new RelayCommand(OnAdd);
            DeleteCommand = new RelayCommand<Data.Student>(OnDelete);
            ClearCommand = new RelayCommand(OnClear);
            EditCommand = new RelayCommand<Data.Student>(OnEdit);

            _studentsRepository = studentsRepository;
            _contactsRepository = contactsRepository;
            _biometricsRepository = biometricsRepository;
            _relBiometricsRepository = relBiometricsRepository;
            _levelsRepository = levelsRepository;
            _sectionsRepository = sectionsRepository;

            Initialize();
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
                SelectedLevelId = Levels.FirstOrDefault().LevelID;

            SearchCriteria = new List<string>();
            SearchCriteria.Add("Student ID");
            SearchCriteria.Add("First Name");
            SearchCriteria.Add("Last Name");
            SelectedSearch = SearchCriteria[0];

        }

        public void LoadStudents()
        {
            _allStudents = new ObservableCollection<Data.Student>(_studentsRepository.GetStudents());
            Students = _allStudents;
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
            var result = await DialogHelper.ShowDialog(DialogType.Validation, "Are you sure you want to delete this student?");

            if (result)
            {
                try
                {
                    if (!student.Attendances.Any())
                    {
                        _studentsRepository.DeleteStudent(student.StudentGuid);
                        LoadStudents();
                    }
                    else
                        throw new ArgumentException("Cannot delete an active student.");
                }
                catch (Exception error)
                {
                    result = await DialogHelper.ShowDialog(DialogType.Error, error.Message);
                }
            }
        }

        private void FilterStudents(string searchInput)
        {
            if (string.IsNullOrWhiteSpace(searchInput))
            {
                Students = new ObservableCollection<Data.Student>(_allStudents);
                return;
            }
            else
            {
                if(SelectedSearch == "Student ID")
                    Students = new ObservableCollection<Data.Student>(_allStudents.Where(student => student.StudentID.ToLower().Contains(searchInput.ToLower())));
                else if (SelectedSearch == "First Name")
                    Students = new ObservableCollection<Data.Student>(_allStudents.Where(student => student.FirstName.ToLower().Contains(searchInput.ToLower())));
                else if (SelectedSearch == "Last Name")
                    Students = new ObservableCollection<Data.Student>(_allStudents.Where(student => student.LastName.ToLower().Contains(searchInput.ToLower())));
            }
        }

        public void OnClear()
        {
            SearchInput = null;
        }
    }
}
