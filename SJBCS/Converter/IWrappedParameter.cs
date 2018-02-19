using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SJBCS.Converter
{
    public interface IWrappedParameter<T>
    {
        T Value { get; }
    }
}
