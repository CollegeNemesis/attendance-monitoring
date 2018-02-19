using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SJBCS.Data;

namespace SJBCS.Services
{
    public class BiometricsRepository : IBiometricsRepository
    {
        AmsDbContext _context = new AmsDbContext();

        public async Task<Biometric> AddBiometricAsync(Biometric biometric)
        {
            _context.Biometrics.Add(biometric);
            await _context.SaveChangesAsync();
            return biometric;
        }

        public async Task DeleteBiometricAsync(Guid biometricId)
        {
            var biometric = _context.Biometrics.FirstOrDefault(b => b.FingerID == biometricId);
            if (biometric != null)
            {
                _context.Biometrics.Remove(biometric);
            }
            await _context.SaveChangesAsync();
        }

        public Task<Biometric> GetBiometricAsync(Guid id)
        {
            return _context.Biometrics.FirstOrDefaultAsync(b => b.FingerID == id);
        }

        public Task<List<Biometric>> GetBiometricsAsync()
        {
            return _context.Biometrics.ToListAsync();
        }

        public async Task<Biometric> UpdateBiometricAsync(Biometric biometric)
        {
            if (!_context.Biometrics.Local.Any(b => b.FingerID == biometric.FingerID))
            {
                _context.Biometrics.Attach(biometric);
            }
            _context.Entry(biometric).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return biometric;
        }
    }
}
