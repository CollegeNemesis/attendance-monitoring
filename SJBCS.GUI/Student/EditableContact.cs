using AMS.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using SJBCS.GUI.Validation;
using SJBCS.Data;

namespace SJBCS.GUI.Student
{
    public class EditableContact : ValidatableBindableBase
    {
        private string _contactNumber;
        [PHPhone(ErrorMessage = "This is not a valid Philippine phone number.")]
        public string ContactNumber
        {
            get { return _contactNumber; }
            set
            {

                ValidateContactNumer(value);
                SetProperty(ref _contactNumber, value);

            }
        }

        private void ValidateContactNumer(string value)
        {
            if (value.Length >= 11)
            {
                if (Contacts != null)
                {
                    foreach (Contact contact in contacts)
                    {
                        if (value.Substring(value.Length - 9) == contact.ContactNumber.Substring(contact.ContactNumber.Length - 9))
                        {
                            throw new ArgumentException("Cannot add duplicate number.");
                        }
                    }
                }
            }
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
