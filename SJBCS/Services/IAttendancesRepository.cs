using SJBCS.Data;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SJBCS.Services
{
    interface IAttendancesRepository
    {
        Task<List<Attendance>> GetAttendancesAsync();
        Task<Attendance> GetAttendanceAsync(Guid id);
        Task<Attendance> AddAttendanceAsync(Attendance attendance);
        Task<Attendance> UpdateAttendanceAsync(Attendance attendance);
        Task DeleteAttendanceAsync(Guid attendanceId);
    }
}
