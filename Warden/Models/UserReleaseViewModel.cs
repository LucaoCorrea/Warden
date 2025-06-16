namespace Warden.Models
{
    public class UserReleaseViewModel
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public int ReleaseNoteId { get; set; }
        public DateTime ViewedAt { get; set; }
    }
}
