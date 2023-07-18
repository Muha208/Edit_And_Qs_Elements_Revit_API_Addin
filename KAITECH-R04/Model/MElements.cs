using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoundationRFT.Model
{
    public class MElements
    {
        public Element ELement { get; set; }
        public ElementId ELementsId { get; set; }
        public string ELementsName { get; set; }
        public string ELementFamilyName { get; set; }
        public EParameters ELementsParameters { get; set; }
    }
}
