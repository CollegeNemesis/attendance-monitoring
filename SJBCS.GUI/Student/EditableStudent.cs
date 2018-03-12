using AMS.Utilities;
using SJBCS.Data;
using SJBCS.Services.Repository;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SJBCS.GUI.Student
{
    public class EditableStudent : ValidatableBindableBase
    {
        private System.Guid studentGuid;
        public System.Guid StudentGuid
        {
            get { return studentGuid; }
            set { SetProperty(ref studentGuid, value); }
        }
        private string firstName;
        [Required]
        public string FirstName
        {
            get { return firstName; }
            set { SetProperty(ref firstName, value); }
        }
        private string middleName;
        public string MiddleName
        {
            get { return middleName; }
            set { SetProperty(ref middleName, value); }
        }
        private string lastName;
        [Required]
        public string LastName
        {
            get { return lastName; }
            set { SetProperty(ref lastName, value); }
        }
        private Nullable<System.DateTime> birthDate;
        public Nullable<System.DateTime> BirthDate
        {
            get { return birthDate; }
            set { SetProperty(ref birthDate, value); }
        }
        private string imageData;
        public string ImageData
        {
            get { return imageData; }
            set { SetProperty(ref imageData, value); }
        }
        private string gender;
        public string Gender
        {
            get { return gender; }
            set { SetProperty(ref gender, value); }
        }
        private string street;
        public string Street
        {
            get { return street; }
            set { SetProperty(ref street, value); }
        }
        private string city;
        public string City
        {
            get { return city; }
            set { SetProperty(ref city, value); }
        }
        private string state;
        public string State
        {
            get { return state; }
            set { SetProperty(ref state, value); }
        }
        private System.Guid sectionID;
        public System.Guid SectionID
        {
            get { return sectionID; }
            set { SetProperty(ref sectionID, value); }
        }
        private System.Guid levelID;
        public System.Guid LevelID
        {
            get { return levelID; }
            set { SetProperty(ref levelID, value); }
        }
        private string studentID;
        public string StudentID
        {
            get { return studentID; }
            set { SetProperty(ref studentID, value); }
        }

        private ICollection<Attendance> attendances;
        public ICollection<Attendance> Attendances
        {
            get { return attendances; }
            set { SetProperty(ref attendances, value); }
        }

        private ObservableCollection<Contact> contacts;
        [Required]
        public ObservableCollection<Contact> Contacts
        {
            get { return contacts; }
            set { SetProperty(ref contacts, value); }
        }
        private Level level;
        public Level Level
        {
            get { return level; }
            set { SetProperty(ref level, value); }
        }
        private ICollection<RelBiometric> relBiometrics;
        public ICollection<RelBiometric> RelBiometrics
        {
            get { return relBiometrics; }
            set { SetProperty(ref relBiometrics, value); }
        }
        private ICollection<RelDistributionList> relDistributionLists;
        public ICollection<RelDistributionList> RelDistributionLists
        {
            get { return relDistributionLists; }
            set { SetProperty(ref relDistributionLists, value); }
        }
        private ObservableCollection<RelOrganization> relOrganizations;
        public ObservableCollection<RelOrganization> RelOrganizations
        {
            get { return relOrganizations; }
            set { SetProperty(ref relOrganizations, value); }
        }
        private Section section;
        public Section Section
        {
            get { return section; }
            set { SetProperty(ref section, value); }
        }
    }
}
