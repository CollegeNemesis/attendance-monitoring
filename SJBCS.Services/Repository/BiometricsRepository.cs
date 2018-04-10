using SJBCS.Data;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace SJBCS.Services.Repository
{
    public class BiometricsRepository : IBiometricsRepository
    {
        AmsModel _context;

        public Biometric AddBiometric(Biometric Biometric)
        {
            using (_context = ConnectionHelper.CreateConnection())
            {
                _context.Biometrics.Add(Biometric);
                _context.SaveChanges();

                return Biometric;
            }
        }

        public void DeleteBiometric(Guid id)
        {
            using (_context = ConnectionHelper.CreateConnection())
            {
                var Biometric = _context.Biometrics.FirstOrDefault(r => r.FingerID == id);
                _context.Entry(Biometric).State = EntityState.Deleted;
                _context.SaveChanges();
            }
        }

        public Biometric GetBiometric(Guid id)
        {
            using (_context = ConnectionHelper.CreateConnection())
            {
                var Biometric = _context.Biometrics
                    .Include(biometric => biometric.RelBiometrics)
                    .FirstOrDefault(r => r.FingerID == id);
                return Biometric;
            }
        }

        public List<Biometric> GetBiometrics()
        {
            using (_context = ConnectionHelper.CreateConnection())
            {
                var Biometrics = _context.Biometrics
                    .Include(biometric => biometric.RelBiometrics)
                    .ToList();
                return Biometrics;
            }
        }

        public Biometric UpdateBiometric(Biometric Biometric)
        {
            using (_context = ConnectionHelper.CreateConnection())
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
}
