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
        private Section _section;
        
        public void Add(AMSEntities dBContext, object obj)
        {
            _section = obj as Section;
            _section.SectionID = Guid.NewGuid();
            dBContext.Sections.Add(_section);
            dBContext.SaveChanges();

        }

        public void Delete(AMSEntities dBContext, object obj)
        {
            throw new NotImplementedException();
        }

        public ObservableCollection<object> RetrieveAll(AMSEntities dBContext, object obj)
        {
            var query = dBContext.ListSection();
            return new ObservableCollection<Object>(query.ToList());
        }

        public ObservableCollection<object> RetrieveViaKeyword(AMSEntities dBContext, object obj, string keyword)
        {
            throw new NotImplementedException();
        }

        public ObservableCollection<object> RetrieveViaSP(AMSEntities dBContext, object obj, string sp, List<string> param)
        {
            throw new NotImplementedException();
        }

        public void Update(AMSEntities dBContext, object obj)
        {
            throw new NotImplementedException();
        }
    }
}
