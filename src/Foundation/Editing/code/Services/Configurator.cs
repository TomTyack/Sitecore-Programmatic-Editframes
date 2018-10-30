using Sitecore.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Extensions.DependencyInjection;
using Sitecore.Foundation.Editing.ExperienceEditor;

namespace Sitecore.Foundation.Editing.Services
{
    public class Configurator : IServicesConfigurator
    {
        public void Configure(IServiceCollection serviceCollection)
        {
			serviceCollection.AddTransient<IExperienceEditorResponseService, ExperienceEditorResponseService>();
		}
    }
}