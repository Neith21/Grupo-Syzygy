using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyzygyVeterinaryAPIControllersData.Models
{
    public class DiagnosticsModel
    {
        public int DiagnosticId { get; set; }
        public string DiagnosticResult { get; set; }
        public DateTime DiagnosticDate { get; set; }
        public string DiagnosticObservations { get; set; }


        public int VeterinarianId { get; set; }
        public int ClinicalExamId { get; set; }

        public VeterinariansModel? Veterinarian { get; set; }
        public ClinicalExamModel? ClinicalExam { get; set; }
        
    }
}
