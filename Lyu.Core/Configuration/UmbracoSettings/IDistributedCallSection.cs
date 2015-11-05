using System.Collections.Generic;

namespace Lyu.Core.Configuration.UmbracoSettings
{
    public interface IDistributedCallSection : IUmbracoConfigurationSection
    {
        bool Enabled { get; }

        int UserId { get; }

        IEnumerable<IServer> Servers { get; }
    }
}