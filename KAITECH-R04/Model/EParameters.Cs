using Autodesk.Revit.DB;
using DLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoundationRFT.Model
{
    public class EParameters
    {
        public EParameters()
        {
            ElementVolume = 0;
            ElementArea = 0;
            ElementLength = 0;
            ElementWidth = 0;
            ElementRadius = 0;
            ElementDaimeter = 0;
            ElementHeight = 0;
            ElementCurvesCount = 0;
            ElementTotalCurvesLength = 0;
            ElementColumnShape = "";
            ElementClearHeight = 0;
            
        }
        public double ElementVolume { get; set; }
        public double ElementArea { get; set; }
        public double ElementLength { get; set; }
        public double ElementWidth { get; set; }
        public double ElementRadius { get; set; }
        public double ElementDaimeter { get; set; }
        public double ElementHeight { get; set; }
        public double ElementClearHeight { get; set; }
        public int ElementCurvesCount { get; set; }
        public double ElementTotalCurvesLength { get; set; }
        public List<Curve> ElementCurvesList { get; set; }
        public PlanarFace ElementPlane { get; set; }
        public Solid ElementSolid { get; set; }
        public List<PlanarFace> ElementAllPlanerFaces { get; set; }
        public ViewPlan ElementViewPlane { get; set; }
        public List<Face> ElementSideFaces { get; set; }
        public string ElementLevel { get; set; }
        public string ElementColumnShape { get; set; }
        public List<Element> ElementIntersected { get; set; }
        public List<Element> ElementIntersectedFloors { get; set; }
    }
}

