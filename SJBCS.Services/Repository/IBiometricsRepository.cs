using SJBCS.Data;
using System;
using System.Collections.Generic;

namespace SJBCS.Services.Repository
{
    public interface IBiometricsRepository
    {
        List<Biometric> GetBiometrics();
        Biometric GetBiometric(Guid id);
        Biometric AddBiometric(Biometric Biometric);
        Biometric UpdateBiometric(Biometric Biometric);
        void DeleteBiometric(Guid id);
    }
}
