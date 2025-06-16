namespace Warden.Repository
{
    public interface IUserReleaseViewRepository
    {
        bool HasUserViewed(string userName, int releaseNoteId);
        void MarkAsViewed(string userName, int releaseNoteId);
    }
}
