using SJBCS.Data;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace SJBCS.Services.Repository
{
    public class StudentsRepository : IStudentsRepository
    {
        AmsModel _context;

        public Student AddStudent(Student Student)
        {
            using (_context = ConnectionHelper.CreateConnection())
            {
                _context.Students.Add(Student);
                _context.SaveChanges();
            }
            return Student;
        }

        public void DeleteStudent(Guid id)
        {
            using (_context = ConnectionHelper.CreateConnection())
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
                }
                _context.SaveChanges();
            }
        }

        public Student GetStudent(Guid id)
        {
            using (_context = ConnectionHelper.CreateConnection())
            {
                var Student = _context.Students
                    .Include(student => student.Attendances)
                    .Include(student => student.Contacts)
                    .Include(student => student.RelBiometrics.Select(relBiometric => relBiometric.Biometric))
                    .Include(student => student.Level)
                    .Include(student => student.Section)
                    .Include(student => student.RelDistributionLists)
                    .Include(student => student.RelOrganizations)
                    .FirstOrDefault(r => r.StudentGuid == id);

                return Student;
            }
        }
        public Student GetStudent(string id)
        {
            using (_context = ConnectionHelper.CreateConnection())
            {
                var Student = _context.Students
                    .Include(student => student.Attendances)
                    .Include(student => student.Contacts)
                    .Include(student => student.RelBiometrics.Select(relBiometric => relBiometric.Biometric))
                    .Include(student => student.Level)
                    .Include(student => student.Section)
                    .Include(student => student.RelDistributionLists)
                    .Include(student => student.RelOrganizations)
                    .FirstOrDefault(r => r.StudentID == id);

                return Student;
            }
        }

        public List<Student> GetStudents()
        {
            using (_context = ConnectionHelper.CreateConnection())
            {
                var Students = _context.Students
                    .Include(student => student.Attendances)
                    .Include(student => student.Contacts)
                    .Include(student => student.RelBiometrics.Select(relBiometric => relBiometric.Biometric))
                    .Include(student => student.Level)
                    .Include(student => student.Section)
                    .Include(student => student.RelDistributionLists)
                    .Include(student => student.RelOrganizations)
                    .OrderBy(student => new { student.Level.LevelOrder, student.Section.SectionName, student.LastName, student.FirstName })
                    .ToList();

                return Students;
            }
        }

        public Student UpdateStudent(Student Student)
        {
            using (_context = ConnectionHelper.CreateConnection())
            {
                if (!_context.Students.Local.Any(r => r.StudentGuid == Student.StudentGuid))
                {
                    _context.Students.Attach(Student);
                }
                _context.Entry(Student).State = EntityState.Modified;
                _context.SaveChanges();

                return Student;
            }
        }
    }
}
