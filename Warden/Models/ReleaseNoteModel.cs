namespace Warden.Models
{
    public class ReleaseNoteModel
    {
        public int Id { get; set; }
        public string Version { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime ReleaseDate { get; set; }
    }

}
