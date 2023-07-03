using prjRGsystem.Models.DbModels;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Newtonsoft.Json;

namespace prjRGsystem.Models
{
    public class SysLogDataChangeEntry
    {
        public SysLogDataChangeEntry(EntityEntry entry)
        {
            Entry = entry;
        }

        public EntityEntry Entry { get; }
        public string? tableName { get; set; }
        public string? requestId { get; set; }//  系統請求序號
        public string? sysUser { get; set; }//  使用者訪問稽核紀錄序號        
        public Dictionary<string, object> KeyValues { get; } = new Dictionary<string, object>();
        public Dictionary<string, object> OldValues { get; } = new Dictionary<string, object>();
        public Dictionary<string, object> NewValues { get; } = new Dictionary<string, object>();
        public List<PropertyEntry> TemporaryProperties { get; } = new List<PropertyEntry>();

        public bool HasTemporaryProperties => TemporaryProperties.Any();

        public SysLogDataChange ToSysLogDataChange()
        {
            var sysLogDataChange = new SysLogDataChange
            {
                tableName = tableName,
                requestId = requestId,
                sysUser = sysUser,
                keyValues = JsonConvert.SerializeObject(KeyValues),
                oldData = OldValues.Count == 0 ? null : JsonConvert.SerializeObject(OldValues),
                newData = NewValues.Count == 0 ? null : JsonConvert.SerializeObject(NewValues)
            };
            return sysLogDataChange;
        }
    }
}
