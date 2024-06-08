using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyzygyVeterinaryAPIControllersData.Models
{
    public class AnimalModel
    {
        public int AnimalId {  get; set; }
        public string AnimalName { get; set;}
        public int AnimalAge { get; set;}
        public char AnimalGender { get; set;}
        public int AnimalOwnerId { get; set;}
        public int SpeciesId { get; set;}

        public AnimalOwnerModel? AnimalOwners {  get; set;}
        public SpeciesModel? Species { get; set; }
    }
}
