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
        public override void Add(object obj)
        {
            DBContext.Attendances.Add((Attendance)obj);
            DBContext.SaveChanges();
        }

        public override void Delete(object obj)
        {
            throw new NotImplementedException();
        }

        public override void Update(object obj)
        {
            Attendance attendance = (Attendance)obj;
            DBContext.SaveChanges();
        }

        public override ObservableCollection<object> RetrieveViaKey(object obj)
        {
            Student student = (Student)obj;
            var query = from attendance in DBContext.Attendances
                        where attendance.StudentID == student.StudentID
                        && DbFunctions.TruncateTime(attendance.TimeIn) == DateTime.Today
                        select attendance;

            return new ObservableCollection<Object>(query.ToList());
        }
    }
}
