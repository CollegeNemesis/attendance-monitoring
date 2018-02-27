using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace SJBCS.Converter
{
    public class PasswordWrapper : IWrappedParameter<string>
    {
        private readonly PasswordBox _source;

        public string Value
        {
            get { return _source != null ? _source.Password : string.Empty; }
        }

        public PasswordWrapper(PasswordBox source)
        {
            _source = source;
        }
    }
}
