using RootMvcProjectlib.Util;
using System.ComponentModel.DataAnnotations.Schema;

namespace prjRGsystem.Models.DbModels
{
    public class SysReserve : AbstractAppEntity
    {
        public string? subject { get; set; }
        public string? content { get; set; }
        public DateTime? stratDate { get; set; }
        public DateTime? endDate { get; set; }
        public string? memo { get; set; }
        public string level
        {
            get { return LevelType_E.ToString(); }
            set { LevelType_E = EnumUtil.Convert(value, LevelType.Low); }
        }
        [NotMapped]
        public LevelType LevelType_E { get; set; }
    }
}
