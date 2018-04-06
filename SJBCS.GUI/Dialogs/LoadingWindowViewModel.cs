using SJBCS.GUI.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SJBCS.GUI.Dialogs
{
    public class LoadingWindowViewModel : BindableBase
    {
        private bool closeTrigger;
        public bool CloseTrigger
        {
            get { return this.closeTrigger; }
            set
            {
                SetProperty(ref closeTrigger, value);
            }
        }
    }
}
