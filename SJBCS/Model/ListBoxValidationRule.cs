using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace SJBCS.Model
{
    public class ListBoxValidationRule : ValidationRule
    {
        // In this case, we have a very simple validation.  If there is text in the Validation property,
        // we consider the validation failed.  If the Validation property is empty, then it succeeds.
        // The text of the property is set when we call the Validate() function in the Window class, 
        // which uses its own logic to decide whether to put an error message in the Validation property or not.
        public override ValidationResult Validate(object value, System.Globalization.CultureInfo cultureInfo)
        {
            string s = value as String;

            if (String.IsNullOrEmpty(s))
            {
                return ValidationResult.ValidResult;
            }
            else
            {
                return new ValidationResult(false, value as string);
            }
        }
    }
}
