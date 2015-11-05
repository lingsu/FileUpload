using System;
using System.Configuration;
using Lyu.Core.Configuration.UmbracoSettings;
using Lyu.Core.Logging;

namespace Lyu.Core.Configuration
{
    public class UmbracoConfig
    {
        #region Singleton

        private static readonly Lazy<UmbracoConfig> Lazy = new Lazy<UmbracoConfig>(() => new UmbracoConfig());

        public static UmbracoConfig For
        {
            get { return Lazy.Value; }
        }

        #endregion

        /// <summary>
        /// Default constructor 
        /// </summary>
        private UmbracoConfig()
        {
            if (_umbracoSettings == null)
            {
                var umbracoSettings = ConfigurationManager.GetSection("umbracoConfiguration/settings") as IUmbracoSettingsSection;
                SetUmbracoSettings(umbracoSettings);
            }

        }

        /// <summary>
        /// Constructor - can be used for testing
        /// </summary>
        /// <param name="umbracoSettings"></param>
        /// <param name="baseRestSettings"></param>
        /// <param name="dashboardSettings"></param>
        public UmbracoConfig(IUmbracoSettingsSection umbracoSettings)
        {
            SetUmbracoSettings(umbracoSettings);
        }

        private IUmbracoSettingsSection _umbracoSettings;

        /// <summary>
        /// Only for testing
        /// </summary>
        /// <param name="value"></param>
        public void SetUmbracoSettings(IUmbracoSettingsSection value)
        {
            _umbracoSettings = value;
        }

        /// <summary>
        /// Gets the IUmbracoSettings
        /// </summary>
        public IUmbracoSettingsSection UmbracoSettings()
        {
            if (_umbracoSettings == null)
            {
                var ex = new ConfigurationErrorsException("Could not load the " + typeof(IUmbracoSettingsSection) + " from config file, ensure the web.config and umbracoSettings.config files are formatted correctly");
                LogHelper.Error<UmbracoConfig>("Config error", ex);
                throw ex;
            }

            return _umbracoSettings;
        }
    }
}