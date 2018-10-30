using Sitecore.Foundation.Editing.Model;

namespace Sitecore.Foundation.Editing
{
    public interface IImageText : IComponentModelBase, IGlassBase
    {
        string Text { get; set; }
        string TextEditable { get; set; }
    }
}