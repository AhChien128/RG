namespace prjRGsystem.Models  
{
    public class AbstractAppEntity : AbstractEntity
    {
        public DateTime? createDate { get; set; }
        public string? createUser { get; set; }
        public DateTime? modifyDate { get; set; }
        public string? modifyUser { get; set; }
        public bool removed { get; set; }
    }
}
