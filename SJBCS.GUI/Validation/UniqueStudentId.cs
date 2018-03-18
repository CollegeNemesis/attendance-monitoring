using SJBCS.Services.Repository;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SJBCS.GUI.Validation
{
    public class UniqueStudentId : ValidationAttribute
    {

        IStudentsRepository studentsRepository;

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            studentsRepository = new StudentsRepository();
            var contains = studentsRepository.GetStudent(value.ToString());

            if (contains != null)
                return new ValidationResult(FormatErrorMessage(validationContext.DisplayName));
            else
                return ValidationResult.Success;

        }
    }
}
