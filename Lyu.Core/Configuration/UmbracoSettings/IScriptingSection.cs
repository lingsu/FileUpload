using System.Collections.Generic;

namespace Lyu.Core.Configuration.UmbracoSettings
{
    public interface IScriptingSection : IUmbracoConfigurationSection
    {
        IEnumerable<INotDynamicXmlDocument> NotDynamicXmlDocumentElements { get; }

        IEnumerable<IRazorStaticMapping> DataTypeModelStaticMappings { get; }
    }
}