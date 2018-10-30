using System;
using System.Collections.Generic;
using System.Linq;
using Sitecore.Foundation.Editing.Model;
using Glass.Mapper.Sc;
using Sitecore.Data.Items;
using Sitecore.Data;
using Sitecore.Globalization;
using Sitecore.Links;
using System.Collections;

namespace Sitecore.Foundation.Editing.ExperienceEditor
{
	using System.Web.Mvc;
	using Sitecore.Layouts;
	using Sitecore.Mvc.Presentation;

	public class ExperienceEditorResponseService : IExperienceEditorResponseService
	{
		private readonly ISitecoreContext _glassService;
		private string _editFrameLocation;

		protected Controller actingController;

		public ExperienceEditorResponseService(ISitecoreContext glassItemService)
		{
			_glassService = glassItemService;
		}

		/// <summary>
		/// Uses the current Rendering context to setup basic fields in the model with information regarding the current rendering.
		/// Most of the processing in this method only happens if the content editor is in IsExperienceEditorEditing.
		/// </summary>
		/// <param name="model">A POCO object that will hold reference to our Rendering details</param>
		/// <param name="controller">A reference to the controller that is calling this method. This is needed for access to the HtmlHelper </param>
		public void PopulateStandardExperienceResponse(ref IComponentModelBase model, Controller controller)
		{
			if (model == null)
				return;

			actingController = controller;
			try
			{
				model.IsExperienceEditorEditing = Context.PageMode.IsExperienceEditorEditing;

				var currentRendering = RenderingContext.CurrentOrNull?.Rendering;

				model.IsDataSourceSet = !string.IsNullOrWhiteSpace(currentRendering.DataSource);
				model.RenderingName = currentRendering.RenderingItem.Name;
				model.RenderingId = currentRendering.Id.ToString();

				if (model.IsDataSourceSet && model.IsExperienceEditorEditing)
				{
					model.DataSourceCount = currentRendering.DataSource.Count();
					model.DataSourceIdentifier = currentRendering.DataSource;

					Guid datasource = Guid.Empty;
					if(Guid.TryParse(model.DataSourceIdentifier, out datasource))
					{
						var dataSourceItem = _glassService.GetItem<GlassBase>(datasource);
						model.DataSourceName = dataSourceItem.Name;
						model.DataSourcePath = dataSourceItem.FullPath;
						SetLinkedItems(model, datasource);
					}

					SetEditDatasourceEditFrame(model, controller);
				}

				if(model.IsExperienceEditorEditing)
				{
					var conditionalRenderings = GetPersonalizedRenderings(currentRendering);
					model.HasConditionalRenderings = conditionalRenderings.Any();
					if (model.HasConditionalRenderings)
						model.Rules = GetRulesForRendering(conditionalRenderings.FirstOrDefault());
				}
			}
			catch(Exception ex)
			{
				model.ErrorOccurred = true;
                model.ErrorMessage = ex.Message;
            }
		}

		/// <summary>
		/// Sets up the linked items in the model.
		/// </summary>
		/// <param name="model"></param>
		/// <param name="datasource"></param>
		private void SetLinkedItems(IComponentModelBase model, Guid datasource)
		{
			var linkedItems = GetLinkedItems(Sitecore.Context.Database, Sitecore.Context.Language, Sitecore.Context.Database.GetItem(new ID(datasource)));
			if(linkedItems.Length > 0)
			{
				model.LinkedItems = linkedItems.ToList().GroupBy(x => x.ID).Select(x => _glassService.GetItem<GlassBase>(x.Key.Guid)).ToList();
			}
			model.LinkedItemCount = model.LinkedItems?.Count() ?? 0;
		}

		/// <summary>
		/// Sets up the edit frame location of this component.
		/// </summary>
		/// <param name="editFrameLocation"></param>
		public void SetEditFrameLocation(string editFrameLocation)
		{
			_editFrameLocation = editFrameLocation;
		}

