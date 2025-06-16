using Microsoft.AspNetCore.Mvc;
using Warden.Helper;
using Warden.Repository;

namespace Warden.Controllers
{
    public class HomeController : Controller
    {
        private readonly ISessionHelper _session;
        private readonly IReleaseNoteRepository _releaseRepo;
        private readonly IUserReleaseViewRepository _userViewRepo;

        public HomeController(ISessionHelper session, IReleaseNoteRepository releaseRepo, IUserReleaseViewRepository userViewRepo)
        {
            _session = session;
            _releaseRepo = releaseRepo;
            _userViewRepo = userViewRepo;
        }

        public IActionResult Index()
        {
            var user = _session.GetUserSession();

            if (user != null)
            {
                var latestRelease = _releaseRepo.GetLatest();

                if (latestRelease != null && !_userViewRepo.HasUserViewed(user.Name, latestRelease.Id))
                {
                    _userViewRepo.MarkAsViewed(user.Name, latestRelease.Id);

                    ViewBag.ShowReleaseNotes = true;
                    ViewBag.ReleaseNote = latestRelease;
                }
            }

            return View();
        }
    }
}
