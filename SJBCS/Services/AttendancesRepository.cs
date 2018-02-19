using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SJBCS.Data;

namespace SJBCS.Services
{
    public class AttendancesRepository : IAttendancesRepository
    {
        AmsDbContext _context = new AmsDbContext();

        public async Task<Attendance> AddAttendanceAsync(Attendance attendance)
        {
            _context.Attendances.Add(attendance);
            await _context.SaveChangesAsync();
            return attendance;
        }

        public async Task DeleteAttendanceAsync(Guid attendanceId)
        {
            var attendance = _context.Attendances.FirstOrDefault(a => a.AttendanceID == attendanceId);
            if (attendance != null)
            {
                _context.Attendances.Remove(attendance);
            }
            await _context.SaveChangesAsync();
        }

        public Task<Attendance> GetAttendanceAsync(Guid id)
        {
            return _context.Attendances.FirstOrDefaultAsync(a => a.AttendanceID == id);
        }

        public Task<List<Attendance>> GetAttendancesAsync()
        {
            return _context.Attendances.ToListAsync();
        }

        public async Task<Attendance> UpdateAttendanceAsync(Attendance attendance)
        {
            if (!_context.Attendances.Local.Any(a => a.AttendanceID == attendance.AttendanceID))
            {
                _context.Attendances.Attach(attendance);
            }
            _context.Entry(attendance).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return attendance;
        }
    }
}
