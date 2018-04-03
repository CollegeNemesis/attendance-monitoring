using AMS.Utilities;
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
using System.ComponentModel.DataAnnotations;
using Unity;

namespace SJBCS.GUI.Student
{
    public class AddEditStudentViewModel : BindableBase
    {
        private IStudentsRepository _studentsRepository;
        private ILevelsRepository _levelsRepository;
        private ISectionsRepository _sectionsRepository;
        private IContactsRepository _contactsRepository;
        private IRelOrganizationsRepository _relOrganizationsRepository;
        private IRelBiometricsRepository _relBiometricsRepository;
        private IBiometricsRepository _biometricsRepository;
        private IOrganizationsRepository _organizationsRepository;

        private EnrollBiometricsViewModel currentViewModel;
        public EnrollBiometricsViewModel CurrentViewModel
        {
            get { return currentViewModel; }
            set { SetProperty(ref currentViewModel, value); }
        }

        private bool _editMode;
        public bool EditMode
        {
            get { return _editMode; }
            set { SetProperty(ref _editMode, value); }
        }

        private Guid _selectedGroupId;
        public Guid SelectedGroupId
        {
            get { return _selectedGroupId; }
            set { SetProperty(ref _selectedGroupId, value); }
        }

        private Organization _selectedOrganization;
        public Organization SelectedOrganization
        {
            get { return _selectedOrganization; }
            set { SetProperty(ref _selectedOrganization, value); }
        }


        private Guid _selectedLevelId;
        public Guid SelectedLevelId
        {
            get { return _selectedLevelId; }
            set
            {
                Sections = new ObservableCollection<Section>(_sectionsRepository.GetSections(value));
                if(Sections.Any())
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
                SetProperty(ref _selectedSectionId, value);
            }
        }

        private ObservableCollection<Organization> _organizations;
        public ObservableCollection<Organization> Organizations
        {
            get { return _organizations; }
            set { SetProperty(ref _organizations, value); }
        }

        private string _selectedImage;
        public string SelectedImage
        {
            get { return _selectedImage; }
            set { SetProperty(ref _selectedImage, value); }
        }

