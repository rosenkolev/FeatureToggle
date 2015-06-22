using FeatureToggle.Core.Services.Business;

namespace FeatureToggle.Lib.Services.Business
{
    /// <summary>
    /// <see cref="IFooterService" /> Service Implementation.
    /// </summary>
    public class FooterService : IFooterService
    {
        /// See Interface For More
        public string GetCopyright()
        {
            return "Copyright by MentorMate";
        }

        /// See Interface For More
        public string GetAdvertising()
        {
            return "Checkout mentormate at <a href=\"http://mentormate.com/\">mentormate.com</a>.";
        }
    }
}
