using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyzygyVeterinaryAPIControllersData.Models
{
	public class ClinicalExamModel
	{
		public int ClinicalExamId { get; set; }
		public DateTime ClinicalExamDate { get; set; }
		public int AnimalId { get; set; }
		public int LabTechnicianId { get; set; }

		/*public AnimalModel? Animal { get; set; }
		public LabTechnicianModel? LabTechnician { get; set; }*/
	}
}
