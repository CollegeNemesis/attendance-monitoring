using SJBCS.Data;

namespace SJBCS.Students
{
    public class StudentInfo
    {
        Student _student;
        Section _section;
        Level _level;
        Contact _contact;
        Organization _group;

        

        public StudentInfo(Student student)
        {
            _student = student;
        }
    }
}
