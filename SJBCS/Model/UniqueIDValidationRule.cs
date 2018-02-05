using SJBCS.Wrapper;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace SJBCS.Model
{
    class UniqueIDValidationRule : ValidationRule
    {
        private StudentWrapper _studentWrapper;
        private AMSEntities DBContext;
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            DBContext = new AMSEntities();
            _studentWrapper = new StudentWrapper();
            ObservableCollection<Object> result = _studentWrapper.RetrieveViaKeyword(DBContext, value, value.ToString());
            Console.WriteLine(result.FirstOrDefault());
            return !string.IsNullOrWhiteSpace((result.FirstOrDefault() ?? "").ToString())
                ? new ValidationResult(false, "Student ID already existing.")
                : ValidationResult.ValidResult;
        }
    }
}
