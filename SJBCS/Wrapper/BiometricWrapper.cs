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
        public override void Add(object obj)
        {
            DBContext.Biometrics.Add((Biometric)obj);
            DBContext.SaveChanges();
        }
        public override ObservableCollection<object> RetrieveAll()
        {
            var query = from bio in DBContext.Biometrics select bio;

            return new ObservableCollection<Object>(query.ToList());
        }

        public override ObservableCollection<object> RetrieveViaKey(object obj)
        {
            Student student = (Student)obj;
            var query = from relBiometric in DBContext.RelBiometrics
                        where relBiometric.StudentID == student.StudentID
                        select relBiometric;

            return new ObservableCollection<Object>(query.ToList());
        }
    }
}
