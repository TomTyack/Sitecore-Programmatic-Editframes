namespace Sitecore.Foundation.Editing.ExperienceEditor
{
	using System.Text;
	using System.Web.Mvc;
	using System.Web.UI;
	public static class HtmlHelperExtensions
	{
		public static HtmlHelper GetHtmlHelper(this Controller controller)
		{
			StringBuilder sb = new StringBuilder();
			HtmlTextWriter htw = new HtmlTextWriter(new System.IO.StringWriter(sb, System.Globalization.CultureInfo.InvariantCulture));
			var viewContext = new ViewContext(controller.ControllerContext, new FakeView(), controller.ViewData, controller.TempData, htw);
			return new HtmlHelper(viewContext, new ViewPage());
		}
	}
}
