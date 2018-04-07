using ExpressiveAnnotations.Attributes;
using SJBCS.Data;
using SJBCS.GUI.Utilities;
using SJBCS.GUI.Validation;
using System.Collections.ObjectModel;

namespace SJBCS.GUI.Student
{
    public class EditableContact : ValidatableBindableBase
    {
        private string _contactNumber;
        [PHPhone(ErrorMessage = "This is not a valid Philippine phone number.")]
        [AssertThat("IsDuplicateContactNumber(ContactNumber)", ErrorMessage = "Contact number is already existing.")]
        public string ContactNumber
        {
            get { return _contactNumber; }
            set
            {
                SetProperty(ref _contactNumber, value);
            }
        }

        public bool IsDuplicateContactNumber(string contactNumber)
        {
            if (contactNumber.Length >= 11)
            {
                if (Contacts != null)
                {
                    foreach (Contact contact in contacts)
                    {
                        if (contactNumber.Substring(contactNumber.Length - 9) == contact.ContactNumber.Substring(contact.ContactNumber.Length - 9))
                            return false;
                    }
                }
            }
            return true;
        }

        private ObservableCollection<Contact> contacts;

        public ObservableCollection<Contact> Contacts
        {
            get { return contacts; }
            set
            {
                SetProperty(ref contacts, value);
            }
        }

    }
}
