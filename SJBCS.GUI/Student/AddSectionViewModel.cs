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

namespace SJBCS.GUI.Student
{
    public class AddSectionViewModel : BindableBase
    {
        private ILevelsRepository _levelsRepository;
        private ISectionsRepository _sectionsRepository;
        private IStudentsRepository _studentsRepository;
        private Section _editingSection;
        public RelayCommand SaveCommand { get; private set; }

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
                AddableSection.LevelID = value;
                AddableSection.Level = _levelsRepository.GetLevel(value);
                SetProperty(ref _selectedLevelId, value);
            }
        }

        private AddableSection addableSection;
        public AddableSection AddableSection
        {
            get => addableSection;
            set
            {
                SetProperty(ref addableSection, value);
                SaveCommand.RaiseCanExecuteChanged();
            }
        }

        public AddSectionViewModel(ILevelsRepository levelsRepository, ISectionsRepository sectionsRepository, IStudentsRepository studentsRepository)
        {
            _levelsRepository = levelsRepository;
            _sectionsRepository = sectionsRepository;
            _studentsRepository = studentsRepository;
            SaveCommand = new RelayCommand(OnSave, CanSave);
            Initialize();
        }

        private bool CanSave()
        {
            if (AddableSection == null)
                return false;

            if (string.IsNullOrEmpty(AddableSection.StartTime) || string.IsNullOrEmpty(AddableSection.EndTime))
                return false;

            if (DateTime.Parse(AddableSection.StartTime).TimeOfDay >= DateTime.Parse(AddableSection.EndTime).TimeOfDay)
                return false;

            return !AddableSection.HasErrors && !AddableSection.HasExceptions;
        }

        private void OnSave()
        {
            UpdateSection(AddableSection, _editingSection);
            _sectionsRepository.AddSection(_editingSection);

            DialogHost.CloseDialogCommand.Execute(new object(), null);
        }

        private void CopySection(Section source, AddableSection target)
        {
            target.SectionID = source.SectionID;
            target.LevelID = SelectedLevelId;
            target.Level = _levelsRepository.GetLevel(SelectedLevelId);
            target.Levels = Levels;
            target.SectionName = source.SectionName;
            target.StartTime = source.StartTime.ToString();
            target.EndTime = source.EndTime.ToString();
        }

        private void SetSection(Section section)
        {
            _editingSection = section;

            if (AddableSection != null) AddableSection.ErrorsChanged -= RaiseCanExecuteChanged;

            AddableSection = new AddableSection();
            AddableSection.ErrorsChanged += RaiseCanExecuteChanged;

            CopySection(section, AddableSection);
        }

        private void UpdateSection(AddableSection source, Section target)
        {
            target.SectionID = source.SectionID;
            target.LevelID = SelectedLevelId;
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
            AddableSection = new AddableSection();
            Section = new Section();
            Section.SectionID = Guid.NewGuid();
            Section.StartTime = new TimeSpan(7, 0, 0);
            Section.EndTime = new TimeSpan(15, 0, 0);
            PopulateComboBox();
            SetSection(Section);
        }

        private void PopulateComboBox()
        {
            Levels = new ObservableCollection<Level>(_levelsRepository.GetLevels());
            SelectedLevelId = Levels[0].LevelID;
        }

    }
}
