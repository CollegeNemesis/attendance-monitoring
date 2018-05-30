using SJBCS.Data;
using SJBCS.GUI.Utilities;
using SJBCS.Services.Repository;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace SJBCS.GUI.Student
{
    public class AddEditSectionViewModel : BindableBase
    {
        private ILevelsRepository _levelsRepository;
        private ISectionsRepository _sectionsRepository;
        private IStudentsRepository _studentsRepository;
        private Section _editingSection;

        private ObservableCollection<Level> _levels;
        public ObservableCollection<Level> Levels
        {
            get { return _levels; }
            set { SetProperty(ref _levels, value); }
        }

        private Section _section;
        public Section Section
        {
            get { return _section; }
            set { SetProperty(ref _section, value); }
        }

        private Guid _selectedLevelId;
        public Guid SelectedLevelId
        {
            get => _selectedLevelId;
            set
            {
                EditableSection.LevelID = value;
                EditableSection.Level = _levelsRepository.GetLevel(value);
                SetProperty(ref _selectedLevelId, value);
            }
        }

        private EditableSection editableSection;
        public EditableSection EditableSection
        {
            get => editableSection;
            set
            {
                SetProperty(ref editableSection, value);
            }
        }

        private bool _editMode;
        public bool EditMode
        {
            get { return _editMode; }
            set { SetProperty(ref _editMode, value); }
        }

        public RelayCommand SaveCommand { get; private set; }
        public RelayCommand CancelCommand { get; private set; }

        public event Action Done = delegate { };

        public AddEditSectionViewModel(ILevelsRepository levelsRepository, ISectionsRepository sectionsRepository, IStudentsRepository studentsRepository)
        {
            _levelsRepository = levelsRepository;
            _sectionsRepository = sectionsRepository;
            _studentsRepository = studentsRepository;
            SaveCommand = new RelayCommand(OnSave, CanSave);
            CancelCommand = new RelayCommand(OnCancel);
            Initialize();
        }

        private void OnCancel()
        {
            Done();
        }

        private void OnSave()
        {
            UpdateSection(EditableSection, _editingSection);

            //Update Student Table
            if (EditMode)
            {
                _sectionsRepository.UpdateSection(_editingSection);
            }
            else
            {
                _sectionsRepository.AddSection(_editingSection);
            }

            Done();
        }

        private bool CanSave()
        {
            if (string.IsNullOrEmpty(EditableSection.SectionName) || string.IsNullOrEmpty(EditableSection.StartTime) || string.IsNullOrEmpty(EditableSection.EndTime))
            {
                return false;
            }

            TimeSpan start = DateTime.Parse(EditableSection.StartTime).TimeOfDay;
            TimeSpan end = DateTime.Parse(EditableSection.EndTime).TimeOfDay;

            if (start >= end)
            {
                return false;
            }

            return !EditableSection.HasErrors;
        }

        private void CopySection(Section source, EditableSection target)
        {
            target.SectionID = source.SectionID;
            target.LevelID = SelectedLevelId;
            target.Level = _levelsRepository.GetLevel(SelectedLevelId);
            target.Levels = Levels;
            target.SectionName = source.SectionName;
            target.StartTime = source.StartTime.ToString();
            target.EndTime = source.EndTime.ToString();
        }

        public void SetSection(Section section)
        {
            _editingSection = section;
            if (EditableSection != null) EditableSection.ErrorsChanged -= RaiseCanExecuteChanged;

            EditableSection = new EditableSection();
            EditableSection.ErrorsChanged += RaiseCanExecuteChanged;
            CopyStudent(section, EditableSection);
        }

        private void CopyStudent(Section source, EditableSection target)
        {
            target.SectionID = source.SectionID;
            target.SectionID = source.LevelID;
            target.StartTime = (new TimeSpan(7, 30, 0)).ToString();
            target.EndTime = (new TimeSpan(15, 30, 0)).ToString();

            if (EditMode)
            {
                target.EditMode = true;
                target.OrigSectionName = source.SectionName;
                target.SectionName = source.SectionName;
                target.StartTime = source.StartTime.ToString();
                target.EndTime = source.EndTime.ToString();
            }
        }

        private void UpdateSection(EditableSection source, Section target)
        {
            target.SectionName = source.SectionName;
            target.StartTime = DateTime.Parse(source.StartTime).TimeOfDay;
            target.EndTime = DateTime.Parse(source.EndTime).TimeOfDay;
        }


        private void RaiseCanExecuteChanged(object sender, DataErrorsChangedEventArgs e)
        {
            SaveCommand.RaiseCanExecuteChanged();
        }

        public void Initialize()
        {
        }

        private void PopulateComboBox()
        {
            Levels = new ObservableCollection<Level>(_levelsRepository.GetLevels());
            SelectedLevelId = Levels[0].LevelID;
        }

    }
}
