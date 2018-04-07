using SJBCS.Data;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace SJBCS.Services.Repository
{
    public class BiometricsRepository : IBiometricsRepository
    {
        AmsModel _context = ConnectionHelper.CreateConnection();

        public Biometric AddBiometric(Biometric Biometric)
        {
            _context.Biometrics.Add(Biometric);
            _context.SaveChanges();
            return Biometric;
        }

        public void DeleteBiometric(Guid id)
        {
            var Biometric = _context.Biometrics.FirstOrDefault(r => r.FingerID == id);
            if (Biometric != null)
            {
                _context.Biometrics.Remove(Biometric);
            }
            _context.SaveChanges();
        }

        public Biometric GetBiometric(Guid id)
        {
            return _context.Biometrics.FirstOrDefault(r => r.FingerID == id);
        }

        public List<Biometric> GetBiometrics()
        {
            return _context.Biometrics.ToList();
        }

        public Biometric UpdateBiometric(Biometric Biometric)
        {
            if (!_context.Biometrics.Local.Any(r => r.FingerID == Biometric.FingerID))
            {
                _context.Biometrics.Attach(Biometric);
            }
            _context.Entry(Biometric).State = EntityState.Modified;
            _context.SaveChanges();
            return Biometric;
        }
    }
}
