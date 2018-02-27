using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SJBCS.Data;

namespace SJBCS.Services.Repository
{
    public interface IStudentsRepository
    {
        Task<List<Student>> GetStudentsAsync();
        Task<Student> GetStudentAsync(string studentId);
        Task<Student> AddStudentAsync(Student user);
        Task<Student> UpdateStudentAsync(Student user);
        Task DeleteStudentAsync(string studentId);
    }
}
