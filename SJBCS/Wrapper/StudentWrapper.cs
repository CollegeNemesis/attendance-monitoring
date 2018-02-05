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
        public void Add(AMSEntities dBContext, object obj)
        {
            Student student = (Student)obj;
            dBContext.Students.Add(student);
            Console.WriteLine(student.StudentID);
            Console.WriteLine(student.LastName);
            Console.WriteLine(student.FirstName);
            Console.WriteLine(student.LevelID);
            Console.WriteLine(student.SectionID);
            dBContext.SaveChanges();
        }

        public void Delete(AMSEntities dBContext, object obj)
        {
            throw new NotImplementedException();
        }

        public ObservableCollection<object> RetrieveAll(AMSEntities dBContext, object obj)
        {
            var query = dBContext.ListStudent();
            return new ObservableCollection<object>(query.ToList());
        }

        public ObservableCollection<object> RetrieveViaKeyword(AMSEntities dBContext, object obj, string keyword)
        {
            var query = from student in dBContext.Students
                        where student.StudentID == keyword
                        select student;

            return new ObservableCollection<Object>(query.ToList());
        }

        public ObservableCollection<object> RetrieveViaSP(AMSEntities dBContext, object obj, string sp, List<string> param)
        {
            throw new NotImplementedException();
        }

        public void Update(AMSEntities dBContext, object obj)
        {
            throw new NotImplementedException();
        }
    }
}
