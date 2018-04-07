using ExpressiveAnnotations.Attributes;
using SJBCS.Data;
using SJBCS.GUI.Utilities;
using SJBCS.GUI.Validation;
using SJBCS.GUI.Wrapper;
using SJBCS.Services.Repository;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace SJBCS.GUI.Student
{
    public class EditableSection : ValidatableBindableBase, ISectionWrapper
    {

        public string OrigSectionName;

        private bool _editMode;
        public bool EditMode { get => _editMode; set => SetProperty(ref _editMode, value); }


        public bool IsDuplicateSectionName(string sectionName)
        {
            ISectionsRepository sectionsRepository = new SectionsRepository();

            if (EditMode && OrigSectionName.ToUpper().Trim().Equals(sectionName.ToUpper().Trim()))
                return true;

            foreach (Section section in sectionsRepository.GetSections())
            {
                if (section.SectionName.ToUpper().Trim().Equals(sectionName.ToUpper().Trim()))
                    return false;
            }
            return true;
        }

        public bool IsInvalidSchedule(string startTime, string endTime)
        {
            try
            {
                TimeSpan start = DateTime.Parse(StartTime).TimeOfDay;
                TimeSpan end = DateTime.Parse(endTime).TimeOfDay;

                if (start >= end)
                    return false;
                return true;
            }
            catch
            {
                return true;
            }
        }

        private Guid sectionID;
        public Guid SectionID { get => sectionID; set => SetProperty(ref sectionID, value); }

        private string sectionName;
        [Required(ErrorMessage = "This field is required.")]
        [AssertThat("IsDuplicateSectionName(SectionName)", ErrorMessage = "Section name is already existing.")]
        public string SectionName
        {
            get => sectionName;

            set
            {
                SetProperty(ref sectionName, value);
            }
        }

        private Guid levelID;
        public Guid LevelID
        {
            get => levelID;

            set
            {
                SetProperty(ref levelID, value);
            }
        }

        private string startTime;
        [Required(ErrorMessage = "This field is required.")]
        [TimeValidation(ErrorMessage = "Invalid input format.")]
        [AssertThat("IsInvalidSchedule(StartTime,EndTime)", ErrorMessage = "Invalid start time.")]
        public string StartTime
        {
            get => startTime;
            set
            {
                
                SetProperty(ref startTime, value);
            }
        }


        private string endTime;
        [Required(ErrorMessage = "This field is required.")]
        [TimeValidation(ErrorMessage = "Invalid input format.")]
        [AssertThat("IsInvalidSchedule(StartTime,EndTime)", ErrorMessage = "Invalid end time.")]
        public string EndTime
        {
            get => endTime;
            set
            {
                string temp = StartTime + "";
                StartTime = temp;
                SetProperty(ref endTime, value);
            }
        }

        private Level level;
        public Level Level { get => level; set => SetProperty(ref level, value); }

        private ObservableCollection<Level> levels;
        public ObservableCollection<Level> Levels { get => levels; set => SetProperty(ref levels, value); }


        private ICollection<Data.Student> students;
        public ICollection<Data.Student> Students { get => students; set => SetProperty(ref students, value); }
    }
}
