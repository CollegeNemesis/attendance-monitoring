using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SJBCS.Data;

namespace SJBCS.Services.Repository
{
    public class StudentsRepository : IStudentsRepository
    {
        AmsDbContext _context = new AmsDbContext();

        public async Task<Student> AddStudentAsync(Student Student)
        {
            _context.Students.Add(Student);
            await _context.SaveChangesAsync();
            return Student;
        }

        public async Task DeleteStudentAsync(string studentId)
        {
            var Student = _context.Students.FirstOrDefault(s => s.StudentID == studentId);
            if (Student != null)
            {
                _context.Students.Remove(Student);
            }
            await _context.SaveChangesAsync();
        }

        public Task<Student> GetStudentAsync(string studentId)
        {
            return _context.Students.FirstOrDefaultAsync(s => s.StudentID == studentId);
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
