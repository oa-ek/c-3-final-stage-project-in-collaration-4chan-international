using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models_Context.Models
{
    public class CharacterBuild
    {
        public int Id { get; set; }
        public string Name { get; set; } 

       
        public int? Vigor { get; set; }
        public int? Endurance { get; set; }
        public int? Strength { get; set; }
        public int? Dexterity { get; set; }
        public int? Intelligence { get; set; }
        public int? Faith { get; set; }

        
        public int? HeadId { get; set; }
        public int? ChestId { get; set; }
        public int? HandsId { get; set; }
        public int? LegsId { get; set; }
        public int? RightHandId { get; set; }
        public int? LeftHandId { get; set; }
    }
}
