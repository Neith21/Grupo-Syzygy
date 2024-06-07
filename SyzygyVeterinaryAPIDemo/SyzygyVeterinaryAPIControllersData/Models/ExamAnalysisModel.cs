using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyzygyVeterinaryAPIControllersData.Models
{
	public class ExamAnalysisModel
	{
		public int ExamAnalysisId { get; set; }
		public string AnalysisType { get; set; }
		public string ResultData { get; set; }
		public int ClinicalExamId { get; set; }

		public ClinicalExamModel? ClinicalExam { get; set; }
	}
}
