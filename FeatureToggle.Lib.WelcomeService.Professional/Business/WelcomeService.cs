using FeatureToggle.Core.Services.Business;

namespace FeatureToggle.Lib.Services.Business
{
    /// <summary>
    /// <see cref="IWelcomeService" /> Service Implementation.
    /// </summary>
    public class WelcomeService : IWelcomeService
    {
        /// See Interface For More
        public string GetTitle()
        {
            return "Hello sir!";
        }

        /// See Interface For More
        public string GetExtraContent()
        {
            return "You should check your email and the daily schedule.";
        }
    }
}
