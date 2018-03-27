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

        private ObservableCollection<Level> _levels;

        public ObservableCollection<Level> Levels
        {
            get { return _levels; }
            set { SetProperty(ref _levels, value); }
        }

        private EditableSection editableSection;

        public EditableSection EditableSection
        {
            get { return editableSection; }
            set
            {
                SetProperty(ref editableSection, value);
            }
        }

        private Section _editingSection;

        private void SetSection(Section section)
        {
            _editingSection = section;

            if (EditableSection != null) EditableSection.ErrorsChanged -= RaiseCanExecuteChanged;

            EditableSection = new EditableSection();
            EditableSection.ErrorsChanged += RaiseCanExecuteChanged;

            CopySection(section, EditableSection);
        }

        private void CopySection(Section source, EditableSection target)
        {
            target.SectionID = source.SectionID;
            target.LevelID = SelectedLevelId;

            target.SectionName = source.SectionName;
            target.StartTime = source.StartTime.ToString();
            target.EndTime = source.EndTime.ToString();
        }

        private Section _section;

        public Section Section
        {
            get { return _section; }
            set { SetProperty(ref _section, value); }
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
                SetProperty(ref _selectedLevelId, value);
                if (_selectedLevelId != null)
                {
                    Sections = new ObservableCollection<Section>(_sectionsRepository.GetSections(_selectedLevelId));
                    if (Sections.Count > 0)
                        SelectedSectionId = Sections[0].SectionID;

                }
            }
        }

        private Guid _selectedSectionId;

        public Guid SelectedSectionId
        {
            get { return _selectedSectionId; }
            set
            {
                SetProperty(ref _selectedSectionId, value);
                if (_selectedLevelId != null && _selectedSectionId != null)
                {
                }
            }
        }

        public RelayCommand AddCommand { get; private set; }

        public AddSectionViewModel(ILevelsRepository levelsRepository, ISectionsRepository sectionsRepository, IStudentsRepository studentsRepository)
        {
            _levelsRepository = levelsRepository;
            _sectionsRepository = sectionsRepository;
            _studentsRepository = studentsRepository;

            AddCommand = new RelayCommand(OnAdd, CanAdd);

            Initialize();
        }

        private bool CanAdd()
        {
            if (Section == null)
                return false;

            if (EditableSection.SectionName == null || EditableSection.StartTime == null || EditableSection.EndTime == null || EditableSection.HasErrors)
                return false;

            else
                return true;
        }

        private void OnAdd()
        {
            UpdateSection(EditableSection, _editingSection);
            _sectionsRepository.AddSection(_editingSection);

            DialogHost.CloseDialogCommand.Execute(new object(), null);
        }

        private void UpdateSection(EditableSection source, Section target)
        {
            target.SectionID = source.SectionID;
            target.LevelID = SelectedLevelId;
            target.SectionName = source.SectionName;
            target.StartTime = DateTime.Parse(source.StartTime).TimeOfDay;
            target.EndTime = DateTime.Parse(source.EndTime).TimeOfDay;
        }

        private void RaiseCanExecuteChanged(object sender, DataErrorsChangedEventArgs e)
        {
            AddCommand.RaiseCanExecuteChanged();
        }

        public void Initialize()
        {
            RefreshList();
            PopulateLevelComboBox();
            SetSection(Section);
        }

        private void PopulateLevelComboBox()
        {
            SelectedLevelId = Levels[0].LevelID;
            SelectedSectionId = Sections[0].SectionID;
        }

        private void RefreshList()
        {
            Levels = new ObservableCollection<Level>(_levelsRepository.GetLevels());
            EditableSection = new EditableSection();
            Section = new Section() { SectionID = Guid.NewGuid() };
        }
    }
}
