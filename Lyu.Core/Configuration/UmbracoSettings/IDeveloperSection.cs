using System.Collections.Generic;

namespace Lyu.Core.Configuration.UmbracoSettings
{
    public interface IDeveloperSection : IUmbracoConfigurationSection
    {
        IEnumerable<IFileExtension> AppCodeFileExtensions { get; }
    }
}