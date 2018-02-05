using SJBCS.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SJBCS.Wrapper
{
    class AttendanceWrapper : EntityModel
    {
        public void Add(AMSEntities dBContext, object obj)
        {
            dBContext.Attendances.Add((Attendance)obj);
            dBContext.SaveChanges();
        }

        public void Delete(AMSEntities dBContext, object obj)
        {
            throw new NotImplementedException();
        }

        public ObservableCollection<object> RetrieveAll(AMSEntities dBContext, object obj)
        {
            throw new NotImplementedException();
        }

        public ObservableCollection<object> RetrieveViaStudent(AMSEntities dBContext, Student student)
        {
            var query = from attendance in dBContext.Attendances
                        where attendance.StudentID == student.StudentID
                        && DbFunctions.TruncateTime(attendance.TimeIn) == DateTime.Today
                        select attendance;

            return new ObservableCollection<Object>(query.ToList());
        }

        public ObservableCollection<object> RetrieveViaKeyword(AMSEntities dBContext, object obj, string keyword)
        {
            throw new NotImplementedException();
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
