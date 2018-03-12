using SJBCS.Services.Repository;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace SJBCS.GUI.Validation
{
    public class UniqueStudentIdValidationRule : ValidationRule
    {
        private IStudentsRepository _studentsRepository;

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            _studentsRepository = new StudentsRepository();

            Data.Student student = _studentsRepository.GetStudent(value.ToString());
            if (student != null)
                return new ValidationResult(false, "Student ID already existing.");
            else
                return ValidationResult.ValidResult;
        }
    }
}
