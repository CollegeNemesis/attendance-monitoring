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

        public Student AddStudent(Student Student)
        {
            _context.Students.Add(Student);
            _context.SaveChanges();
            return Student;
        }

        public void DeleteStudent(Guid id)
        {
            var Student = _context.Students.FirstOrDefault(r => r.StudentGuid == id);
            if (Student != null)
            {
                _context.Students.Remove(Student);
            }
            _context.SaveChanges();
        }

        public Student GetStudent(Guid id)
        {
            return _context.Students.FirstOrDefault(r => r.StudentGuid == id);
        }
        public Student GetStudent(string id)
        {
            _context = new AmsDbContext();
            return _context.Students.FirstOrDefault(r => r.StudentID == id);
        }

        public List<Student> GetStudents()
        {
            _context = new AmsDbContext();
            return _context.Students.ToList();
        }

        public Student UpdateStudent(Student source)
        {
            if (!_context.Students.Local.Any(r => r.StudentGuid == source.StudentGuid))
            {
                _context.Students.Attach(source);
            }
            _context.Entry(source).State = EntityState.Modified;
            _context.SaveChanges();
            return source;
        }
    }
}