		/// <summary>
		/// Programmatically retrieves an edit frame. The HTML to construct the start and end of the edit frame are setup in the Datasource.
		/// </summary>
		/// <param name="model"></param>
		/// <param name="controller"></param>
		private void SetEditDatasourceEditFrame(IComponentModelBase model, Controller controller)
		{
			var htmlHelper = controller.GetHtmlHelper();
			EditFrameRendering editFrame = new EditFrameRendering(htmlHelper.ViewContext.Writer, model.DataSourcePath, _editFrameLocation, "Edit Component", string.Empty, string.Empty, null);
			model.EditDatasourceStart = editFrame.EditFrameStart();
			model.EditDatasourceEnd = editFrame.EditFrameEnd();
		}

		/// <summary>
		/// Using the link databases retrieve references the Item passed in.
		/// </summary>
		/// <param name="database"></param>
		/// <param name="language"></param>
		/// <param name="refItem"></param>
		/// <returns></returns>
		private static Item[] GetLinkedItems(Database database, Language language, Item refItem)
		{
			// getting all linked Items that refer to the “refItem” Item
			ItemLink[] links = Globals.LinkDatabase.GetReferers(refItem);
			if (links == null)
			{
				return null;
			}

			ArrayList result = new ArrayList(links.Length);

			foreach (ItemLink link in links)
			{
				// checking the database name of the linked Item
				if (link.SourceDatabaseName == database.Name)
				{
					Item item = database.Items[link.SourceItemID, language];
					// adding the Item to an array if the Item is not null
					if (item != null)
					{
						result.Add(item);
					}
				}
			}

			return (Item[])result.ToArray(typeof(Item));
		}

		/// <summary>
		/// Given a rendering get the list of RenderingReferences that contain personalisation Rules.
		/// General a single reference is returned.
		/// </summary>
		/// <param name="currentRendering"></param>
		/// <returns></returns>
		private List<RenderingReference> GetPersonalizedRenderings(Rendering currentRendering)
		{
			var item = Sitecore.Mvc.Presentation.RenderingContext.CurrentOrNull.PageContext.Item;

			Sitecore.Data.Fields.LayoutField layoutField = item.Fields["__final renderings"];
			RenderingReference[] renderings = layoutField.GetReferences(Context.Device);

			var renderingsWithPersonalization = renderings.Where(r => r.Settings.Rules.Count > 0).ToList();
			renderingsWithPersonalization = renderingsWithPersonalization.Where(r => currentRendering.Placeholder == r.Settings.Placeholder && currentRendering.DataSource == r.Settings.DataSource).ToList();

			return renderingsWithPersonalization;
		}

		/// <summary>
		/// Given a RenderingReference as a parameter. Retrieve basic details about the personlisation rules applied. 
		/// @Pre-Condition - the rendering passed in has personlisation
		/// </summary>
		/// <param name="current"></param>
		/// <returns></returns>
		private List<SimpleRuleInformation> GetRulesForRendering(RenderingReference current)
		{
			List<SimpleRuleInformation> rules = new List<SimpleRuleInformation>();

			foreach (var rule in current.Settings.Rules.Rules)
			{
				SimpleRuleInformation information = new SimpleRuleInformation();
				information.Name = rule.Name;
				if(rule.Condition is Sitecore.Analytics.Rules.Conditions.HasPatternCondition<Sitecore.Rules.ConditionalRenderings.ConditionalRenderingsRuleContext>)
				{
					var patternCondition = rule.Condition as Sitecore.Analytics.Rules.Conditions.HasPatternCondition<Sitecore.Rules.ConditionalRenderings.ConditionalRenderingsRuleContext>;
					information.PatternName = patternCondition.PatternName;
				}

				if (rule.Actions.Any())
				{
					var firstAction = rule.Actions.FirstOrDefault();
					if(firstAction != null)
					{
						if (firstAction is Sitecore.Rules.ConditionalRenderings.SetDataSourceAction<Sitecore.Rules.ConditionalRenderings.ConditionalRenderingsRuleContext>)
						{
							var patternCondition = firstAction as Sitecore.Rules.ConditionalRenderings.SetDataSourceAction<Sitecore.Rules.ConditionalRenderings.ConditionalRenderingsRuleContext>;
							information.DatasourceId = patternCondition.DataSource;
							var dataSourceItem = _glassService.GetItem<IGlassBase>(information.DatasourceId);
							if (dataSourceItem != null)
								information.DatasourcePath = dataSourceItem.FullPath;
						}
					}
				}
				
				rules.Add(information);
			}

			return rules;
		}
	}
}
