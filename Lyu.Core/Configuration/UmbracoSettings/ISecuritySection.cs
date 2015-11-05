namespace Lyu.Core.Configuration.UmbracoSettings
{
    public interface ISecuritySection : IUmbracoConfigurationSection
    {
        bool KeepUserLoggedIn { get; }

        bool HideDisabledUsersInBackoffice { get; }

        string AuthCookieName { get; }

        string AuthCookieDomain { get; }
    }
}