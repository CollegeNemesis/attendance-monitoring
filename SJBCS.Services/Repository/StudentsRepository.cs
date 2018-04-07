using SJBCS.Data;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace SJBCS.Services.Repository
{
    public class StudentsRepository : IStudentsRepository
    {
        AmsModel _context = ConnectionHelper.CreateConnection();

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
                foreach (Contact contact in Student.Contacts.ToList())
                {
                    _context.Entry(contact).State = EntityState.Deleted;
                }
                foreach (RelBiometric relBiometric in Student.RelBiometrics.ToList())
                {
                    _context.Entry(relBiometric.Biometric).State = EntityState.Deleted;
                    _context.Entry(relBiometric).State = EntityState.Deleted;
                }
                _context.Entry(Student).State = EntityState.Deleted;
                //_context.Students.Remove(Student);
            }
            _context.SaveChanges();

        }

        public Student GetStudent(Guid id)
        {
            return _context.Students.FirstOrDefault(r => r.StudentGuid == id);
        }
        public Student GetStudent(string id)
        {
            _context = ConnectionHelper.CreateConnection();
            return _context.Students.FirstOrDefault(r => r.StudentID == id);
        }

        public List<Student> GetStudents()
        {
            _context = ConnectionHelper.CreateConnection();
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
