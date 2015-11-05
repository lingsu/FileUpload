using System.Collections.Generic;

namespace Lyu.Core.Configuration.UmbracoSettings
{
    public interface IHelpSection : IUmbracoConfigurationSection
    {
        string DefaultUrl { get; }

        IEnumerable<ILink> Links { get; }
    }
}