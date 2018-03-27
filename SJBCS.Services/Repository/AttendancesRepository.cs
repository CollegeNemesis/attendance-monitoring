using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SJBCS.Data;

namespace SJBCS.Services.Repository
{
    public class AttendancesRepository : IAttendancesRepository
    {
        AmsDbContext _context = new AmsDbContext();

        public Attendance AddAttendance(Attendance Attendance)
        {
            _context.Attendances.Add(Attendance);
            _context.SaveChanges();
            return Attendance;
        }

        public void DeleteAttendance(Guid id)
        {
            var Attendance = _context.Attendances.FirstOrDefault(r => r.AttendanceID == id);
            if (Attendance != null)
            {
                _context.Attendances.Remove(Attendance);
            }
            _context.SaveChanges();
        }

        public Attendance GetAttendance(Guid id)
        {
            var today = DateTime.Today;

            return _context.Attendances.Where(a => a.StudentID == id && DbFunctions.TruncateTime(a.TimeIn) >= today).FirstOrDefault();
        }

        public List<Attendance> GetAttendances()
        {
            return _context.Attendances.ToList();
        }

        public Attendance UpdateAttendance(Attendance Attendance)
        {
            if (!_context.Attendances.Local.Any(r => r.AttendanceID == Attendance.AttendanceID))
            {
                _context.Attendances.Attach(Attendance);
            }
            _context.Entry(Attendance).State = EntityState.Modified;
            _context.SaveChanges();
            return Attendance;
        }
    }
}
