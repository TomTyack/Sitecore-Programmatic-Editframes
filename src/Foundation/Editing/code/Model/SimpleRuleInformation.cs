using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sitecore.Foundation.Editing.Model
{
	public class SimpleRuleInformation
	{
		public string Name { get; set; }
		public string DatasourcePath { get; set; }
		public string DatasourceId { get; set; }
		public string PatternName { get; internal set; }
	}
}
