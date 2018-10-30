using System;
using System.Collections;
using Sitecore.Globalization;

namespace Sitecore.Foundation.Editing.Model
{
    public class GlassBase : IGlassBase
	{
		public IEnumerable BaseTemplateIds { get; set; }

		public string FullPath { get; set; }

		public Guid Id { get; set; }

		public Language Language { get; set; }

		public string Name { get; set; }

		public Guid TemplateId { get; set; }

		public string TemplateName { get; set; }

		public string Url { get; set; }

		public int Version { get; set; }
	}
}
