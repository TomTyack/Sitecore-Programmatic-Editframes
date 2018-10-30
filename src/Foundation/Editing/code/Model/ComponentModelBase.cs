using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sitecore.Foundation.Editing.Model
{
	public class ComponentModelBase : IComponentModelBase
	{
		public bool ErrorOccurred { get; set; } // Set when retrieval of data encounters an issue.
        public bool ErrorMessage { get; set; }
        public bool IsExperienceEditorEditing { get; set; }
		public bool IsDataSourceSet { get; set; }
		public int DataSourceCount { get; set; }
		public string DataSourceIdentifier { get; set; }
		public string RenderingName { get; set; }
		public string DataSourceName { get; set; }
		public string DataSourcePath { get; set; }
		public List<SimpleRuleInformation> Rules { get; set; }
		public int LinkedItemCount { get; set; }
		public string EditDatasourceStart { get; set; }
		public string EditDatasourceEnd { get; set; }
		public List<GlassBase> LinkedItems { get; set; }
		public bool HasConditionalRenderings { get; set; }
		public string RenderingId { get; set; }
	}
}
