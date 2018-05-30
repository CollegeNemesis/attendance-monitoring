using SJBCS.GUI.Student;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;

namespace SJBCS.GUI.Validation
{
    public class ScheduleValidation : ValidationRule
    {
        public override ValidationResult Validate(object value,
                                                 CultureInfo cultureInfo)
        {
            BindingGroup bindingGroup = (BindingGroup)value;
            EditableSection editableSection = (EditableSection)bindingGroup.Items[0];

            if (string.IsNullOrEmpty(editableSection.StartTime) || string.IsNullOrEmpty(editableSection.EndTime))
            {
                return ValidationResult.ValidResult;
            }

            TimeSpan start = DateTime.Parse(editableSection.StartTime).TimeOfDay;
            TimeSpan end = DateTime.Parse(editableSection.EndTime).TimeOfDay;

            if (start >= end)
            {
                return new ValidationResult(false, "Schedule is invalid.");
            }

            return ValidationResult.ValidResult;
        }
    }
}
