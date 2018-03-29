using AMS.Utilities;
using SJBCS.Data;
using SJBCS.GUI.Validation;
using SJBCS.GUI.Wrapper;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace SJBCS.GUI.Student
{
    public class AddableSection : ValidatableBindableBase, ISectionWrapper
    {
        private Guid sectionID;
        public Guid SectionID { get => sectionID; set => SetProperty(ref sectionID, value); }

        private ICollection<Level> levels;
        public ICollection<Level> Levels { get => levels; set => SetProperty(ref levels, value); }

        private string sectionName;
        [Required(ErrorMessage = "This field is required.")]
        public string SectionName
        {
            get => sectionName;

            set
            {
                ValidateUniqueSectionName(value);

                SetProperty(ref sectionName, value);
            }
        }

        private Guid levelID;
        public Guid LevelID
        {
            get => levelID;

            set
            {
                OnPropertyChanged("SectionName");
                SetProperty(ref levelID, value);
            }
        }

        private string startTime;
        [Required(ErrorMessage = "This field is required.")]
        [TimeValidation(ErrorMessage = "Invalid input format.")]
        public string StartTime { get => startTime; set => SetProperty(ref startTime, value); }


        private string endTime;
        [Required(ErrorMessage = "This field is required.")]
        [TimeValidation(ErrorMessage = "Invalid input format.")]
        public string EndTime { get => endTime; set => SetProperty(ref endTime, value); }

        private Level level;
        public Level Level { get => level; set => SetProperty(ref level, value); }

        private ICollection<Data.Student> students;
        public ICollection<Data.Student> Students { get => students; set => SetProperty(ref students, value); }

        private void ValidateUniqueSectionName(string value)
        {
            if (string.IsNullOrEmpty(value))
                return;

            foreach (Level level in Levels)
            {
                if (level.Sections.Any(x => x.SectionName.Trim().ToUpper().Equals(value.Trim().ToUpper())))
                {
                    throw new ArgumentException("Section name already exist.");
                }
            }

        }
    }
}
