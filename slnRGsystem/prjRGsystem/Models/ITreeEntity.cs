namespace prjRGsystem.Models
{
    public interface ITreeEntity
    {
        Int64 id { get; set; }

        Int64 parentId { get; set; }
    }
}
