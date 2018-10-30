using System;

namespace Sitecore.Foundation.Editing.ExperienceEditor
{
	using System.IO;
	using System.Web.Mvc;
	public class FakeView : IView
	{
		public void Render(ViewContext viewContext, TextWriter writer)
		{
			throw new InvalidOperationException();
		}
	}
}
