using SJBCS.Data;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace SJBCS.Services.Repository
{
    public class RelBiometricsRepository : IRelBiometricsRepository
    {
        AmsModel _context = ConnectionHelper.CreateConnection();

        public RelBiometric AddRelBiometric(RelBiometric relBiometric)
        {
            //_context = ConnectionHelper.CreateConnection();
            _context.RelBiometrics.Add(relBiometric);
            _context.SaveChanges();
            return relBiometric;
        }

        public  void DeleteRelBiometric(Guid id)
        {
            _context = ConnectionHelper.CreateConnection();
            var relBiometric = _context.RelBiometrics.FirstOrDefault(r => r.FingerID == id);
            if (relBiometric != null)
            {
                _context.RelBiometrics.Remove(relBiometric);
            }
            _context.SaveChanges();
        }

        public  RelBiometric GetRelBiometric(Guid id)
        {
            _context = ConnectionHelper.CreateConnection();
            return _context.RelBiometrics.FirstOrDefault(r => r.FingerID == id);
        }

        public  List<RelBiometric> GetRelBiometrics()
        {
            _context = ConnectionHelper.CreateConnection();
            return _context.RelBiometrics.ToList();
        }

        public  RelBiometric UpdateRelBiometric(RelBiometric relBiometric)
        {
           //_context = ConnectionHelper.CreateConnection();
            if (!_context.RelBiometrics.Local.Any(r => r.RelBiometricID == relBiometric.RelBiometricID))
            {
                _context.RelBiometrics.Attach(relBiometric);
            }
            _context.Entry(relBiometric).State = EntityState.Modified;
            _context.SaveChanges();
            return relBiometric;
        }
    }
}
