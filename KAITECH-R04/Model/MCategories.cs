using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoundationRFT.Model
{
    public class MCategories
    {
        public ElementId CategoriesId { get; set; }
        public string CategoriesName { get; set; }
        public List<MElements> Elements { get; set; }
    }
}
