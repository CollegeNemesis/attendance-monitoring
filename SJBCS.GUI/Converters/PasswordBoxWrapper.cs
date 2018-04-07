using System.Windows.Controls;

namespace SJBCS.GUI.Converters
{
    public class PasswordBoxWrapper : IWrappedParameter<string>
    {
        private readonly PasswordBox _source;

        public string Value
        {
            get { return _source != null ? _source.Password : string.Empty; }
        }

        public PasswordBoxWrapper(PasswordBox source)
        {
            _source = source;
        }
    }
}