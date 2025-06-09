namespace Warden.Helper
{
    public interface IEmail
    {
        bool SendEmail(string to, string subject, string body);
    }
}
