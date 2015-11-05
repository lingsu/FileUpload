using System.Collections.Generic;

namespace Lyu.Core.Configuration.UmbracoSettings
{
    public interface IScheduledTasksSection : IUmbracoConfigurationSection
    {
        IEnumerable<IScheduledTask> Tasks { get; }

        string BaseUrl { get; }
    }
}