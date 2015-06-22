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
            return "Hi my friend. How are you today?";
        }

        /// See Interface For More
        public string GetExtraContent()
        {
            return "Here are some suggestions to pick up your mood. Eat, sleep, party.";
        }
    }
}
