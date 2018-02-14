using SJBCS.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SJBCS.Wrapper
{
    public class StudentWrapper : EntityModel
    {
        public override void Add(object obj)
        {
            Student student = (Student)obj;
            DBContext.Students.Add(student);
            DBContext.SaveChanges();
        }

        public override ObservableCollection<object> RetrieveViaKey(object obj)
        {
            if (obj is RelBiometric)
            {
                RelBiometric relBiometric = (RelBiometric)obj;
                var query = from student in DBContext.Students
                            where student.StudentID == relBiometric.StudentID
                            select student;

                return new ObservableCollection<Object>(query.ToList());

            }

            return null;
        }

        public override ObservableCollection<object> RetrieveViaSP(object obj, string storedProc)
        {
            var query = DBContext.ListStudent();
            return new ObservableCollection<object>(query.ToList());
        }
    }
}
