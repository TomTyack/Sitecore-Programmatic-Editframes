using Sitecore.Foundation.Editing.Model;

namespace Sitecore.Foundation.Editing.ExperienceEditor
{
	using System.Web.Mvc;

	public interface IExperienceEditorResponseService
	{
		void PopulateStandardExperienceResponse(ref IComponentModelBase model, Controller controller);
		void SetEditFrameLocation(string editFrameLocation);
	}
}
