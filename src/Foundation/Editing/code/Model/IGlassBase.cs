using System;
using System.Collections;
using Sitecore.Globalization;

namespace Sitecore.Foundation.Editing.Model
{
    /// <summary>
    /// The GlassBase interface.
    /// </summary>
    public interface IGlassBase
    {
        IEnumerable BaseTemplateIds { get; set; }

        string FullPath { get; set; }

        Guid Id { get; set; }

        Language Language { get; set; }

        string Name { get; set; }

        Guid TemplateId { get; set; }

        string TemplateName { get; set; }

        string Url { get; set; }

        int Version { get; set; }
    }
}
