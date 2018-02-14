using SJBCS.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SJBCS.Wrapper
{
    public class SectionWrapper : EntityModel
    {
        public override void Add(object obj)
        {
            Section section = (Section)obj;
            section.SectionID = Guid.NewGuid();
            DBContext.Sections.Add(section);
            DBContext.SaveChanges();

        }

        public override ObservableCollection<object> RetrieveAll()
        {
            var query = DBContext.ListSection();
            return new ObservableCollection<Object>(query.ToList());
        }

        public override ObservableCollection<object> RetrieveViaKey(object obj)
        {
            if (obj is Student)
            {
                Student student = (Student)obj;
                var query = from section in DBContext.Sections
                            where section.SectionID == student.SectionID
                            && section.LevelID == student.LevelID
                            select section;

                return new ObservableCollection<Object>(query.ToList());
            }
           else if(obj is Level)
            {
                Level level = (Level)obj;
                var query = from section in DBContext.Sections
                            where section.LevelID == level.LevelID
                            select section;

                return new ObservableCollection<Object>(query.ToList());
            }
            return null;
        }
    }
}
