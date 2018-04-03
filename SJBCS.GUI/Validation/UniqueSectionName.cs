using SJBCS.Data;
using SJBCS.Services.Repository;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SJBCS.GUI.Validation
{
    public class UniqueSectionName : ValidationAttribute
    {
        ISectionsRepository sectionsRepository = new SectionsRepository();

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            List<Section> Sections = sectionsRepository.GetSections();

            foreach (Section section in Sections)
            {
                if (value.ToString().ToUpper().Trim().Equals(section.SectionName.ToUpper().Trim()))
                    return new ValidationResult(FormatErrorMessage(validationContext.DisplayName));
            }

            return ValidationResult.Success;

        }
    }
}
