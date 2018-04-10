using SJBCS.Data;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace SJBCS.Services.Repository
{
    public class RelBiometricsRepository : IRelBiometricsRepository
    {
        AmsModel _context;

        public RelBiometric AddRelBiometric(RelBiometric relBiometric)
        {
            using (_context = ConnectionHelper.CreateConnection())
            {
                _context.RelBiometrics.Add(relBiometric);
                _context.SaveChanges();

                return relBiometric;
            }
        }

        public void DeleteRelBiometric(Guid id)
        {
            using (_context = ConnectionHelper.CreateConnection())
            {
                var RelBiometric = _context.RelBiometrics.FirstOrDefault(r => r.FingerID == id);
                _context.Entry(RelBiometric).State = EntityState.Deleted;
                _context.SaveChanges();
            }
        }

        public RelBiometric GetRelBiometric(Guid id)
        {
            using (_context = ConnectionHelper.CreateConnection())
            {
                var RelBiometric = _context.RelBiometrics
                    .Include(relBiometric => relBiometric.Student)
                    .Include(relBiometric => relBiometric.Biometric)
                    .FirstOrDefault(r => r.FingerID == id);

                return RelBiometric;
            }
        }

        public List<RelBiometric> GetRelBiometrics()
        {
            using (_context = ConnectionHelper.CreateConnection())
            {
                var RelBiometrics = _context.RelBiometrics
                    .Include(relBiometric => relBiometric.Student)
                    .Include(relBiometric => relBiometric.Biometric)
                    .ToList();

                return RelBiometrics;
            }
        }

        public RelBiometric UpdateRelBiometric(RelBiometric RelBiometric)
        {
            using (_context = ConnectionHelper.CreateConnection())
            {
                if (!_context.RelBiometrics.Local.Any(r => r.RelBiometricID == RelBiometric.RelBiometricID))
                {
                    _context.RelBiometrics.Attach(RelBiometric);
                }
                _context.Entry(RelBiometric).State = EntityState.Modified;
                _context.SaveChanges();

                return RelBiometric;
            }
        }
    }
}
