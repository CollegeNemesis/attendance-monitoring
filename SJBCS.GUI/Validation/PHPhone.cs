using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace SJBCS.GUI.Validation
{
    public class PHPhone : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value.ToString() == string.Empty || value.ToString() == null)
                return ValidationResult.Success;
            if (!Regex.Match(value.ToString(), @"^(09|\+639)\d{9}$").Success)
                return new ValidationResult(FormatErrorMessage(validationContext.DisplayName));
            else
                return ValidationResult.Success;

        }
    }
}
