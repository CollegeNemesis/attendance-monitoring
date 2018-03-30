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
using Unity;

namespace SJBCS.GUI.Student
{
    public class SectionViewModel : BindableBase
    {
        private ILevelsRepository _levelsRepository;
        private ISectionsRepository _sectionsRepository;
        private IStudentsRepository _studentsRepository;
        private Section _editingSection;

        private AddSectionViewModel currentViewModel;
        public AddSectionViewModel CurrentViewModel
        {
            get { return currentViewModel; }
            set { SetProperty(ref currentViewModel, value); }
        }

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

        private bool _editMode;
        public bool EditMode
        {
            get { return _editMode; }
            set { SetProperty(ref _editMode, value); }
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
                EditCommand.RaiseCanExecuteChanged();
                DeleteCommand.RaiseCanExecuteChanged();
            }
        }
        
        public RelayCommand EditCommand { get; private set; }
        public RelayCommand SaveCommand { get; private set; }
        public RelayCommand DeleteCommand { get; private set; }

        public AddSectionViewModel _addSectionViewModel;

        public SectionViewModel(ILevelsRepository levelsRepository, ISectionsRepository sectionsRepository, IStudentsRepository studentsRepository)
        {
            _levelsRepository = levelsRepository;
            _sectionsRepository = sectionsRepository;
            _studentsRepository = studentsRepository;

            _addSectionViewModel = ContainerHelper.Container.Resolve<AddSectionViewModel>();

            currentViewModel = _addSectionViewModel;

            EditCommand = new RelayCommand(OnEdit, CanEdit);
            SaveCommand = new RelayCommand(OnSave, CanSave);
            DeleteCommand = new RelayCommand(OnDelete, CanDelete);
            Initialize();
        }

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
            target.LevelID = source.LevelID;
            target.Levels = Levels;
            target.Level = _levelsRepository.GetLevel(SelectedLevelId);

            target.OrigSectionName = source.SectionName;
            target.SectionName = source.SectionName;
            target.StartTime = source.StartTime.ToString();
            target.EndTime = source.EndTime.ToString();
        }

        private bool CanSave()
        {
            if (EditableSection == null)
                return false;

            if (EditableSection.SectionName == null || EditableSection.StartTime == null || EditableSection.EndTime == null)
                return false;

            return !EditableSection.HasErrors;
        }

        private bool CanDelete()
        {
            if (SelectedSection == null)
                return false;
            return true;
        }

        private bool CanEdit()
        {
            if (SelectedSection == null)
                return false;
            return true;
        }

        private async void OnDelete()
        {
            List<Data.Student> Students = _studentsRepository.GetStudents();

            if (Students.SingleOrDefault(i => i.SectionID == _selectedSection.SectionID) != null)
            {
                //Show Delete Contact Dialog
                var view = new DialogBoxView
                {
                    DataContext = new DialogBoxViewModel(MessageType.Informational, "Section is currently link to students. You cannot delete it.")
                };

                //show the dialog
                var result = await DialogHost.Show(view, "RootDialog", ClosingEventHandler);

                //check the result...
                Console.WriteLine("Dialog was closed, the CommandParameter used to close it was : " + (result.ToString()));
            }

            else
            {

                _sectionsRepository.DeleteSection(_selectedSection.SectionID);
                Initialize();
            }
        }

        private void OnSave()
        {
            UpdateSection(EditableSection, _editingSection);
            //Update Student Table
            if (EditMode)
            {
                _sectionsRepository.UpdateSection(SelectedSection);
            }

            Initialize();
        }

        private void UpdateSection(EditableSection source, Section target)
        {
            target.SectionName = source.SectionName;
            target.StartTime = DateTime.Parse(source.StartTime).TimeOfDay;
            target.EndTime = DateTime.Parse(source.EndTime).TimeOfDay;
        }

        private void OnEdit()
        {
            EditMode = true;
            SetSection(_selectedSection);
        }

        private void RaiseCanExecuteChanged(object sender, DataErrorsChangedEventArgs e)
        {
            SaveCommand.RaiseCanExecuteChanged();
            EditCommand.RaiseCanExecuteChanged();
            DeleteCommand.RaiseCanExecuteChanged();
        }

        public void Initialize()
        {
            RefreshList();
            PopulateLevelComboBox();

        }

        private void PopulateLevelComboBox()
        {
            SelectedLevelId = Levels[0].LevelID;
            EditMode = false;
        }

        private void RefreshList()
        {
            Levels = new ObservableCollection<Level>(_levelsRepository.GetLevels());
        }
    }
}
