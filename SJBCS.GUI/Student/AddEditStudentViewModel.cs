﻿using AMS.Utilities;
using MaterialDesignThemes.Wpf;
using Microsoft.Win32;
using SJBCS.Data;
using SJBCS.GUI.Dialogs;
using SJBCS.Services.Repository;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity;

namespace SJBCS.GUI.Student
{
    public class AddEditStudentViewModel : BindableBase
    {
        private bool _editMode;
        private IStudentsRepository _studentsRepository;
        private ILevelsRepository _levelsRepository;
        private ISectionsRepository _sectionsRepository;
        private IContactsRepository _contactsRepository;
        private IRelOrganizationsRepository _relOrganizationsRepository;
        private IRelBiometricsRepository _relBiometricsRepository;
        private IBiometricsRepository _biometricsRepository;

        public bool EditMode
        {
            get { return _editMode; }
            set { SetProperty(ref _editMode, value); }
        }

        private string _selectedImage;

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
                    Student.LevelID = SelectedLevelId;
                    Student.SectionID = SelectedSectionId;
                }
            }
        }

        public string SelectedImage
        {
            get { return _selectedImage; }
            set { SetProperty(ref _selectedImage, value); }
        }

        private string _contactNumber;

        public string ContactNumber
        {
            get { return _contactNumber; }
            set { SetProperty(ref _contactNumber, value); }
        }

        private EditableStudent _student;

        public EditableStudent Student
        {
            get { return _student; }
            set { SetProperty(ref _student, value); }
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

        private ObservableCollection<Contact> _addedContacts;

        public ObservableCollection<Contact> AddedContacts
        {
            get { return _addedContacts; }
            set { SetProperty(ref _addedContacts, value); }
        }

        private ObservableCollection<Contact> _deletedContacts;

        public ObservableCollection<Contact> DeletedContacts
        {
            get { return _deletedContacts; }
            set { SetProperty(ref _deletedContacts, value); }
        }

        private ObservableCollection<RelOrganization> _addedGroups;

        public ObservableCollection<RelOrganization> AddedGroups
        {
            get { return _addedGroups; }
            set { SetProperty(ref _addedGroups, value); }
        }

        private ObservableCollection<RelOrganization> _deletedGroups;

        public ObservableCollection<RelOrganization> DeletedGroups
        {
            get { return _deletedGroups; }
            set { SetProperty(ref _deletedGroups, value); }
        }

        private ObservableCollection<Biometric> _addedBiometrics;

        public ObservableCollection<Biometric> AddedBiometrics
        {
            get { return _addedBiometrics; }
            set { SetProperty(ref _addedBiometrics, value); }
        }

        private ObservableCollection<Biometric> _deletedBiometrics;

        public ObservableCollection<Biometric> DeletedBiometrics
        {
            get { return _deletedBiometrics; }
            set { SetProperty(ref _deletedBiometrics, value); }
        }

        private ObservableCollection<RelBiometric> _addedRelBiometrics;

        public ObservableCollection<RelBiometric> AddedRelBiometrics
        {
            get { return _addedRelBiometrics; }
            set { SetProperty(ref _addedRelBiometrics, value); }
        }

        private ObservableCollection<RelBiometric> _deletedRelBiometrics;

        public ObservableCollection<RelBiometric> DeletedRelBiometrics
        {
            get { return _deletedRelBiometrics; }
            set { SetProperty(ref _deletedRelBiometrics, value); }
        }

        private Data.Student _editingStudent;

        public void SetStudent(Data.Student student)
        {
            _editingStudent = student;

            if (Student != null) Student.ErrorsChanged -= RaiseCanExecuteChanged;
            Student = new EditableStudent();
            Student.ErrorsChanged += RaiseCanExecuteChanged;
            CopyStudent(student, Student);
        }

        private void RaiseCanExecuteChanged(object sender, DataErrorsChangedEventArgs e)
        {
            SaveCommand.RaiseCanExecuteChanged();
        }

        private void CopyStudent(Data.Student source, EditableStudent target)
        {
            target.StudentGuid = source.StudentGuid;

            if (EditMode)
            {
                target.StudentID = source.StudentID;
                target.FirstName = source.FirstName;
                target.MiddleName = source.MiddleName;
                target.LastName = source.LastName;
                target.LevelID = source.LevelID;
                target.SectionID = source.SectionID;
                target.BirthDate = source.BirthDate;
                target.Gender = source.Gender;
                target.Street = source.Street;
                target.City = source.City;
                target.State = source.State;
                target.ImageData = source.ImageData;

                target.Attendances = source.Attendances;
                target.Contacts = new ObservableCollection<Contact>(source.Contacts);
                target.Level = source.Level;
                target.Section = source.Section;
                target.RelBiometrics = new ObservableCollection<RelBiometric>(source.RelBiometrics);
                target.RelDistributionLists = source.RelDistributionLists;
                target.RelOrganizations = new ObservableCollection<RelOrganization>(source.RelOrganizations);

                Sections = new ObservableCollection<Section>(_sectionsRepository.GetSections(Student.LevelID));
                SelectedImage = Student.ImageData;
            }
        }

        public RelayCommand CancelCommand { get; private set; }
        public RelayCommand SaveCommand { get; private set; }
        public RelayCommand OpenFileCommand { get; private set; }
        public RelayCommand AddContactCommand { get; private set; }
        public RelayCommand<Contact> DeleteContactCommand { get; private set; }
        public RelayCommand AddGroupCommand { get; private set; }
        public RelayCommand<RelOrganization> DeleteGroupCommand { get; private set; }
        public RelayCommand EnrollBiometricCommand { get; private set; }
        public RelayCommand<RelBiometric> DeleteBiometricCommand { get; private set; }

        public event Action Done = delegate { };

        public AddEditStudentViewModel(IStudentsRepository studentsRepository, ILevelsRepository levelsRepository, ISectionsRepository sectionsRepository, IContactsRepository contactsRepository, IRelBiometricsRepository relBiometricsRepository, IBiometricsRepository biometricsRepository, IRelOrganizationsRepository relOrganizationsRepository)
        {
            _studentsRepository = studentsRepository;
            _levelsRepository = levelsRepository;
            _sectionsRepository = sectionsRepository;
            _contactsRepository = contactsRepository;
            _relOrganizationsRepository = relOrganizationsRepository;
            _relBiometricsRepository = relBiometricsRepository;
            _biometricsRepository = biometricsRepository;

            CancelCommand = new RelayCommand(OnCancel);
            SaveCommand = new RelayCommand(OnSave, CanSave);
            OpenFileCommand = new RelayCommand(OnOpenFile);

            AddContactCommand = new RelayCommand(OnAddContact);
            DeleteContactCommand = new RelayCommand<Contact>(OnDeleteContact);

            AddGroupCommand = new RelayCommand(OnAddGroup);
            DeleteGroupCommand = new RelayCommand<RelOrganization>(OnDeleteGroup);

            EnrollBiometricCommand = new RelayCommand(OnEnrollBiometric);
            DeleteBiometricCommand = new RelayCommand<RelBiometric>(OnDeleteBiometric);

        }

        private async void OnDeleteBiometric(RelBiometric relBiometric)
        {
            //Show Delete Biometric Dialog
            var view = new DialogBoxView
            {
                DataContext = new DialogBoxViewModel("Are you sure you want to unenroll this finger?")
            };

            //show the dialog
            var result = await DialogHost.Show(view, "RootDialog", ClosingEventHandler);

            //check the result...
            Console.WriteLine("Dialog was closed, the CommandParameter used to close it was : " + (result.ToString()));

            if ((bool)result)
            {
                Student.RelBiometrics.Remove(relBiometric);
                DeletedRelBiometrics.Add(relBiometric);
            }
        }

        private async void OnEnrollBiometric()
        {
            if (Student.RelBiometrics.Count() == 2)
            {
                MessageDialog.OpenDialog(MessageType.Error, "You're only allowed to enroll 2 fingers");
            }
            else
            {
                //Show Add Biometric Dialog
                var view = new EnrollBiometricsView
                {
                    DataContext = ContainerHelper.Container.Resolve<EnrollBiometricsViewModel>()
                };

                //show the dialog
                var result = await DialogHost.Show(view, "RootDialog", ClosingEventHandler);

                //check the result...
                Console.WriteLine("Dialog was closed, the CommandParameter used to close it was : " + (result.ToString()));

                if (result.ToString().ToLower() != "false")
                {
                    RelBiometric relBiometric = new RelBiometric { RelBiometricID = Guid.NewGuid() };
                    relBiometric.Biometric = (Biometric)result;
                    relBiometric.StudentID = Student.StudentGuid;


                    if (Student.RelBiometrics.FirstOrDefault() == null)
                    {
                        relBiometric.Biometric.FingerName = "Left";
                    }
                    else
                    {
                        Biometric temp = Student.RelBiometrics.FirstOrDefault().Biometric;

                        if (temp.FingerName == "Left")
                        {
                            relBiometric.Biometric.FingerName = "Right";
                        }
                        else
                        {
                            relBiometric.Biometric.FingerName = "Left";
                        }
                    }

                    Student.RelBiometrics.Add(relBiometric);
                    AddedRelBiometrics.Add(relBiometric);
                }



            }
        }

        private async void OnDeleteContact(Contact contact)
        {
            //Show Delete Contact Dialog
            var view = new DialogBoxView
            {
                DataContext = new DialogBoxViewModel("Are you sure you want to remove contact?")
            };

            //show the dialog
            var result = await DialogHost.Show(view, "RootDialog", ClosingEventHandler);

            //check the result...
            Console.WriteLine("Dialog was closed, the CommandParameter used to close it was : " + (result.ToString()));

            if ((bool)result)
            {
                Student.Contacts.Remove(contact);
                DeletedContacts.Add(contact);
            }
        }

        private async void OnAddContact()
        {
            //Show Add Contact Dialog
            var view = new AddContactView
            {
                DataContext = ContainerHelper.Container.Resolve<AddContactViewModel>()
            };

            //show the dialog
            var result = await DialogHost.Show(view, "RootDialog", ClosingEventHandler);

            //check the result...
            Console.WriteLine("Dialog was closed, the CommandParameter used to close it was : " + (result.ToString()));

            if (result.ToString().ToLower() != "false")
            {
                Contact contact = new Contact { ContactID = Guid.NewGuid() };
                contact.ContactNumber = result.ToString();
                contact.StudentID = Student.StudentGuid;
                Student.Contacts.Add(contact);
                AddedContacts.Add(contact);
            }

        }

        private async void OnDeleteGroup(RelOrganization relOrganization)
        {
            //Show Delete Contact Dialog
            var view = new DialogBoxView
            {
                DataContext = new DialogBoxViewModel("Are you sure you want to remove student from this group?")
            };

            //show the dialog
            var result = await DialogHost.Show(view, "RootDialog", ClosingEventHandler);

            //check the result...
            Console.WriteLine("Dialog was closed, the CommandParameter used to close it was : " + (result.ToString()));

            if ((bool)result)
            {
                Student.RelOrganizations.Remove(relOrganization);
                DeletedGroups.Add(relOrganization);
            }
        }

        private async void OnAddGroup()
        {
            //Show Add Contact Dialog
            var view = new AddGroupView
            {
                DataContext = ContainerHelper.Container.Resolve<AddGroupViewModel>()
            };

            //show the dialog
            var result = await DialogHost.Show(view, "RootDialog", ClosingEventHandler);

            //check the result...
            Console.WriteLine("Dialog was closed, the CommandParameter used to close it was : " + (result.ToString()));

            if (result.ToString().ToLower() != "false")
            {
                RelOrganization relOrganization = new RelOrganization { RelOrganizationID = Guid.NewGuid() };
                relOrganization.OrganizationID = ((Organization)result).OrganizationID;
                relOrganization.StudentID = Student.StudentGuid;
                Student.RelOrganizations.Add(relOrganization);
                AddedGroups.Add(relOrganization);
            }

        }

        private void OnOpenFile()
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();

            // Set filter for file extension and default file extension 
            dlg.DefaultExt = ".png";
            dlg.Filter = "JPEG Files (*.jpeg)|*.jpeg|PNG Files (*.png)|*.png|JPG Files (*.jpg)|*.jpg|GIF Files (*.gif)|*.gif";


            // Display OpenFileDialog by calling ShowDialog method 
            Nullable<bool> result = dlg.ShowDialog();


            // Get the selected file name and display in a TextBox 
            if (result == true)
            {
                SelectedImage = dlg.FileName;
                Student.ImageData = SelectedImage;
            }

        }

        private bool CanSave()
        {
            return !Student.HasErrors;
        }

        private void OnSave()
        {
            UpdateStudent(Student, _editingStudent);

            //Update Student Table
            if (EditMode)
            {
                _studentsRepository.UpdateStudent(_editingStudent);
            }
            else
            {
                _studentsRepository.AddStudent(_editingStudent);
            }

            //Update Contacts
            foreach (Contact contact in AddedContacts)
            {
                _contactsRepository.AddContact(contact);
            }
            foreach (Contact contact in DeletedContacts)
            {
                _contactsRepository.DeleteContact(contact.ContactID);
            }

            //Update Groups
            foreach (RelOrganization group in AddedGroups)
            {
                _relOrganizationsRepository.AddRelOrganization(group);
            }
            foreach (RelOrganization group in DeletedGroups)
            {
                _relOrganizationsRepository.DeleteRelOrganization(group.RelOrganizationID);
            }

            //Update Biometrics
            foreach (RelBiometric relBiometric in AddedRelBiometrics)
            {
                _relBiometricsRepository.AddRelBiometric(relBiometric);
            }
            foreach (RelBiometric relBiometric in DeletedRelBiometrics)
            {
                _relBiometricsRepository.DeleteRelBiometric(relBiometric.RelBiometricID);
                _biometricsRepository.DeleteBiometric(relBiometric.FingerID);
            }
            Done();
        }

        private void UpdateStudent(EditableStudent source, Data.Student target)
        {
            target.StudentID = source.StudentID;
            target.FirstName = source.FirstName;
            target.MiddleName = source.MiddleName;
            target.LastName = source.LastName;
            target.LevelID = source.LevelID;
            target.SectionID = source.SectionID;
            target.BirthDate = source.BirthDate;
            target.Gender = source.Gender;
            target.Street = source.Street;
            target.City = source.City;
            target.State = source.State;
            target.ImageData = source.ImageData;
        }

        private void OnCancel()
        {
            Done();
        }

        public void LoadComboBox()
        {
            AddedContacts = new ObservableCollection<Contact>();
            DeletedContacts = new ObservableCollection<Contact>();
            AddedBiometrics = new ObservableCollection<Biometric>();
            DeletedBiometrics = new ObservableCollection<Biometric>();
            AddedGroups = new ObservableCollection<RelOrganization>();
            DeletedGroups = new ObservableCollection<RelOrganization>();
            AddedRelBiometrics = new ObservableCollection<RelBiometric>();
            DeletedRelBiometrics = new ObservableCollection<RelBiometric>();

            Levels = new ObservableCollection<Level>(_levelsRepository.GetLevels());

            if (Student != null && EditMode)
            {
                SelectedLevelId = Student.LevelID;
                SelectedSectionId = Student.SectionID;
            }
            else
            {
                SelectedLevelId = Levels[0].LevelID;
                SelectedSectionId = Sections[0].SectionID;
            }
        }
    }
}
