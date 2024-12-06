using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;

namespace prjRGsystem.Models.DbModels
{
    public class Estimates : AbstractAppEntity
    {
        [NotMapped]
        public const string TABLE_NAME = "Estimates";
        public string? carOwner { get; set; }
        public string? carLicense { get; set; }
        public string? engineNum { get; set; }
        public string? brand { get; set; }
        public string? carCategory { get; set; }
        public DateTime? year { get; set; }
        public string? contact { get; set; }
        public DateTime? entryDate { get; set; }
        public string? engineer { get; set; }
    }
}
