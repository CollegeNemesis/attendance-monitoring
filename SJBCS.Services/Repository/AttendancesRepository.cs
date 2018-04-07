using SJBCS.Data;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace SJBCS.Services.Repository
{
    public class AttendancesRepository : IAttendancesRepository
    {
        AmsModel _context = ConnectionHelper.CreateConnection();

        public Attendance AddAttendance(Attendance Attendance)
        {
            //_context = ConnectionHelper.CreateConnection();
            _context.Attendances.Add(Attendance);
            _context.SaveChanges();
            return Attendance;
        }

        public void DeleteAttendance(Guid id)
        {
            _context = ConnectionHelper.CreateConnection();
            var Attendance = _context.Attendances.FirstOrDefault(r => r.AttendanceID == id);
            if (Attendance != null)
            {
                _context.Attendances.Remove(Attendance);
            }
            _context.SaveChanges();
        }

        public Attendance GetAttendanceByStudentID(Guid studentID)
        {
            _context = ConnectionHelper.CreateConnection();
            var today = DateTime.Today;

            return _context.Attendances.Where(attendance => attendance.StudentID == studentID && DbFunctions.TruncateTime(attendance.TimeIn) >= today).FirstOrDefault();
        }

        public Attendance GetAttendanceByID(Guid attendanceID)
        {
            _context = ConnectionHelper.CreateConnection();
            return _context.Attendances.Where(attendance => attendance.AttendanceID == attendanceID).FirstOrDefault();
        }

        public Attendance GetAttendanceBySMSID(string smsID)
        {
            _context = ConnectionHelper.CreateConnection();
            return _context.Attendances.Where(attendance => attendance.TimeInSMSID == smsID || attendance.TimeOutSMSID == smsID).FirstOrDefault();
        }

        public List<Attendance> GetAttendances()
        {
            _context = ConnectionHelper.CreateConnection();
            return _context.Attendances.ToList();
        }

        public List<Attendance> GetAttendancesWithFailedSMSRecord()
        {
            _context = ConnectionHelper.CreateConnection();
            var today = DateTime.Today;
            return _context.Attendances.Where(attendance =>
                        ((attendance.TimeIn != null &&
                            (attendance.TimeInSMSID == null
                            || attendance.TimeInSMSStatus == null))
                         || (attendance.TimeOut != null &&
                            (attendance.TimeOutSMSID == null
                            || attendance.TimeOutSMSStatus == null)))
                        && DbFunctions.TruncateTime(attendance.TimeIn) >= today).ToList();
        }

        public Attendance UpdateAttendance(Attendance Attendance)
        {
            //_context = ConnectionHelper.CreateConnection();
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
