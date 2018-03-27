using AMS.Utilities;
using SJBCS.GUI.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SJBCS.GUI.Student
{
    public class EditableSection : ValidatableBindableBase
    {
        private Guid levelID;
        public Guid LevelID
        {
            get { return levelID; }
            set { SetProperty(ref levelID, value); }
        }

        private Guid sectionID;
        public Guid SectionID
        {
            get { return sectionID; }
            set { SetProperty(ref sectionID, value); }
        }

        private string sectionName;
        [Required(ErrorMessage = "This field is required.")]
        public string SectionName
        {
            get { return sectionName; }
            set { SetProperty(ref sectionName, value); }
        }

        private string startTime;
        [Required(ErrorMessage = "This field is required.")]
        [TimeValidation(ErrorMessage = "Invalid input format.")]
        public string StartTime
        {
            get { return startTime; }
            set
            {
                SetProperty(ref startTime, value);
            }
        }

        private string endTime;
        [Required(ErrorMessage = "This field is required.")]
        [TimeValidation(ErrorMessage = "Invalid input format.")]
        public string EndTime
        {
            get { return endTime; }
            set
            {
                SetProperty(ref endTime, value);
            }
        }


    }
}
