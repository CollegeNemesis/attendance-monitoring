using SJBCS.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SJBCS.Wrapper
{
    class LevelWrapper : EntityModel
    {
        public override ObservableCollection<object> RetrieveAll()
        {
            var query = from level in DBContext.Levels select level;

            return new ObservableCollection<Object>(query.ToList());
        }

        public override ObservableCollection<object> RetrieveViaKey(object obj)
        {
            if (obj is Student)
            {
                Student student = (Student)obj;
                var query = from level in DBContext.Levels
                            where level.LevelID == student.LevelID
                            select level;

                return new ObservableCollection<Object>(query.ToList());
            }
            else if (obj is Section)
            {
                Section section = (Section)obj;
                var query = from level in DBContext.Levels
                            where level.LevelID == section.LevelID
                            select level;

                return new ObservableCollection<Object>(query.ToList());
            }

            return null;
        }
    }
}
