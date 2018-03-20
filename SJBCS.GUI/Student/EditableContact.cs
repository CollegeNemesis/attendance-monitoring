using AMS.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using SJBCS.GUI.Validation;

namespace SJBCS.GUI.Student
{
    public class EditableContact : ValidatableBindableBase
    {
        private string _contactNumber;

        [PhoneValidationRule (ErrorMessage ="This is not a valid Philippine phone number.")]
        public string ContactNumber
        {
            get { return _contactNumber; }
            set { SetProperty(ref _contactNumber, value); }
        }
    }
}
