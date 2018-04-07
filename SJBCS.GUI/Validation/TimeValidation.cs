using System;
using System.ComponentModel.DataAnnotations;

namespace SJBCS.GUI.Validation
{
    public class TimeValidation : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            DateTime dateValue;

            if (!DateTime.TryParse(value.ToString(), out dateValue))
                return new ValidationResult(FormatErrorMessage(validationContext.DisplayName));

            return ValidationResult.Success;

        }
    }
}
