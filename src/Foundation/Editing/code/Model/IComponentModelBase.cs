using System.Collections.Generic;

namespace Sitecore.Foundation.Editing.Model
{
	public interface IComponentModelBase
	{
		bool ErrorOccurred { get; set; } // Set when retrieval of data encounters an issue.
        string ErrorMessage { get; set; }

        bool IsExperienceEditorEditing { get; set; }
		bool IsDataSourceSet { get; set; }
		int DataSourceCount { get; set; }
		string DataSourceIdentifier { get; set; }
		string RenderingName { get; set; }
		string DataSourceName { get; set; }
		string DataSourcePath { get; set; }
		List<SimpleRuleInformation> Rules { get; set; }
		int LinkedItemCount { get; set; }
		string EditDatasourceStart { get; set; }
		string EditDatasourceEnd { get; set; }
		List<GlassBase> LinkedItems { get; set; }
		bool HasConditionalRenderings { get; set; }
		string RenderingId { get; set; }
    }
}
