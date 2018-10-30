using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sitecore.Foundation.Editing.ExperienceEditor
{
	using System.IO;
	using System.Text;
	using System.Web.UI;
	using Sitecore.Mvc.Common;

	public class EditFrameRendering
	{
		private EditFrame _editFrame;
		private HtmlTextWriter _htmlWriter;
		private readonly TextWriter writer;
		private string _datasourcePath;
		private readonly string editFramePath;
		private readonly string title;
		private readonly string tooltip;
		private readonly string cssClass;
		private readonly object parameters;

		public EditFrameRendering(TextWriter writer, string datasourcePath, string editFramePath, string title, string tooltip, string cssClass, object parameters)
		{
			this.writer = writer;
			this._datasourcePath = datasourcePath;
			this.editFramePath = editFramePath;
			this.title = title;
			this.tooltip = tooltip;
			this.cssClass = cssClass;
			this.parameters = parameters;
			this._editFrame = new EditFrame(this._datasourcePath, this.editFramePath, this.title, this.tooltip, this.cssClass, parameters);
		}

		public string EditFrameStart()
		{
			StringWriter stringWriter = new StringWriter();
			// Put HtmlTextWriter in using block because it needs to call Dispose.
			using (HtmlTextWriter writer = new HtmlTextWriter(stringWriter))
			{
				this._editFrame.RenderFirstPart(writer);
			}
			// Return the result.
			return stringWriter.ToString();
		}

		public string EditFrameEnd()
		{
			StringWriter stringWriter = new StringWriter();
			// Put HtmlTextWriter in using block because it needs to call Dispose.
			using (HtmlTextWriter writer = new HtmlTextWriter(stringWriter))
			{
				this._editFrame.RenderLastPart(writer);
			}
			// Return the result.
			return stringWriter.ToString();
		}

		private static string GetString(HtmlTextWriter writerCurrent)
		{
			// the flags to see the internal properties of the writer
			System.Reflection.BindingFlags flags = System.Reflection.BindingFlags.Default;
			flags |= System.Reflection.BindingFlags.NonPublic;
			flags |= System.Reflection.BindingFlags.Instance;
			flags |= System.Reflection.BindingFlags.FlattenHierarchy;
			flags |= System.Reflection.BindingFlags.Public;

			// get the information about the internal TextWriter object
			System.Reflection.FieldInfo baseWriter = writerCurrent.GetType().GetField("writer", flags);

			// use that info to create a StringWriter
			if (baseWriter != null)
			{
				StringWriter reflectedWriter = (StringWriter)baseWriter.GetValue(writerCurrent);

				// now we get a StringBuilder!
				StringBuilder builder = reflectedWriter.GetStringBuilder();

				return builder.ToString();
			}

			return string.Empty;
		}
	}
}
