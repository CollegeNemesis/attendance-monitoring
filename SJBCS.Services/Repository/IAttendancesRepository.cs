using SJBCS.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SJBCS.Services.Repository
{
    public interface IAttendancesRepository
    {
        List<Attendance> GetAttendances();
        Attendance GetAttendanceByStudentID(Guid id);
        Attendance GetAttendanceByID(Guid id);
        Attendance AddAttendance(Attendance Attendance);
        Attendance UpdateAttendance(Attendance Attendance);
        void DeleteAttendance(Guid id);
    }
}
