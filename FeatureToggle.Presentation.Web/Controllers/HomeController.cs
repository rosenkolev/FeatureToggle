using FeatureToggle.Core.Services.Business;
using System.Web.Mvc;

namespace FeatureToggle.Presentation.Web.Controllers
{
    /// <summary>
    /// Main/Home Web Controller.
    /// </summary>
    public class HomeController : Controller
    {
        #region Services
        /// <summary>
        /// Gets or sets the content service.
        /// </summary>
        public IWelcomeService ContentService { get; set; }

        /// <summary>
        /// Gets or sets the footer service.
        /// </summary>
        public IFooterService FooterService { get; set; }
        #endregion

        #region Actions
        /// <summary>
        /// Retrieve the Main Working View.
        /// </summary>
        public ActionResult Index()
        {
            ViewBag.IsContentServiceActive = ContentService != null;

            if (ViewBag.IsContentServiceActive)
            {
                ViewBag.Title = ContentService.GetTitle();
                ViewBag.ExtraContent = ContentService.GetExtraContent();
            }

            ViewBag.Copyright = FooterService.GetCopyright();
            ViewBag.Adv = FooterService.GetAdvertising();
            return View();
        }
        #endregion
    }
}