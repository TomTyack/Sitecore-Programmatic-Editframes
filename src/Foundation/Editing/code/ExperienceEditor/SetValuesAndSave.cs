using Sitecore;
using Sitecore.Data.Items;
using Sitecore.Data;
using Sitecore.Configuration;
using Sitecore.Shell.Applications.ContentEditor;
using Sitecore.Reflection;
using Sitecore.Web.UI.Sheer;
using Sitecore.Shell.Applications.ContentManager;
using System.Web.UI;

/// <summary>
/// http://www.flux-digital.com/blog/refreshing-experience-page-editor-after-using-edit-frames-or-buttons/
/// </summary>
namespace Sitecore.Foundation.Editing.ExperienceEditor
{
	public class SetValuesAndSave : Sitecore.Shell.Applications.ContentManager.ReturnFieldEditorValues.SetValues
	{
		public void Process(Sitecore.Shell.Applications.ContentManager.ReturnFieldEditorValues.ReturnFieldEditorValuesArgs args)
		{
			if (args.Options.DialogTitle != "Control Properties")
			{
				args.Options.SaveItem = true;
				foreach (FieldInfo fieldInfo in args.FieldInfo.Values)
				{
					Control subControl = Context.ClientPage.FindSubControl(fieldInfo.ID);
					if (subControl != null)
					{
						string str1;
						if (subControl is IContentField)
							str1 = StringUtil.GetString((subControl as IContentField).GetValue());
						else
							str1 = StringUtil.GetString(ReflectionUtil.GetProperty(subControl, "Value"));
						if (str1 != "__#!$No value$!#__")
						{
							string str2 = fieldInfo.Type.ToLowerInvariant();
							if (str2 == "rich text" || str2 == "html")
								str1 = str1.TrimEnd(' ');
							foreach (FieldDescriptor fieldDescriptor in args.Options.Fields)
							{
								if (fieldDescriptor.FieldID == fieldInfo.FieldID)
								{
									ItemUri itemUri = new ItemUri(fieldInfo.ItemID, fieldInfo.Language, fieldInfo.Version, Factory.GetDatabase(fieldDescriptor.ItemUri.DatabaseName));
									if (fieldDescriptor.ItemUri == itemUri)
										fieldDescriptor.Value = str1;
									Item item = Factory.GetDatabase(fieldDescriptor.ItemUri.DatabaseName).GetItem(itemUri.ItemID);

									// Save item as above doesn’t persist after postback
									if (item != null)
									{
										item.Editing.BeginEdit();
										item.Fields[fieldInfo.FieldID.ToString()].Value = str1;
										item.Editing.EndEdit();
									}
								}
							}
						}
					}
				}
			}
			else
			{
				base.Process(args);
			}
		}
	}
	public class ReturnCloseAndRefresh : Sitecore.Shell.Applications.ContentManager.ReturnFieldEditorValues.ReturnAndClose
	{
		public void Process(Sitecore.Shell.Applications.ContentManager.ReturnFieldEditorValues.ReturnFieldEditorValuesArgs args)
		{
			if (args.Options.DialogTitle != "Control Properties")
			{
				SheerResponse.SetDialogValue(args.Options.ToUrlHandle().ToHandleString());
				SheerResponse.SetModified(true);
				SheerResponse.CloseWindow();

				//Reload the page editor after closing
				SheerResponse.Eval("window.top.location.reload();");
			}
			else
			{
				base.Process(args);
			}
		}
	}
}