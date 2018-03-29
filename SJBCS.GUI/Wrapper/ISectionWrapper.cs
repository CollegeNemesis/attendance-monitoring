using SJBCS.Data;
using System.Collections.Generic;

namespace SJBCS.GUI.Wrapper
{
    public interface ISectionWrapper
    {
        System.Guid SectionID { get; set; }
        string SectionName { get; set; }
        System.Guid LevelID { get; set; }
        string StartTime { get; set; }
        string EndTime { get; set; }
        Level Level { get; set; }
        ICollection<Data.Student> Students { get; set; }
    }
}
