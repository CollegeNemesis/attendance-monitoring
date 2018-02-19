using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using SJBCS.Data;

namespace SJBCS.Services
{
    public class StudentsRepository : IStudentsRepository
    {
        AmsDbContext _context = new AmsDbContext();

        public async Task<Student> AddStudentAsync(Student student)
        {
            _context.Students.Add(student);
            await _context.SaveChangesAsync();
            return student;
        }

        public async Task DeleteStudentAsync(String studentNum)
        {
            var student = _context.Students.FirstOrDefault(s => s.StudentNum == studentNum);
            if (student != null)
            {
                _context.Students.Remove(student);
            }
            await _context.SaveChangesAsync();
        }

        public Task<Student> GetStudentAsync(String id)
        {
            return _context.Students.FirstOrDefaultAsync(s => s.StudentNum == id);
        }

        public Task<List<Student>> GetStudentsAsync()
        {
            return _context.Students.ToListAsync();
        }

        public async Task<Student> UpdateStudentAsync(Student student)
        {
            if (!_context.Students.Local.Any(s => s.StudentID == student.StudentID))
            {
                _context.Students.Attach(student);
            }
            _context.Entry(student).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return student;
        }
    }
}
