using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SJBCS.Model
{
    public class EntityModel
    {
        protected AMSEntities DBContext;
        public EntityModel()
        {
            DBContext = new AMSEntities();
        }
        public virtual void Add(Object obj) { }
        public virtual void Update(Object obj) { }
        public virtual void Delete(Object obj) { }
        public virtual ObservableCollection<Object> RetrieveViaSP(Object obj, String storedProc) { return null; }
        public virtual ObservableCollection<Object> RetrieveAll() { return null; }
        public virtual ObservableCollection<Object> RetrieveViaKey(Object obj) { return null; }
    }
}
