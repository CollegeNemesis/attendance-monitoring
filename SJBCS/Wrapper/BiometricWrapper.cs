using SJBCS.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;

namespace SJBCS.Wrapper
{
    class BiometricWrapper : EntityModel
    {
        public void Add(AMSEntities dBContext, object obj)
        {
            dBContext.Biometrics.Add((Biometric)obj);
            dBContext.SaveChanges();
        }

        public void Delete(AMSEntities dBContext, object obj)
        {
            throw new NotImplementedException();
        }

        public ObservableCollection<object> RetrieveAll(AMSEntities dBContext, object obj)
        {
            var query = from bio in dBContext.Biometrics select bio;
            return new ObservableCollection<Object>(query.ToList());
        }

        public ObservableCollection<object> RetrieveViaKeyword(AMSEntities dBContext, object obj, string keyword)
        {
            var query = from relBiometric in dBContext.RelBiometrics
                        where relBiometric.StudentID == keyword
                        select relBiometric;

            return new ObservableCollection<Object>(query.ToList());
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
