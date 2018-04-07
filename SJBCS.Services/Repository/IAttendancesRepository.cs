using SJBCS.Data;
using System;
using System.Collections.Generic;

namespace SJBCS.Services.Repository
{
    public interface IAttendancesRepository
    {
        List<Attendance> GetAttendances();
        List<Attendance> GetAttendancesWithFailedSMSRecord();
        Attendance GetAttendanceByStudentID(Guid id);
        Attendance GetAttendanceByID(Guid id);
        Attendance GetAttendanceBySMSID(string smsID);
        Attendance AddAttendance(Attendance Attendance);
        Attendance UpdateAttendance(Attendance Attendance);
        void DeleteAttendance(Guid id);
    }
}
