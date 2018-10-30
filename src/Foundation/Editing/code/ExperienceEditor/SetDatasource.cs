using Sitecore.Data;
using Sitecore.Diagnostics;
using Sitecore.Shell.Framework.Commands;
using Sitecore.Data.Items;
using Sitecore.Shell.Applications.WebEdit;
using Sitecore.Web.UI.Sheer;
using System.Collections.Specialized;

namespace Sitecore.Foundation.Editing.ExperienceEditor
{
	public class SetDatasource : Sitecore.Shell.Applications.WebEdit.Commands.SetDatasource
	{
		public override void Execute(CommandContext context)
		{
			Assert.ArgumentNotNull(context, "context");
			ItemUri itemUri = ItemUri.ParseQueryString();
			if (itemUri != null)
			{
				Item item = Database.GetItem(itemUri);
				if (item != null && !Sitecore.Web.WebEditUtil.CanDesignItem(item))
				{
					SheerResponse.Alert("The action cannot be executed because of security restrictions.");
					return;
				}
			}
			NameValueCollection nameValueCollection = new NameValueCollection();
			string value = context.Parameters["referenceId"];
			Assert.IsNotNullOrEmpty(value, "uniqueid must not be empty");
			nameValueCollection["uniqueId"] = value;
			string value2 = context.Parameters["renderingId"];
			Assert.IsNotNullOrEmpty(value2, "rendering item id must not be empty");
			nameValueCollection["renderingItemId"] = value2;
			string value3 = context.Parameters["id"];
			if (!string.IsNullOrEmpty(value3))
			{
				nameValueCollection["id"] = value3;
			}
			Sitecore.Context.ClientPage.Start(this, "Run", nameValueCollection);
		}
	}
}