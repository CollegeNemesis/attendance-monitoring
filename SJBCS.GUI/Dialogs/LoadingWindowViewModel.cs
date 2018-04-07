using SJBCS.GUI.Utilities;

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
