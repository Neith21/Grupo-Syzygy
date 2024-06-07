using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyzygyVeterinaryAPIControllersData.Models
{
	public class ReferenceValueModel
	{
		public int ReferenceValueId { get; set; }
		public string AgeRange { get; set; }
		public string AnalysisType { get; set; }
		public string ReferenceData { get; set; }
		public int SpeciesId { get; set; }

		public SpeciesModel? Species { get; set; }
	}
}
