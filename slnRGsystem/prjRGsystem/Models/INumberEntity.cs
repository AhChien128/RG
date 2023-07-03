using prjRGsystem.Context;

namespace prjRGsystem.Models
{
    public interface INumberEntity
    {
        public string? sysNumber { get; set; }
        public string GetPattern();
        public void GenerateSysNumber(RGPropertyContext db);
    }
}
