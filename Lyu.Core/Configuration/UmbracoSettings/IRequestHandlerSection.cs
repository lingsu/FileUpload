using System.Collections.Generic;

namespace Lyu.Core.Configuration.UmbracoSettings
{
    public interface IRequestHandlerSection : IUmbracoConfigurationSection
    {
        bool UseDomainPrefixes { get; }

        bool AddTrailingSlash { get; }

        bool RemoveDoubleDashes { get; }

        bool ConvertUrlsToAscii { get; }

        IEnumerable<IChar> CharCollection { get; }
    }
}