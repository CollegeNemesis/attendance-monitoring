using AMS.Utilities;
using SJBCS.Data;
using SJBCS.GUI.Validation;
using SJBCS.GUI.Wrapper;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace SJBCS.GUI.Student
{
    public class AddableSection : ValidatableBindableBase, ISectionWrapper
    {
        public bool HasExceptions;

        private Guid sectionID;
        public Guid SectionID { get => sectionID; set => SetProperty(ref sectionID, value); }

        private string sectionName;
        [Required(ErrorMessage = "This field is required.")]
        public string SectionName
        {
            get => sectionName;

            set
            {
                SetProperty(ref sectionName, value);
                ValidateUniqueSectionName(value);
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
        public string StartTime
        {
            get => startTime; set
            {
                SetProperty(ref startTime, value);
                ValidateStartTime();
            }
        }


        private string endTime;
        [Required(ErrorMessage = "This field is required.")]
        [TimeValidation(ErrorMessage = "Invalid input format.")]
        public string EndTime
        {
            get => endTime; set
            {
                SetProperty(ref endTime, value);
                ValidateEndTime();
            }
        }

        private Level level;
        public Level Level { get => level; set => SetProperty(ref level, value); }

        private ObservableCollection<Level> levels;

        public ObservableCollection<Level> Levels { get => levels; set => SetProperty(ref levels, value); }


        private ICollection<Data.Student> students;
        public ICollection<Data.Student> Students { get => students; set => SetProperty(ref students, value); }

        private void ValidateUniqueSectionName(string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                foreach (Level level in Levels)
                {
                    if (level.Sections.Any(x => x.SectionName.Trim().ToUpper().Equals(value.Trim().ToUpper())))
                    {
                        HasExceptions = true;
                        throw new ArgumentException("Section name already exist.");
                    }
                    else
                        HasExceptions = false;


                }
            }
        }

        private void ValidateStartTime()
        {
            if (!string.IsNullOrEmpty(StartTime) && !string.IsNullOrEmpty(EndTime))
            {
                DateTime start = new DateTime();
                DateTime end = new DateTime();

                if (DateTime.TryParse(StartTime, out start) && DateTime.TryParse(EndTime, out end))
                {
                    if (start.TimeOfDay >= end.TimeOfDay)
                    {
                        throw new ArgumentException("Start Time cannot be greater than End Time.");
                    }
                }
                OnPropertyChanged("EndTime");
            }
            
        }

        private void ValidateEndTime()
        {
            if (!string.IsNullOrEmpty(StartTime) && !string.IsNullOrEmpty(EndTime))
            {
                DateTime start = new DateTime();
                DateTime end = new DateTime();

                if (DateTime.TryParse(StartTime, out start) && DateTime.TryParse(EndTime, out end))
                {
                    if (start.TimeOfDay >= end.TimeOfDay)
                    {
                        throw new ArgumentException("End time cannot be less than Start Time.");
                    }
                }
            }
            OnPropertyChanged("StartTime");
        }
    }
}
