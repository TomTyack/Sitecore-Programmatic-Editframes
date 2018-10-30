using Glass.Mapper.Sc.Web.Mvc;
using Sitecore.Foundation.Editing.ExperienceEditor;
using Sitecore.Foundation.Editing.Extensions;
using Sitecore.Foundation.Editing.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Sitecore.Foundation.Editing.Controller
{
    public class SampleEditingController : GlassController
    {
        private readonly IExperienceEditorResponseService _experienceEditorService;

        public SampleEditingController(IExperienceEditorResponseService eEditorService)
        {
            _experienceEditorService = eEditorService;
        }

        public ActionResult ImageWithText()
        {
            var textGlassModel = GetDataSourceItem<IImageText>();

            var baseModel = (textGlassModel as IComponentModelBase);
            _experienceEditorService.SetEditFrameLocation(EditFrames.ImageWithText);
            _experienceEditorService.PopulateStandardExperienceResponse(ref baseModel, this);

            if (textGlassModel != null)
            {
                textGlassModel.TextEditable = textGlassModel.AttachFieldRender(FieldConstants.TextFields.Text);
            }
            return this.View("~/Views/components/ImageText.cshtml", textGlassModel);
        }
    }
}