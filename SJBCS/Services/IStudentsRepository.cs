using SJBCS.Data;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace SJBCS.Services
{
    interface IStudentsRepository
    {
        Task<List<Student>> GetStudentsAsync();
        Task<Student> GetStudentAsync(String id);
        Task<Student> AddStudentAsync(Student student);
        Task<Student> UpdateStudentAsync(Student student);
        Task DeleteStudentAsync(String studentNum);
    }
}
