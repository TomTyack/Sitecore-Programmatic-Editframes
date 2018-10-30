using System;
using Sitecore.Web.UI.WebControls;
using Sitecore.Foundation.Editing.Model;

namespace Sitecore.Foundation.Editing.Extensions
{
    public static class FieldRenderers
    {
        public static String AttachFieldRender(this IGlassBase content, string fieldName)
        {
            // Experience Editor support
            if (Sitecore.Context.PageMode.IsExperienceEditorEditing)
            {
                var relatedItem = Sitecore.Context.Database.GetItem(new Sitecore.Data.ID(content.Id));
                return FieldRenderer.Render(relatedItem, fieldName);
            }

            return string.Empty;
        }
    }
}