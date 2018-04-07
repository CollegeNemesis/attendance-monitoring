using SJBCS.GUI.Utilities;
using System.ComponentModel.DataAnnotations;

namespace SJBCS.GUI.Settings
{
    public class EditableSmsConfig : ValidatableBindableBase
    {
        private string url;
        [Required(ErrorMessage = "This field is required.")]
        [Url(ErrorMessage = "Invalid URL format.")]
        public string Url
        {
            get { return url; }
            set { SetProperty(ref url, value); }
        }
    }
}
