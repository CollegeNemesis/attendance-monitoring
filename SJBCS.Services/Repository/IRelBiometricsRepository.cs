using SJBCS.Data;
using System;
using System.Collections.Generic;

namespace SJBCS.Services.Repository
{
    public interface IRelBiometricsRepository
    {
        List<RelBiometric> GetRelBiometrics();
        RelBiometric GetRelBiometric(Guid id);
        RelBiometric AddRelBiometric(RelBiometric relBiometric);
        RelBiometric UpdateRelBiometric(RelBiometric relBiometric);
        void DeleteRelBiometric(Guid id);
    }
}
