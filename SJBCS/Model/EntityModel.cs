using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SJBCS.Model
{
    interface EntityModel
    {
        void Add(AMSEntities dBContext, Object obj);
        void Update(AMSEntities dBContext, Object obj);
        void Delete(AMSEntities dBContext, Object obj);
        ObservableCollection<Object> RetrieveViaSP(AMSEntities dBContext, Object obj,String sp, List<string> param);
        ObservableCollection<Object> RetrieveAll(AMSEntities dBContext, Object obj);
        ObservableCollection<Object> RetrieveViaKeyword(AMSEntities dBContext, Object obj,string keyword);
    }
}
