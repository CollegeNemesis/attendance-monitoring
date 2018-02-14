using SJBCS.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SJBCS.Wrapper
{
    class RelBiometricWrapper : EntityModel
    {
        public override void Add(object obj)
        {
            DBContext.RelBiometrics.Add((RelBiometric)obj);
            DBContext.SaveChanges();
        }

        public override ObservableCollection<object> RetrieveViaKey(object obj)
        {
            if(obj is Biometric)
            {
                Biometric biometric = (Biometric)obj;
                var query = from relBiometric in DBContext.RelBiometrics
                            where relBiometric.FingerID == biometric.FingerID
                            select relBiometric;

                return new ObservableCollection<Object>(query.ToList());
            }
            else if (obj is Student)
            {
                Student student = (Student)obj;
                var query = from relBiometric in DBContext.RelBiometrics
                            where relBiometric.StudentID == student.StudentID
                            select relBiometric;

                return new ObservableCollection<Object>(query.ToList());
            }
            return null;
        }
    }
}