        private EditableContact _editableContact;
        public EditableContact EditableContact
        {
            get
            {
                return _editableContact;
            }

            set
            {
                SetProperty(ref _editableContact, value);
            }
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

        private Biometric _biometric;
        public Biometric Biometric
        {
            get { return _biometric; }
            set { SetProperty(ref _biometric, value); }
        }

        private Data.Student _editingStudent;

        public RelayCommand CancelCommand { get; private set; }
        public RelayCommand SaveCommand { get; private set; }
        public RelayCommand OpenFileCommand { get; private set; }
        public RelayCommand AddContactCommand { get; private set; }
        public RelayCommand<Contact> DeleteContactCommand { get; private set; }
        public RelayCommand AddGroupCommand { get; private set; }
        public RelayCommand<Organization> DeleteGroupCommand { get; private set; }
        public RelayCommand<Biometric> DeleteBiometricCommand { get; private set; }

        public event Action Done = delegate { };

        public EnrollBiometricsViewModel _enrollBiometricsViewModel;

        public AddEditStudentViewModel(IStudentsRepository studentsRepository, ILevelsRepository levelsRepository, ISectionsRepository sectionsRepository, IContactsRepository contactsRepository, IRelBiometricsRepository relBiometricsRepository, IBiometricsRepository biometricsRepository, IRelOrganizationsRepository relOrganizationsRepository, IOrganizationsRepository organizationsRepository)
        {
            _studentsRepository = studentsRepository;
            _levelsRepository = levelsRepository;
            _sectionsRepository = sectionsRepository;
            _contactsRepository = contactsRepository;
            _relOrganizationsRepository = relOrganizationsRepository;
            _relBiometricsRepository = relBiometricsRepository;
            _biometricsRepository = biometricsRepository;
            _organizationsRepository = organizationsRepository;

            _enrollBiometricsViewModel = ContainerHelper.Container.Resolve<EnrollBiometricsViewModel>();

            currentViewModel = _enrollBiometricsViewModel;

            CancelCommand = new RelayCommand(OnCancel);
            SaveCommand = new RelayCommand(OnSave, CanSave);
            OpenFileCommand = new RelayCommand(OnOpenFile);

            AddContactCommand = new RelayCommand(OnAddContact, CanAddContact);
            DeleteContactCommand = new RelayCommand<Contact>(OnDeleteContact);

            AddGroupCommand = new RelayCommand(OnAddGroup, CanAddGroup);
            DeleteGroupCommand = new RelayCommand<Organization>(OnDeleteGroup);

            DeleteBiometricCommand = new RelayCommand<Biometric>(OnDeleteBiometric);

        }

        public void SetStudent(Data.Student student)
        {
            _editingStudent = student;

            if (Student != null)
            {
                Student.ErrorsChanged -= RaiseCanExecuteChanged;
            }
            if (EditableContact != null) EditableContact.ErrorsChanged -= RaiseCanExecuteChanged;

            Student = new EditableStudent();
            EditableContact = new EditableContact();

            Student.ErrorsChanged += RaiseCanExecuteChanged;
            EditableContact.ErrorsChanged += RaiseCanExecuteChanged;

            CopyStudent(student, Student);
        }

        private void UpdateStudent(EditableStudent source, Data.Student target)
        {
            target.StudentID = source.StudentID;
            target.FirstName = source.FirstName;
            target.MiddleName = source.MiddleName;
            target.LastName = source.LastName;
            target.LevelID = SelectedLevelId;
            target.SectionID = SelectedSectionId;
            target.BirthDate = source.BirthDate;
            target.Gender = source.Gender;
            target.Street = source.Street;
            target.City = source.City;
            target.State = source.State;
            target.ImageData = source.ImageData;
        }

        private void CopyStudent(Data.Student source, EditableStudent target)
        {
            target.StudentGuid = source.StudentGuid;
            target.EditMode = EditMode;

            EditableContact.Contacts = new ObservableCollection<Contact>(source.Contacts.ToList());

            SelectedLevelId = source.LevelID;
            SelectedSectionId = source.SectionID;

            if (EditMode)
            {
                target.OrigStudentId = source.StudentID;
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
            }

            target.Biometrics = new ObservableCollection<Biometric>();
            target.Organizations = new ObservableCollection<Organization>();

            if (target.RelBiometrics != null)
            {
                foreach (RelBiometric relBiometric in target.RelBiometrics)
                {
                    target.Biometrics.Add(relBiometric.Biometric);
                }
            }
            if (target.RelOrganizations != null)
            {
                foreach (RelOrganization group in target.RelOrganizations)
                {
                    target.Organizations.Add(group.Organization);
                }
            }

        }

        private void RaiseCanExecuteChanged(object sender, DataErrorsChangedEventArgs e)
        {
            SaveCommand.RaiseCanExecuteChanged();
            AddContactCommand.RaiseCanExecuteChanged();
            AddGroupCommand.RaiseCanExecuteChanged();
        }

        private async void OnDeleteBiometric(Biometric biometric)
        {
            var view = new DialogBoxView
            {
                DataContext = new DialogBoxViewModel(MessageType.Informational, "Are you sure you want to unenroll this finger?")
            };

            //show the dialog
            var result = await DialogHost.Show(view, "RootDialog", ClosingEventHandler);

            //check the result...
            Console.WriteLine("Dialog was closed, the CommandParameter used to close it was : " + (result.ToString()));

            if ((bool)result)
            {
                Student.Biometrics.Remove(Student.Biometrics.SingleOrDefault(i => i.FingerID == biometric.FingerID));
                DeletedBiometrics.Add(biometric);
            }
        }

        public void OnEnrollBiometric(Biometric biometric)
        {

            Biometric = biometric;

            RelBiometric relBiometric = new RelBiometric { RelBiometricID = Guid.NewGuid() };
            relBiometric.FingerID = Biometric.FingerID;
            relBiometric.StudentID = Student.StudentGuid;

            if (Student.RelBiometrics == null)
            {
                Student.RelBiometrics = new ObservableCollection<RelBiometric>();
            }

            if (Student.RelBiometrics.FirstOrDefault() == null)
            {
                Biometric.FingerName = "Finger 1";
            }
            else
            {
                Biometric temp = Student.Biometrics.FirstOrDefault();

                if (temp.FingerName == "Finger 1")
                {
                    Biometric.FingerName = "Finger 2";
                }
                else
                {
                    Biometric.FingerName = "Finger 1";
                }
            }


            Student.RelBiometrics.Add(relBiometric);
            Student.Biometrics.Add(Biometric);
            _biometricsRepository.AddBiometric(Biometric);

            AddedBiometrics.Add(Biometric);
            AddedRelBiometrics.Add(relBiometric);
        }

        private async void OnDeleteContact(Contact contact)
        {
            //Show Delete Contact Dialog
            var view = new DialogBoxView
            {
                DataContext = new DialogBoxViewModel(MessageType.Warning, "Are you sure you want to remove contact?")
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

            SaveCommand.RaiseCanExecuteChanged();
        }

        private void OnAddContact()
        {
            if (Student.Contacts == null)
                Student.Contacts = new ObservableCollection<Contact>();

            Contact contact = new Contact { ContactID = Guid.NewGuid() };
            contact.ContactNumber = EditableContact.ContactNumber;
            contact.StudentID = Student.StudentGuid;

            Student.Contacts.Add(contact);
            AddedContacts.Add(contact);

            EditableContact.ContactNumber = null;
            EditableContact.Contacts = Student.Contacts;
        }

        private async void OnDeleteGroup(Organization organization)
        {
            //Show Delete Contact Dialog
            var view = new DialogBoxView
            {
                DataContext = new DialogBoxViewModel(MessageType.Warning, "Are you sure you want to remove student from this group?")
            };

            //show the dialog
            var result = await DialogHost.Show(view, "RootDialog", ClosingEventHandler);

            //check the result...
            Console.WriteLine("Dialog was closed, the CommandParameter used to close it was : " + (result.ToString()));

            if ((bool)result)
            {
                DeletedGroups.Add(Student.RelOrganizations.SingleOrDefault(i => i.OrganizationID == organization.OrganizationID));
                Student.RelOrganizations.Remove(Student.RelOrganizations.SingleOrDefault(i => i.OrganizationID == organization.OrganizationID));
                Student.Organizations.Remove(Student.Organizations.SingleOrDefault(i => i.OrganizationID == organization.OrganizationID));
                Organizations.Add(_organizationsRepository.GetOrganization(organization.OrganizationID));
            }

            if (Organizations.Count != 0)
            {
                SelectedGroupId = Organizations.FirstOrDefault().OrganizationID;
            }
            else
            {
                SelectedGroupId = new Guid();
            }

            AddGroupCommand.RaiseCanExecuteChanged();
        }

        private void OnAddGroup()
        {
            if (Student.RelOrganizations == null)
                Student.RelOrganizations = new ObservableCollection<RelOrganization>();

            RelOrganization relOrganization = new RelOrganization { RelOrganizationID = Guid.NewGuid() };
            relOrganization.OrganizationID = SelectedGroupId;
            relOrganization.StudentID = Student.StudentGuid;

            AddedGroups.Add(relOrganization);
            Student.RelOrganizations.Add(relOrganization);
            Student.Organizations.Add(Organizations.SingleOrDefault(i => i.OrganizationID == relOrganization.OrganizationID));

            Organizations.Remove(Organizations.SingleOrDefault(i => i.OrganizationID == relOrganization.OrganizationID));

            if (Organizations.Count != 0)
            {
                SelectedGroupId = Organizations.FirstOrDefault().OrganizationID;
            }
            else
            {
                SelectedGroupId = new Guid();
            }

            AddGroupCommand.RaiseCanExecuteChanged();
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
                Student.ImageData = File.ReadAllBytes(SelectedImage);
            }

        }

        private bool CanSave()
        {
            if (Student.Contacts == null)
            {
                return false;
            }
            if (Student.StudentID == null || Student.LastName == null || Student.FirstName == null || Student.Contacts.FirstOrDefault() == null)
                return false;

            return !Student.HasErrors;
        }

        private bool CanAddContact()
        {
            if (string.IsNullOrEmpty(EditableContact.ContactNumber))
                return false;
            return !EditableContact.HasErrors;
        }

        private bool CanAddBiometric()
        {
            if (Student.RelBiometrics == null)
                return true;

            if (Student.RelBiometrics.Count() == 2)
                return false;

            return true;
        }

        private bool CanAddGroup()
        {
            if (Organizations == null || SelectedGroupId == null || SelectedGroupId == new Guid())
                return false;

            else if (Organizations.Count() == 0)
                return false;

            else
                return true;
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
            foreach (Biometric biometric in DeletedBiometrics)
            {
                _relBiometricsRepository.DeleteRelBiometric(biometric.FingerID);
                _biometricsRepository.DeleteBiometric(biometric.FingerID);
            }

            SelectedImage = null;
            Done();
        }

        private void OnCancel()
        {

            foreach (Biometric biometric in AddedBiometrics)
            {
                _biometricsRepository.DeleteBiometric(biometric.FingerID);
            }
            Done();
        }

        public void RefreshList()
        {
            AddedContacts = new ObservableCollection<Contact>();
            DeletedContacts = new ObservableCollection<Contact>();
            AddedBiometrics = new ObservableCollection<Biometric>();
            DeletedBiometrics = new ObservableCollection<Biometric>();
            AddedGroups = new ObservableCollection<RelOrganization>();
            DeletedGroups = new ObservableCollection<RelOrganization>();
            AddedRelBiometrics = new ObservableCollection<RelBiometric>();
            DeletedRelBiometrics = new ObservableCollection<RelBiometric>();
        }

        public void PopulateLevelComboBox()
        {
            Levels = new ObservableCollection<Level>(_levelsRepository.GetLevels());
            if (!EditMode)
            {
                Level level = Levels.FirstOrDefault();
                SelectedLevelId = level.LevelID;
                Sections = new ObservableCollection<Section>(level.Sections);
                SelectedSectionId = Sections.FirstOrDefault().SectionID;
            }
            else
            {
                SelectedLevelId = Student.LevelID;
                SelectedSectionId = Student.SectionID;
            } 
        }

        public void Initialize()
        {
            RefreshList();
            PopulateLevelComboBox();
        }
    }
}
