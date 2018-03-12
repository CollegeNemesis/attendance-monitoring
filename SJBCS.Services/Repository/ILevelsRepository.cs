using SJBCS.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SJBCS.Services.Repository
{
    public interface ILevelsRepository
    {
        List<Level> GetLevels();
        Level GetLevel(Guid id);
        Level AddLevel(Level Level);
        Level UpdateLevel(Level Level);
        void DeleteLevel(Guid id);
    }
}
