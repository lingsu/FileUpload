using System.Collections.Generic;

namespace Lyu.Core.Configuration.UmbracoSettings
{
    public interface IRepositoriesSection : IUmbracoConfigurationSection
    {
        IEnumerable<IRepository> Repositories { get; }
    }
}