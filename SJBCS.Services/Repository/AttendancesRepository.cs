using SJBCS.Data;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace SJBCS.Services.Repository
{
    public class AttendancesRepository : IAttendancesRepository
    {
        AmsModel _context;

        public Attendance AddAttendance(Attendance Attendance)
        {
            using (_context = ConnectionHelper.CreateConnection())
            {
                _context.Attendances.Add(Attendance);
                _context.SaveChanges();

                return Attendance;
            }
        }

        public void DeleteAttendance(Guid id)
        {
            using (_context = ConnectionHelper.CreateConnection())
            {
                var Attendance = _context.Attendances.FirstOrDefault(r => r.AttendanceID == id);
                _context.Entry(Attendance).State = EntityState.Deleted;
                _context.SaveChanges();
            }
        }

        public Attendance GetAttendanceByStudentID(Guid studentID)
        {
            using (_context = ConnectionHelper.CreateConnection())
            {
                var today = DateTime.Today;
                var Attendance = _context.Attendances
                    .Where(attendance => attendance.StudentID == studentID && DbFunctions.TruncateTime(attendance.TimeIn) >= today)
                    .Include(attendance => attendance.Student)
                    .FirstOrDefault();

                return Attendance;
            }
        }

        public Attendance GetAttendanceByID(Guid attendanceID)
        {
            using (_context = ConnectionHelper.CreateConnection())
            {
                var Attendance = _context.Attendances.Where(attendance => attendance.AttendanceID == attendanceID)
                    .Include(attendance => attendance.Student)
                    .FirstOrDefault();

                return Attendance;
            }
        }

        public Attendance GetAttendanceBySMSID(string smsID)
        {
            using (_context = ConnectionHelper.CreateConnection())
            {
                var Attendance = _context.Attendances.Where(attendance => attendance.TimeInSMSID == smsID || attendance.TimeOutSMSID == smsID)
                    .Include(attendance => attendance.Student)
                    .FirstOrDefault();

                return Attendance;
            }
        }

        public List<Attendance> GetAttendances()
        {
            using (_context = ConnectionHelper.CreateConnection())
            {
                var Attendances = _context.Attendances
                    .Include(attendance => attendance.Student)
                    .ToList();

                return Attendances;
            }
        }

        public List<Attendance> GetAttendancesWithFailedSMSRecord()
        {
            using (_context = ConnectionHelper.CreateConnection())
            {
                var today = DateTime.Today;
                var Attendances = _context.Attendances.Where(attendance =>
                        ((attendance.TimeIn != null &&
                            (attendance.TimeInSMSID == null
                            || attendance.TimeInSMSStatus == null))
                         || (attendance.TimeOut != null &&
                            (attendance.TimeOutSMSID == null
                            || attendance.TimeOutSMSStatus == null)))
                        && DbFunctions.TruncateTime(attendance.TimeIn) >= today)
                        .Include(attendance => attendance.Student)
                        .ToList();

                return Attendances;
            }
        }

        public Attendance UpdateAttendance(Attendance Attendance)
        {
            using (_context = ConnectionHelper.CreateConnection())
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
}
