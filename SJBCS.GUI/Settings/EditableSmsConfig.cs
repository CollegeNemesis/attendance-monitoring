using AMS.Utilities;
using System.ComponentModel.DataAnnotations;

namespace SJBCS.GUI.Settings
{
    public class EditableSmsConfig : ValidatableBindableBase
    {
        private string url;
        [Required]
        public string Url
        {
            get { return url; }
            set { SetProperty(ref url, value); }
        }
    }
}
