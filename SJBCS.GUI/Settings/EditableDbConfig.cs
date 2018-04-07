using SJBCS.GUI.Utilities;
using System.ComponentModel.DataAnnotations;

namespace SJBCS.GUI.Settings
{
    public class EditableDbConfig : ValidatableBindableBase
    {
        private string hostname;
        [Required(ErrorMessage = "This field is required.")]
        public string Hostname
        {
            get { return hostname; }
            set { SetProperty(ref hostname, value); }
        }

        private string initialCatalog;
        [Required(ErrorMessage = "This field is required.")]
        public string InitialCatalog
        {
            get { return initialCatalog; }
            set { SetProperty(ref initialCatalog, value); }
        }

        private string username;
        [Required(ErrorMessage = "This field is required.")]
        public string Username
        {
            get { return username; }
            set { SetProperty(ref username, value); }
        }

        private string password;
        [Required(ErrorMessage = "This field is required.")]
        public string Password
        {
            get { return password; }
            set { SetProperty(ref password, value); }
        }
    }
}
