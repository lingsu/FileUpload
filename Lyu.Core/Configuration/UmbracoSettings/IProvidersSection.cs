﻿namespace Lyu.Core.Configuration.UmbracoSettings
{
    public interface IProvidersSection : IUmbracoConfigurationSection
    {
        string DefaultBackOfficeUserProvider { get; }
    }
}