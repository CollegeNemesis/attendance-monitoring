using SJBCS.Data;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SJBCS.Services
{
    interface IBiometricsRepository
    {
        Task<List<Biometric>> GetBiometricsAsync();
        Task<Biometric> GetBiometricAsync(Guid id);
        Task<Biometric> AddBiometricAsync(Biometric biometric);
        Task<Biometric> UpdateBiometricAsync(Biometric biometric);
        Task DeleteBiometricAsync(Guid biometricId);
    }
}
