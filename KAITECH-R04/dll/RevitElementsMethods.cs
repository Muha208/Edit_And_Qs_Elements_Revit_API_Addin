using Autodesk.Revit.Creation;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Visual;
using Autodesk.Revit.UI;
using DLL;
using FoundationRFT.Model;
using KAITECH_R04;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Xml;
using System.Xml.Linq;
using static DLL.OwneRevitMathods;
using Grid = Autodesk.Revit.DB.Grid;
using Line = Autodesk.Revit.DB.Line;

namespace Dll
{
    public class RevitElementsMethods
    {
        private static string RetunValueOfLevelElementCheck = "False";
        private static string RetunFamilyInstanceCheck = "False";
        private static Autodesk.Revit.DB.Document document = Commands.document;
        private static UIDocument uidoc = Commands.uidoc;
        private static int SN = 1;

        public static ViewFamilyType ViewPalnType;

        public static List<ElementId> GenereicModels;

        public static List<Level> ListOfColumnsLevel;
        public static List<ElementId> StairsList;
        public static List<ElementId> FoundationsList;
        public static List<ElementId> WallsList;
        public static List<ElementId> FloorsList;
        public static List<ElementId> FramingList;
        public static List<ElementId> RampsList;
        public static List<ElementId> SectionsList;
        public static List<ElementId> ElevationsList;
        public static List<ElementId> ImportInstanceList;
        public static List<ElementId> ViewFamilesEleIdList;

        public static List<ViewFamilyType> ViewFamilesEleList;

        public static ElementId TiltleBolckElementId;

        public static List<Element> ColumsList;
        public static List<Element> WallsEleList;
        public static List<Element> FloorsEleList;
        public static List<Element> FramingEleList;
        public static List<View> FloorEle;
        public static List<string> FloorEleName;
        public static List<Level> LevelsList;
        public static List<Element> ViewPalnTypeList;
        public static List<Grid> GridsElemints;
        public static List<Element> ColumnsTags;
        public static Grid OptGrid;


        public static double StartY;
        public static double EndY;
        public static double StartX;
        public static double EndX;


        public static List<View> Views;


        public static void GetElements()
        {
            TiltleBolckElementId = RevitFilters.GetSymbolElement(BuiltInCategory.OST_TitleBlocks).Id;

            ViewPalnType = RevitFilters.GetViewTypes(ViewType.FloorPlan).FirstOrDefault();
            ViewPalnTypeList = RevitFilters.GetElementsDataByClass(typeof(ViewPlan)).ELements;
            GenereicModels = RevitFilters.GetAllInstanceElements(BuiltInCategory.OST_GenericModel).ELementsID;

            StairsList = RevitFilters.GetAllInstanceElements(BuiltInCategory.OST_Stairs).ELementsID;
            FoundationsList = RevitFilters.GetAllInstanceElements(BuiltInCategory.OST_StructuralFoundation).ELementsID;
            WallsList = RevitFilters.GetAllInstanceElements(BuiltInCategory.OST_Walls).ELementsID;
            WallsEleList = RevitFilters.GetAllInstanceElements(BuiltInCategory.OST_Walls).ELements;
            FloorsList = RevitFilters.GetAllInstanceElements(BuiltInCategory.OST_Floors).ELementsID;
            FramingList = RevitFilters.GetAllInstanceElements(BuiltInCategory.OST_StructuralFraming).ELementsID;
            FloorsEleList = RevitFilters.GetAllInstanceElements(BuiltInCategory.OST_Floors).ELements;
            FramingEleList = RevitFilters.GetAllInstanceElements(BuiltInCategory.OST_StructuralFraming).ELements;
            RampsList = RevitFilters.GetAllInstanceElements(BuiltInCategory.OST_Ramps).ELementsID;
            SectionsList = RevitFilters.GetAllInstanceElements(BuiltInCategory.OST_Views).ELementsID;
            LevelsList = RevitFilters.GetAllInstanceElements(BuiltInCategory.OST_Levels).ELements.Cast<Level>().ToList();
            ElevationsList = RevitFilters.GetAllInstanceElements(BuiltInCategory.OST_SpotElevations).ELementsID;
            ImportInstanceList = RevitFilters.GetSymbolElements().ELementsID;
            ViewFamilesEleList = RevitFilters.GetViewTypes(ViewType.Section).ToList();
            ViewFamilesEleIdList = new List<ElementId>();
            foreach (var v in ViewFamilesEleList)
            {
                ViewFamilesEleIdList.Add(v.Id);
            }
            //Columns 
            ColumsList = RevitFilters.GetAllInstanceElements(BuiltInCategory.OST_StructuralColumns).ELements;
            //Get list of all grids
            GridsElemints = RevitFilters.GetAllInstanceElements(BuiltInCategory.OST_Grids).ELements.Cast<Grid>().ToList();
            //Get list of all Columns Tags
            ColumnsTags = RevitFilters.GetAllInstanceElements(BuiltInCategory.OST_StructuralColumnTags).ELements;
            //Views
            Views = RevitFilters.GetAllViewElements().Views;
            FloorEle = Views.Where(x => x is ViewPlan && x.ViewType == ViewType.FloorPlan && x.Origin != null).ToList();
            FloorEleName = new List<string>();
            foreach (var floor in FloorEle)
            {

                FloorEleName.Add(floor.Name);

            }

            GetMaxAndMinGridPoints();

            MainWindow.ViewCoboBox.ItemsSource = FloorEleName;
            MainWindow.ViewCoboBox.SelectedIndex = 0;
        }
        public static void GetMaxAndMinGridPoints()
        {
            var Max0Y = GridsElemints.FindAll(x => x.Curve.ComputeDerivatives(.5, true).BasisX.X == 0).Max(x => x.Curve.GetEndPoint(0).Y);
            var Min0Y = GridsElemints.FindAll(x => x.Curve.ComputeDerivatives(.5, true).BasisX.X == 0).Min(x => x.Curve.GetEndPoint(0).Y);
            var Max1Y = GridsElemints.FindAll(x => x.Curve.ComputeDerivatives(.5, true).BasisX.X == 0).Max(x => x.Curve.GetEndPoint(1).Y);
            var Min1Y = GridsElemints.FindAll(x => x.Curve.ComputeDerivatives(.5, true).BasisX.X == 0).Min(x => x.Curve.GetEndPoint(1).Y);

            var Max0X = GridsElemints.FindAll(x => x.Curve.ComputeDerivatives(.5, true).BasisX.Y == 0).Max(x => x.Curve.GetEndPoint(0).X);
            var Min0X = GridsElemints.FindAll(x => x.Curve.ComputeDerivatives(.5, true).BasisX.Y == 0).Min(x => x.Curve.GetEndPoint(0).X);
            var Max1X = GridsElemints.FindAll(x => x.Curve.ComputeDerivatives(.5, true).BasisX.Y == 0).Max(x => x.Curve.GetEndPoint(1).X);
            var Min1X = GridsElemints.FindAll(x => x.Curve.ComputeDerivatives(.5, true).BasisX.Y == 0).Min(x => x.Curve.GetEndPoint(1).X);

            StartY = Math.Max(Max0Y, Max1Y);
            EndY = Math.Min(Min0Y, Min1Y);
            StartX = Math.Max(Max0X, Max1X);
            EndX = Math.Min(Min0X, Min1X);


        }
        private static (string LevelName, ViewPlan viewPlan, Level ColumnLevel) CheckingElementLevel(MElements element)
        {
            if (element.ELement.Category.Name == "Structural Columns")
            {
                double ELementElevation = 0;
                ViewPlan viewPlan = null;
                if (element.ELement.LevelId.Value < 0 && element.ELementsParameters.ElementPlane != null)
                {
                    ELementElevation = Math.Ceiling(element.ELementsParameters.ElementPlane.Origin.Z);
                    var LevelsList = RevitFilters.GetAllLevelElements(BuiltInCategory.OST_Levels).levels;
                    foreach (var IntLevel in LevelsList)
                    {
                        if (Math.Ceiling(IntLevel.Elevation) == ELementElevation)
                        {
                            viewPlan = ViewPalnTypeList.Where(v => v.Name == IntLevel.Name).FirstOrDefault() as ViewPlan;
                            return (IntLevel.Name, viewPlan, IntLevel);
                        }
                    }
                    var LevelNewName = element.ELementsName + $"-ELevation {ELementElevation}-{element.ELementsId}";
                    Level NewLevel = LevelsList.Find(x => x.Name == LevelNewName);
                    if (NewLevel == null)
                    {
                        NewLevel = RevitCreate.CreateLevel(ELementElevation, LevelNewName);
                        viewPlan = RevitCreate.CreateViewPlane(NewLevel.Id, NewLevel.Name);
                    }
                    else
                    {
                        viewPlan = ViewPalnTypeList.Where(v => v.Name == NewLevel.Name).FirstOrDefault() as ViewPlan;
                    }
                    return (NewLevel.Name, viewPlan, NewLevel);
                }
                else
                {
                    var EleLevel = document.GetElement(element.ELement.LevelId) as Level;
                    if (EleLevel != null)
                    {
                        viewPlan = ViewPalnTypeList.Where(v => v.Name == EleLevel.Name).FirstOrDefault() as ViewPlan;
                        return (EleLevel.Name, viewPlan, EleLevel);
                    }
                    else
                    {
                        return ("Null", viewPlan, EleLevel);
                    }
                }
            }
            else
            {
                Level ELevel = null;
                string ELvelName = "Null";
                ViewPlan viewPlan = null;
                foreach (Parameter para in element.ELement.Parameters)
                {
                    switch (para.StorageType)
                    {
                        case StorageType.None:
                            break;
                        case StorageType.Integer:
                            break;
                        case StorageType.Double:
                            if (para.Definition.Name == "Reference Level")
                            {
                                if (para.AsElementId() != null || para.AsElementId().Value > 0)
                                {
                                    ELevel = document.GetElement(para.AsElementId()) as Level;
                                    ELvelName = ELevel.Name;
                                    viewPlan = ViewPalnTypeList.Where(v => v.Name == ELvelName).FirstOrDefault() as ViewPlan;
                                }
                            }
                            break;
                        case StorageType.String:
                            break;
                        case StorageType.ElementId:
                            break;
                        default:
                            break;
                    }
                }
                return (ELvelName, viewPlan, ELevel);
            }
        }
        [Obsolete]
        public static void FillClassesByLIstOfElements(List<Element> ListOfElements)
        {
            ListOfColumnsLevel = new List<Level>();
            OwneRevitMathods.InformationClass = new Informations() { SerialNumber = 1, CategoryInformation = new List<MCategories>() };
            OwneRevitMathods.ListOfSelectedElements = new List<Element>();
            OwneRevitMathods.FilterSelectionList = new List<string>() { OwneRevitMathods.SelectAllString };
            try
            {
                if (ListOfElements != null)
                {
                    SN = 1;
                    foreach (var element in ListOfElements)
                    {
                        if (element != null)
                        {
                            //Isert Data At Model Classes
                            if (OwneRevitMathods.InformationClass.CategoryInformation.Count > 0)
                            {
                                MCategories MatchCat = OwneRevitMathods.InformationClass.CategoryInformation.Find(cat => cat.CategoriesId == element.Category.Id);
                                if (MatchCat != null)
                                {
                                    MElements MatchEle = MatchCat.Elements.Find(ele => ele.ELementsId == element.Id);
                                    if (MatchEle == null)
                                    {
                                        MElements ElementsClass = new MElements();
                                        FillMElements(ElementsClass, element);
                                        MatchCat.Elements.Add(ElementsClass);
                                        OwneRevitMathods.ListOfSelectedElements.Add(element);
                                        OwneRevitMathods.FilterSelectionList.Add(element.Category.Name);
                                    }
                                }
                                else
                                {
                                    MCategories CategoryClass = new MCategories();
                                    CategoryClass.CategoriesId = element.Category.Id;
                                    CategoryClass.CategoriesName = element.Category.Name;
                                    MElements ElementsClass = new MElements();
                                    FillMElements(ElementsClass, element);
                                    CategoryClass.Elements = new List<MElements>() { ElementsClass };
                                    OwneRevitMathods.InformationClass.CategoryInformation.Add(CategoryClass);
                                    OwneRevitMathods.ListOfSelectedElements.Add(element);
                                    OwneRevitMathods.FilterSelectionList.Add(element.Category.Name);
                                }
                            }
                            else
                            {
                                MCategories CategoryClass = new MCategories();
                                CategoryClass.CategoriesId = element.Category.Id;
                                CategoryClass.CategoriesName = element.Category.Name;
                                MElements ElementsClass = new MElements();
                                FillMElements(ElementsClass, element);
                                CategoryClass.Elements = new List<MElements>() { ElementsClass };
                                OwneRevitMathods.InformationClass.CategoryInformation.Add(CategoryClass);
                                OwneRevitMathods.ListOfSelectedElements.Add(element);
                            }

                            SN++;
                        }
                    }
                    MainWindow.LogBox.Text = "Get Data Successfully!..........";
                }
                else
                {
                    MainWindow.LogBox.Text = "There is No Elements..............";
                }
            }
            catch (Exception e)
            {
                MainWindow.LogBox.Text = e.Message;
                throw;
            }
        }
        [Obsolete]
        private static void FillMElements(MElements ElementsClass, Element element)
        {
            var Logfile = new StringBuilder();
            Logfile.Append($"ELements To Fill:\n");
            ElementsClass.ELement = element;
            ElementsClass.ELementsName = element.Name;
            ElementsClass.ELementsId = element.Id;
            ElementsClass.ELementFamilyName = GetFamilyName(element);
            ElementsClass.ELementsParameters = new EParameters();
            //element.Category.Name == "Structural Columns" &&
            if (element is FamilyInstance)
            {
                if (ElementsClass.ELementFamilyName == "Concrete Round")
                {
                    ElementsClass.ELementsParameters.ElementRadius = GetElementCurvesInfo(element).Radius;
                    ElementsClass.ELementsParameters.ElementDaimeter = ElementsClass.ELementsParameters.ElementRadius * 2;
                }
                else
                {
                    ElementsClass.ELementsParameters.ElementRadius = 0;
                    ElementsClass.ELementsParameters.ElementDaimeter = 0;
                }
                ElementsClass.ELementsParameters.ElementLength = 0;
                ElementsClass.ELementsParameters.ElementWidth = 0;
                ElementsClass.ELementsParameters.ElementTotalCurvesLength = GetElementCurvesInfo(element).CurvesTotalLength;
                ElementsClass.ELementsParameters.ElementCurvesCount = GetElementCurvesInfo(element).CurvesCount;
                ElementsClass.ELementsParameters.ElementCurvesList = GetElementCurvesInfo(element).ListOfCurves;
                ElementsClass.ELementsParameters.ElementSolid = GetElementCurvesInfo(element).ElementSolid;
                ElementsClass.ELementsParameters.ElementPlane = GetElementCurvesInfo(element).ElePlane;
                ElementsClass.ELementsParameters.ElementSideFaces = GetElementCurvesInfo(element).SidePlanes;
                ElementsClass.ELementsParameters.ElementAllPlanerFaces = GetElementCurvesInfo(element).ListOfAllPlannerFaces;
                ElementsClass.ELementsParameters.ElementLevel = CheckingElementLevel(ElementsClass).LevelName;
                ElementsClass.ELementsParameters.ElementViewPlane = CheckingElementLevel(ElementsClass).viewPlan;
                ElementsClass.ELementsParameters.ElementIntersected = GetIntersectedElementsWithColumns(ElementsClass).InterSectionsElements;

                if (ElementsClass.ELement.Category.Name == "Structural Columns")
                {
                    if (ElementsClass.ELement.Id.Value == 458311)
                    {

                    }
                    ElementsClass.ELementsParameters.ElementIntersectedFloors = GetIntersectedElementsWithColumns(ElementsClass).InterSectionsFloorss;
                    ElementsClass.ELementsParameters.ElementHeight = GetColumnsHeights(ElementsClass.ELementsParameters.ElementIntersectedFloors, ElementsClass.ELement, GetIntersectedElementsWithColumns(ElementsClass).ListOfFloorElevations).ClearHeight;
                    ElementsClass.ELementsParameters.ElementColumnShape = GetColumnsHeights(ElementsClass.ELementsParameters.ElementIntersectedFloors, ElementsClass.ELement, GetIntersectedElementsWithColumns(ElementsClass).ListOfFloorElevations).ColumnShape;
                    ListOfColumnsLevel.Add(CheckingElementLevel(ElementsClass).ColumnLevel);
                }
                else
                {
                    ElementsClass.ELementsParameters.ElementHeight = GetHZElementsHeight(ElementsClass.ELement).Height;
                }
                ElementsClass.ELementsParameters.ElementClearHeight = GetIntersectedElementsWithColumns(ElementsClass).ClearHeight;
                ElementsClass.ELementsParameters.ElementArea = GetColumnDimeterHightAreaVolumeOrigin(GetFamilyInstance(element) as FamilyInstance, ElementsClass.ELementsParameters.ElementHeight).Area;

                if (ElementsClass.ELement.Category.Name == "Structural Columns")
                {
                    ElementsClass.ELementsParameters.ElementVolume = ElementsClass.ELementsParameters.ElementArea * ElementsClass.ELementsParameters.ElementClearHeight;
                }
                else
                {
                    ElementsClass.ELementsParameters.ElementVolume = GetColumnDimeterHightAreaVolumeOrigin(GetFamilyInstance(element) as FamilyInstance, ElementsClass.ELementsParameters.ElementHeight).Volume;
                }
            }
            OwneRevitMathods.FilterSelectionList.Add(element.Category.Name);
            Logfile.Append($"No.{SN}- Category: {element.Category.Name} --- Category ID: {element.Category.Id} --- Name: {element.Name} --- Id: {element.Id} " +
                           $"--- Area: {ElementsClass.ELementsParameters.ElementArea}" +
                           $"--- Volume: {ElementsClass.ELementsParameters.ElementVolume}" +
                           $"--- Height: {ElementsClass.ELementsParameters.ElementHeight}" +
                           $"--- Parameter: {ElementsClass.ELementsParameters.ElementTotalCurvesLength}" +
                           $"--- Curves Count: {ElementsClass.ELementsParameters.ElementCurvesCount}\n");
            Open_StreamFiles.CreatEncryptedLogFile(LogDirectors.ElementsToFillFilePath, ".log", Logfile, "OwneRevitMathods", false);
            MainWindow.LogBox.Text = Logfile.ToString();
        }
        public static (List<Element> InterSectionsElements, List<Element> InterSectionsFloorss, double TotalSubHeight, double ClearHeight, List<double> ListOfFloorElevations)
            GetIntersectedElementsWithColumns(MElements element)
        {
            var ColumnBoundingBox = element.ELement.get_BoundingBox(null);
            var EleCurves = element.ELementsParameters.ElementCurvesList;
            var IntersectedElements = new List<Element>();
            var ListOfFloorElevations = new List<double>();
            if (EleCurves != null)
            {

                if (ColumnBoundingBox != null)
                {
                    var MinPoint = ColumnBoundingBox.Min;
                    var MaxPoint = ColumnBoundingBox.Max;

                    var MaxZ = Math.Round(UnitUtils.ConvertFromInternalUnits(MaxPoint.Z, UnitTypeId.Meters), 2);
                    var MinZ = Math.Round(UnitUtils.ConvertFromInternalUnits(MinPoint.Z, UnitTypeId.Meters), 2);

                    var ClearHeight = UnitUtils.ConvertFromInternalUnits(Math.Abs(MaxPoint.Z - MinPoint.Z), UnitTypeId.Meters);
                    var ColumnOutline = new Outline(MinPoint, MaxPoint);
                    if (ColumnOutline != null)
                    {
                        BoundingBoxIntersectsFilter filter = new BoundingBoxIntersectsFilter(ColumnOutline);
                        FilteredElementCollector collector = new FilteredElementCollector(document);
                        IntersectedElements = collector.WherePasses(filter).WhereElementIsNotElementType().ToElements().Where(x => x.Category != null).ToList();
                    }
                    var RealIntersectionsElelemts = new Dictionary<Element, double>();
                    if (IntersectedElements != null)
                    {
                        foreach (var ele in IntersectedElements)
                        {
                            if (ele.Category.Name == "Floors")
                            {
                                var Intersected = 0;
                                var FloorG = ele.get_Geometry(new Options());
                                if (FloorG != null)
                                {
                                    foreach (var obj in FloorG)
                                    {
                                        if (obj is Solid)
                                        {
                                            Solid FloorS = obj as Solid;
                                            foreach (Face face in FloorS.Faces)
                                            {
                                                foreach (var curve in EleCurves)
                                                {
                                                    face.Intersect(curve, out IntersectionResultArray result);
                                                    if (result != null)
                                                    {
                                                        Intersected++;
                                                        goto GetElement;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            GetElement:
                                if (Intersected > 0)
                                {
                                    var MaxP = Math.Round(GetHZElementsHeight(ele).MaxP, 2);
                                    var MinP = Math.Round(GetHZElementsHeight(ele).MinP, 2);

                                    if (MaxP > MinZ && MaxP < MaxZ)
                                    {
                                        if (MinP > MinZ && MinP < MaxZ)
                                        {
                                            RealIntersectionsElelemts.Add(ele, MaxP);
                                        }
                                        else if (MinP < MinZ)
                                        {
                                            RealIntersectionsElelemts.Add(ele, MinZ);
                                        }
                                    }
                                    else if (MaxP > MaxZ)
                                    {
                                        if (MinP > MinZ && MinP < MaxZ)
                                        {
                                            RealIntersectionsElelemts.Add(ele, MaxZ);
                                        }
                                    }
                                }
                            }
                        }
                    }
                    var GetAllH = new List<Element>();
                    var TotalH = new List<double>();
                    ListOfFloorElevations = RealIntersectionsElelemts.Values.Distinct().ToList();

                    foreach (var key in ListOfFloorElevations)
                    {
                        var ListOfMax = new List<double>();
                        var ListOfMaxEle = new List<Element>();
                        foreach (var ele in RealIntersectionsElelemts)
                        {
                            if (ele.Value == key)
                            {
                                if (ele.Value == MinZ)
                                {
                                    var MaxP = GetHZElementsHeight(ele.Key).MaxP;
                                    var H = Math.Abs(MaxP - UnitUtils.ConvertFromInternalUnits(MinPoint.Z, UnitTypeId.Meters));
                                    ListOfMax.Add(H);
                                    ListOfMaxEle.Add(ele.Key);
                                }
                                else if (ele.Value == MaxZ)
                                {
                                    var MinP = GetHZElementsHeight(ele.Key).MinP;
                                    var H = Math.Abs(UnitUtils.ConvertFromInternalUnits(MaxPoint.Z, UnitTypeId.Meters) - MinP);
                                    ListOfMax.Add(H);
                                    ListOfMaxEle.Add(ele.Key);
                                }
                                else
                                {
                                    ListOfMax.Add(GetHZElementsHeight(ele.Key).Height);
                                    ListOfMaxEle.Add(ele.Key);
                                }
                            }
                        }
                        var MaxMax = ListOfMax.Max();
                        var indedx = ListOfMax.IndexOf(ListOfMax.FirstOrDefault(x => x == MaxMax));
                        TotalH.Add(MaxMax);
                        GetAllH.Add(ListOfMaxEle.ElementAt(indedx));
                    }
                    ClearHeight -= TotalH.Sum();
                    return (IntersectedElements, GetAllH.ToList(), TotalH.Sum(), ClearHeight, ListOfFloorElevations);
                }
            }
            return (null, null, 0, 0, ListOfFloorElevations);
        }
        public static (double Height, double MaxP, double MinP) GetHZElementsHeight(Element element)
        {
            var FramingBoundingBox = element.get_BoundingBox(null);

            if (FramingBoundingBox != null)
            {
                var MaxH = UnitUtils.ConvertFromInternalUnits(FramingBoundingBox.Max.Z, UnitTypeId.Meters);
                var MinH = UnitUtils.ConvertFromInternalUnits(FramingBoundingBox.Min.Z, UnitTypeId.Meters);
                return (MaxH - MinH, MaxH, MinH);
            }
            return (0, 0, 0);
        }
        public static object GetFamilyInstance(Element element)
        {
            if (element != null)
            {
                if (element is FamilyInstance)
                {
                    RetunFamilyInstanceCheck = "True";
                    return element as FamilyInstance;
                }
            }
            return element;
        }
        public static string GetFamilyName(Element element)
        {
            if (RetunFamilyInstanceCheck == "True")
            {
                var Logfile = new StringBuilder();
                Logfile.Append($"Successed To Get Family Name Of Elements:\n");
                FamilyInstance familyInstance = GetFamilyInstance(element) as FamilyInstance;
                if (familyInstance != null)
                {
                    Logfile.Append($"Family Name: {familyInstance.Symbol.Family.Name} --- Element Name: {element.Name}  --- Element Id: {element.Id}\n");
                    Open_StreamFiles.CreatEncryptedLogFile(LogDirectors.GetFamilyName, ".log", Logfile, "Revit Elements", false);
                    return familyInstance.Symbol.Family.Name;
                }
            }
            return "";
        }
        private static (double Area, double PalnElevation) GetAreaOfFaceElementCastOnPlace(Element element)
        {
            //Get the goemetry object of the Element
            GeometryElement geometryElement = element.get_Geometry(new Options());
            //iterate over ther geometry objects and find the extrusion
            if (geometryElement != null)
            {
                foreach (var geometryObject in geometryElement)
                {
                    Solid solidIn = geometryObject as Solid;
                    if (solidIn != null && solidIn.Volume > 0)
                    {
                        foreach (Face face in solidIn.Faces)
                        {
                            PlanarFace planarFace = face as PlanarFace;
                            if (planarFace != null && (planarFace.FaceNormal.IsAlmostEqualTo(XYZ.BasisZ) || planarFace.FaceNormal.IsAlmostEqualTo(XYZ.BasisZ.Negate())))
                            {
                                EdgeArrayArray edgeLoops = planarFace.EdgeLoops;
                                if (edgeLoops != null && edgeLoops.Size > 0)
                                {
                                    return (UnitUtils.ConvertFromInternalUnits(planarFace.Area, UnitTypeId.SquareMeters), planarFace.Origin.Z);
                                }
                            }
                        }
                    }
                    else
                    {
                        var geometryObjects = geometryObject as GeometryInstance;
                        if (geometryObjects != null)
                        {
                            var GeoInsta = geometryObjects.GetInstanceGeometry();
                            foreach (var GObject in GeoInsta)
                            {
                                Solid solid = GObject as Solid;
                                if (solid != null && solid.Volume > 0)
                                {
                                    foreach (Face face in solid.Faces)
                                    {
                                        PlanarFace planarFace = face as PlanarFace;
                                        if (planarFace != null && (planarFace.FaceNormal.IsAlmostEqualTo(XYZ.BasisZ) || planarFace.FaceNormal.IsAlmostEqualTo(XYZ.BasisZ.Negate())))
                                        {
                                            EdgeArrayArray edgeLoops = planarFace.EdgeLoops;
                                            if (edgeLoops != null && edgeLoops.Size > 0)
                                            {
                                                return (UnitUtils.ConvertFromInternalUnits(planarFace.Area, UnitTypeId.SquareMeters), planarFace.Origin.Z);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return (0, 0);
        }
        //---------------------------------------------------------------------------------------------------------//

        public static (Dictionary<string, double> ListOfHeightWithLevels, double ClearHeight, string ColumnShape) GetColumnsHeights(List<Element> ElementsLIst, Element element, List<double> ListOfElevations)
        {
            var ListOfHeightWithLevels = new Dictionary<string, double>();
            var UElevationsList = new List<double>();
            var stringShap = new StringBuilder();
            //Get Hieght Of intersected Elements
            double TotalCH = 0;
            //Get All Elevations From Plans
            //for (int i = 0; i < planarFaces.Count; i++)
            //{
            //    UElevationsList.Add(UnitUtils.ConvertFromInternalUnits(planarFaces[i].Origin.Z, UnitTypeId.Meters));
            //}
            var ColumnBoundingBox = element.get_BoundingBox(null);
            var MinPoint = ColumnBoundingBox.Min;
            var MaxPoint = ColumnBoundingBox.Max;

            var MaxZ = Math.Round(UnitUtils.ConvertFromInternalUnits(MaxPoint.Z, UnitTypeId.Meters), 2);
            var MinZ = Math.Round(UnitUtils.ConvertFromInternalUnits(MinPoint.Z, UnitTypeId.Meters), 2);
            UElevationsList = ListOfElevations;
            UElevationsList.Add(MaxZ);
            UElevationsList.Add(MinZ);
            if (UElevationsList.Count > 0)
            {
                var ElevationsList = UElevationsList.Distinct().OrderByDescending(x => x).ToList();
                double ZeroH = 0;
                int ZeroIndex = ElevationsList.Count;
                double ZeroR = 0;
                for (int i = 0; i < ElevationsList.Count; i++)
                {
                    var StringElev = new StringBuilder(); ;
                    if (i < ElevationsList.Count - 1)
                    {

                        //To Get The Zero Line To Draw
                        if (ElevationsList[i + 1] < 0 && ElevationsList[i] > 0)
                        {
                            ZeroH = ElevationsList[i];
                            ZeroIndex = i;
                            ZeroR = Math.Round(ZeroH / (Math.Abs(ElevationsList[i + 1]) + Math.Abs(ElevationsList[i])));
                        }
                        StringElev.Append($"--- ({ElevationsList.Count}-{ElevationsList.Count - i}) - Elev({Math.Round(ElevationsList[i], 2)}) ---\n");

                        if (ElementsLIst != null)
                        {
                            if (ElementsLIst != null)
                            {
                                foreach (var ele in ElementsLIst)
                                {
                                    var MaxP = Math.Round(GetHZElementsHeight(ele).MaxP, 2);
                                    if (MaxP == ElevationsList[i])
                                    {
                                        StringElev.Append($"-------------------------------------------\n" + $"----- {ele.Name} -----\n");
                                    }
                                }
                            }
                        }
                        double H = 0;
                        if ((ElevationsList[i + 1] < 0 && ElevationsList[i] > 0) || (ElevationsList[i] < 0 && ElevationsList[i + 1] > 0))
                        {
                            H = Math.Abs(ElevationsList[i + 1]) + Math.Abs(ElevationsList[i]);
                        }
                        else
                        {
                            H = Math.Abs(Math.Abs(ElevationsList[i + 1]) - Math.Abs(ElevationsList[i]));
                        }
                        ZeroR *= H;
                        ListOfHeightWithLevels.Add(StringElev.ToString(), H);
                    }
                    if (i == ElevationsList.Count - 1)
                    {
                        if (ElementsLIst != null)
                        {
                            if (ElementsLIst != null)
                            {
                                foreach (var ele in ElementsLIst)
                                {
                                    var MaxP = Math.Round(GetHZElementsHeight(ele).MaxP, 2);
                                    if (MaxP == ElevationsList[i])
                                    {
                                        StringElev.Append($"-------------------------------------------\n" + $"----- {ele.Name} ------\n");
                                    }
                                }
                            }
                        }
                        StringElev.Append($"--- ({ElevationsList.Count}-{ElevationsList.Count - i}) - Elev({Math.Round(ElevationsList[i], 2)}) ---\n");
                    }
                }
                var TotalH = ListOfHeightWithLevels.Values.Sum();
                TotalCH = ListOfHeightWithLevels.Values.Sum();
                for (int i = 0; i < ListOfHeightWithLevels.Count; i++)
                {
                    //write Each Level Befor it's Line Draw
                    stringShap.Append($"{ListOfHeightWithLevels.ElementAt(i).Key}" +
                                      $"-------------------------------------------\n");

                    int LinesCount = (int)Math.Round(20 * (ListOfHeightWithLevels.ElementAt(i).Value / TotalH), 1);
                    int PZeroLine = (int)Math.Round(ZeroR * LinesCount, 1);
                    if (LinesCount < 3)
                    {
                        LinesCount = 3;
                    }
                    int HalfLine = (int)LinesCount / 2;
                    if (LinesCount == 3)
                    {
                        HalfLine = 2;
                    }
                    //Drow Each Connected Column Line
                    for (int j = 0; j < LinesCount; j++)
                    {
                        if (j == 0)
                        {
                            stringShap.Append("   ||||||   ---\n");
                            if (ZeroH != 0 && ZeroR != 0 && ZeroIndex != ElevationsList.Count)
                            {
                                if (ZeroIndex == i & PZeroLine == j)
                                {
                                    stringShap.Append($"---------- Zero Level ----------\n");
                                }
                            }
                        }
                        else if (j == LinesCount - 1)
                        {
                            stringShap.Append("   ||||||   ---\n");
                            if (ZeroH != 0 && ZeroR != 0 && ZeroIndex != ElevationsList.Count)
                            {
                                if (ZeroIndex == i & PZeroLine == j)
                                {
                                    stringShap.Append($"---------- Zero Level ----------\n");
                                }
                            }
                        }
                        else if (j == HalfLine)
                        {
                            if (i == (int)ListOfHeightWithLevels.Count / 2)
                            {
                                stringShap.Append($"   ||||||      {Math.Round(ListOfHeightWithLevels.ElementAt(i).Value, 2)} m\n");
                            }
                            else
                            {
                                stringShap.Append($"   ||||||      {Math.Round(ListOfHeightWithLevels.ElementAt(i).Value, 2)} m\n");
                            }
                            if (ZeroH != 0 && ZeroR != 0 && ZeroIndex != ElevationsList.Count)
                            {
                                if (ZeroIndex == i)
                                {
                                    if (ZeroIndex == i & PZeroLine == j)
                                    {
                                        stringShap.Append($"---------- Zero Level ----------\n");
                                    }
                                }
                            }
                        }
                        else
                        {
                            if (LinesCount == 3)
                            {
                                if (i == (int)ListOfHeightWithLevels.Count / 2)
                                {
                                    stringShap.Append($"   ||||||      {Math.Round(ListOfHeightWithLevels.ElementAt(i).Value, 2)} m\n");
                                }
                                else
                                {
                                    stringShap.Append($"   ||||||      {Math.Round(ListOfHeightWithLevels.ElementAt(i).Value, 2)} m\n");
                                }
                                if (ZeroH != 0 && ZeroR != 0 && ZeroIndex != ElevationsList.Count)
                                {
                                    if (ZeroIndex == i & PZeroLine == j)
                                    {
                                        stringShap.Append($"---------- Zero Level ----------\n");
                                    }
                                }
                            }
                            else
                            {
                                stringShap.Append("   ||||||      |\n");
                            }
                        }
                    }
                }
                stringShap.Append($"\n-Total Height = {Math.Round(TotalH, 2)} m");
            }
            return (ListOfHeightWithLevels, TotalCH, stringShap.ToString());
        }
        #region Columns
        [Obsolete]
        public static (double Area, double Volume)
        GetColumnDimeterHightAreaVolumeOrigin(Element element, double Height)
        {
            double ColumnVolume = 0;
            double ColumnHeight = Height;
            double ColumnArea = 0;


            double TopFrame = 0;
            double BottomFrame = 0;
            if (element != null)
            {
                var FamilyInstanceOfColumn = GetFamilyInstance(element) as FamilyInstance;
                foreach (Parameter para in element.Parameters)
                {
                    switch (para.StorageType)
                    {
                        case StorageType.None:
                            break;
                        case StorageType.Integer:
                            break;
                        case StorageType.Double:

                            if (element.Category.Name == "Structural Framing")
                            {
                                if (para.Definition.Name == "Elevation at Top")
                                {
                                    TopFrame = UnitUtils.ConvertFromInternalUnits(para.AsDouble(), UnitTypeId.Meters);
                                }
                                if (para.Definition.Name == "Elevation at Bottom")
                                {
                                    BottomFrame = UnitUtils.ConvertFromInternalUnits(para.AsDouble(), UnitTypeId.Meters);
                                }
                            }

                            if (para.Definition.Name == "Volume")
                            {
                                if (para.AsValueString() != null)
                                {
                                    ColumnVolume = StringMethods.IsDoubleNumber(para.AsValueString().Replace("m³", ""));
                                }
                            }

                            break;
                        case StorageType.String:
                            break;
                        case StorageType.ElementId:

                            break;
                        default:
                            break;
                    }
                }
                if (element.Category.Name == "Structural Columns")
                {
                    ColumnArea = GetAreaOfFaceElementCastOnPlace(element).Area;
                }
                else
                {
                    ColumnArea = ColumnVolume / ColumnHeight;
                }

            }
            return (ColumnArea, ColumnVolume);
        }
        public static (List<Curve> CurvesList, double Radius, Solid ElementSolid) GetListOfColumnsCurvesViewPaln(ViewPlan viewPlan, Element element)
        {
            double Radius = 0;
            var Curves = new List<Curve>();
            var Logfile = new StringBuilder();
            int GetOne = 0;
            Solid EleSolid = null;
            Logfile.Append($"Successed To Get List Of Columns Curves At ViewPaln:\n");
            if (viewPlan != null)
            {
                if (element.LevelId == viewPlan.GenLevel.Id)
                {
                    // Get the column's location curve
                    GeometryElement geometryElement = element.get_Geometry(new Options());
                    //iterate over ther geometry objects and find the extrusion
                    foreach (var geometryObject in geometryElement)
                    {

                        Solid solidIn = geometryObject as Solid;
                        if (solidIn != null && solidIn.Volume > 0)
                        {
                            EleSolid = solidIn;
                            foreach (Face face in solidIn.Faces)
                            {
                                PlanarFace planarFace = face as PlanarFace;
                                if (planarFace != null)
                                {
                                    if (planarFace.FaceNormal.IsAlmostEqualTo(XYZ.BasisZ) || planarFace.FaceNormal.IsAlmostEqualTo(XYZ.BasisZ.Negate()))
                                    {
                                        if (GetOne != 0)
                                        {
                                            return (Curves, Radius, EleSolid);
                                        }
                                        else
                                        {
                                            XYZ Palnpoint = new XYZ(planarFace.Origin.X, planarFace.Origin.Y, viewPlan.Origin.Z);
                                            if (planarFace.Origin.IsAlmostEqualTo(Palnpoint))
                                            {
                                                var edgeLoops = planarFace.GetEdgesAsCurveLoops();
                                                if (edgeLoops != null && edgeLoops.Count > 0)
                                                {
                                                    // Add the curve to the list
                                                    foreach (var eArray in edgeLoops)
                                                    {
                                                        foreach (var curve in eArray)
                                                        {
                                                            if (curve is Arc)
                                                            {
                                                                Arc arc = curve as Arc;
                                                                GetOne = 1;
                                                                Curves.Add(arc);
                                                                Radius = UnitUtils.ConvertFromInternalUnits(arc.Radius, UnitTypeId.Meters);
                                                                Logfile.Append($"ARC Id: {arc.Id} -- ARC Length: {arc.Length} ---- ARC Raduis: {arc.Radius}");
                                                            }
                                                            else
                                                            {
                                                                GetOne = 1;
                                                                Curves.Add(curve);
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            var geometryObjects = geometryObject as GeometryInstance;
                            if (geometryObjects != null)
                            {
                                var GeoInsta = geometryObjects.GetInstanceGeometry();
                                foreach (var GObject in GeoInsta)
                                {
                                    Solid solid = GObject as Solid;
                                    if (solid != null && solid.Volume > 0)
                                    {
                                        EleSolid = solid;
                                        foreach (Face face in solid.Faces)
                                        {
                                            PlanarFace planarFace = face as PlanarFace;
                                            if (planarFace != null)
                                            {
                                                if (planarFace.FaceNormal.IsAlmostEqualTo(XYZ.BasisZ) || planarFace.FaceNormal.IsAlmostEqualTo(XYZ.BasisZ.Negate()))
                                                {
                                                    if (GetOne != 0)
                                                    {
                                                        return (Curves, Radius, EleSolid);
                                                    }
                                                    else
                                                    {
                                                        XYZ Palnpoint = new XYZ(planarFace.Origin.X, planarFace.Origin.Y, viewPlan.Origin.Z);
                                                        if (planarFace.Origin.IsAlmostEqualTo(Palnpoint))
                                                        {
                                                            var edgeLoops = planarFace.GetEdgesAsCurveLoops();
                                                            if (edgeLoops != null && edgeLoops.Count > 0)
                                                            {
                                                                // Add the curve to the list
                                                                foreach (var eArray in edgeLoops)
                                                                {
                                                                    foreach (var curve in eArray)
                                                                    {
                                                                        if (curve is Arc)
                                                                        {
                                                                            Arc arc = curve as Arc;
                                                                            GetOne = 1;
                                                                            Curves.Add(arc);
                                                                            Radius = UnitUtils.ConvertFromInternalUnits(arc.Radius, UnitTypeId.Meters);
                                                                            Logfile.Append($"ARC Id: {arc.Id} -- ARC Length: {arc.Length} ---- ARC Raduis: {arc.Radius}");
                                                                        }
                                                                        else
                                                                        {
                                                                            GetOne = 1;
                                                                            Curves.Add(curve);

                                                                        }
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    // Create a new reference array with the two grid lines End
                    ReferenceArray ColumnLineRefsList = new ReferenceArray();
                    Logfile.Append($"\n---------------------------------------------------------------------------\n");
                }
            }
            Open_StreamFiles.CreatEncryptedLogFile(LogDirectors.GetListOfColumnsCurvesAtViewPalnPath, ".log", Logfile, "Revit Elements", false);
            return (Curves, Radius, EleSolid);
        }
        public static (List<PlanarFace> ListOfAllPlannerFaces, List<Curve> ListOfCurves, int CurvesCount, double CurvesTotalLength, PlanarFace ElePlane, List<Face> SidePlanes, double Radius, Solid ElementSolid)
            GetElementCurvesInfo(Element element)
        {
            double Radius = 0;
            Solid ElementSolid = null;
            var Logfile = new StringBuilder();
            Logfile.Append($"Successed To Get List Of Columns Curves At ViewPaln:\n");
            // Get the columns in the view plan
            double Clength = 0;
            int GetOne = 0;
            // Get the column's location curve
            var Curves = new List<Curve>();
            var SidePlanes = new List<Face>();
            PlanarFace refPlane = null;
            var ListOfAllPlannerFaces = new List<PlanarFace>();
            GeometryElement geometryElement = element.get_Geometry(new Options());
            //iterate over ther geometry objects and find the extrusion
            if (geometryElement != null)
            {
                foreach (var geometryObject in geometryElement)
                {
                    Solid solidIn = geometryObject as Solid;
                    if (solidIn != null && solidIn.Volume > 0)
                    {
                        ElementSolid = solidIn;
                        foreach (Face face in solidIn.Faces)
                        {
                            PlanarFace planarFace = face as PlanarFace;
                            if (planarFace != null)
                            {
                                var LevelIdEle = element.LevelId;
                                var Zpoint = planarFace.Origin.Z;
                                if (LevelIdEle.Value > 0)
                                {
                                    Level level = document.GetElement(LevelIdEle) as Level;
                                    Zpoint = level.Elevation;
                                }
                                XYZ Palnpoint = new XYZ(planarFace.Origin.X, planarFace.Origin.Y, Zpoint);

                                if (planarFace.FaceNormal.IsAlmostEqualTo(XYZ.BasisZ) || planarFace.FaceNormal.IsAlmostEqualTo(XYZ.BasisZ.Negate()))
                                {

                                    if (GetOne != 0)
                                    {
                                        ListOfAllPlannerFaces.Add(planarFace);
                                    }
                                    else
                                    {
                                        ListOfAllPlannerFaces.Add(planarFace);
                                        if (planarFace.Origin.IsAlmostEqualTo(Palnpoint, 1))
                                        {
                                            var edgeLoops = planarFace.GetEdgesAsCurveLoops();
                                            if (edgeLoops != null && edgeLoops.Count > 0)
                                            {
                                                // Add the curve to the list
                                                foreach (var eArray in edgeLoops)
                                                {
                                                    foreach (var curve in eArray)
                                                    {
                                                        if (curve is Arc)
                                                        {
                                                            Arc arc = curve as Arc;
                                                            GetOne = 1;
                                                            Radius = UnitUtils.ConvertFromInternalUnits(arc.Radius, UnitTypeId.Meters);
                                                            Clength += UnitUtils.ConvertFromInternalUnits(arc.Length, UnitTypeId.Meters);
                                                            Curves.Add(arc);
                                                            Logfile.Append($"ARC Id: {arc.Id} -- ARC Length: {arc.Length} ---- ARC Raduis: {arc.Radius}");
                                                        }
                                                        else
                                                        {
                                                            GetOne = 1;
                                                            Clength += UnitUtils.ConvertFromInternalUnits(curve.Length, UnitTypeId.Meters);
                                                            Curves.Add(curve);
                                                            Logfile.Append($"Curve Id: {curve.Id} -- Curve Length: {curve.Length} ----");
                                                        }
                                                    }
                                                }
                                            }
                                            refPlane = planarFace;
                                        }
                                    }
                                }
                                else
                                {
                                    SidePlanes.Add(face);
                                }
                            }
                        }
                    }
                    else
                    {
                        if (solidIn == null)
                        {
                            var geometryObjects = geometryObject as GeometryInstance;
                            if (geometryObjects != null)
                            {
                                var GeoInsta = geometryObjects.GetInstanceGeometry();
                                foreach (var GObject in GeoInsta)
                                {
                                    Solid solid = GObject as Solid;
                                    if (solid != null && solid.Volume > 0)
                                    {
                                        ElementSolid = solid;
                                        foreach (Face face in solid.Faces)
                                        {
                                            PlanarFace planarFace = face as PlanarFace;
                                            if (planarFace != null)
                                            {
                                                var LevelIdEle = element.LevelId;
                                                var Zpoint = planarFace.Origin.Z;
                                                if (LevelIdEle.Value > 0)
                                                {
                                                    Level level = document.GetElement(LevelIdEle) as Level;
                                                    Zpoint = level.Elevation;
                                                }
                                                XYZ Palnpoint = new XYZ(planarFace.Origin.X, planarFace.Origin.Y, Zpoint);
                                                if (planarFace.FaceNormal.IsAlmostEqualTo(XYZ.BasisZ) || planarFace.FaceNormal.IsAlmostEqualTo(XYZ.BasisZ.Negate()))
                                                {
                                                    if (GetOne != 0)
                                                    {
                                                        ListOfAllPlannerFaces.Add(planarFace);
                                                    }
                                                    else
                                                    {
                                                        ListOfAllPlannerFaces.Add(planarFace);
                                                        if (planarFace.Origin.IsAlmostEqualTo(Palnpoint, 1))
                                                        {
                                                            var edgeLoops = planarFace.GetEdgesAsCurveLoops();
                                                            if (edgeLoops != null && edgeLoops.Count > 0)
                                                            {
                                                                // Add the curve to the list
                                                                foreach (var eArray in edgeLoops)
                                                                {
                                                                    foreach (var curve in eArray)
                                                                    {
                                                                        if (curve is Arc)
                                                                        {
                                                                            Arc arc = curve as Arc;
                                                                            GetOne = 1;
                                                                            Radius = UnitUtils.ConvertFromInternalUnits(arc.Radius, UnitTypeId.Meters);
                                                                            Clength += UnitUtils.ConvertFromInternalUnits(arc.Length, UnitTypeId.Meters);
                                                                            Curves.Add(arc);
                                                                            Logfile.Append($"ARC Id: {arc.Id} -- ARC Length: {arc.Length} ---- ARC Raduis: {arc.Radius}");
                                                                        }
                                                                        else
                                                                        {
                                                                            GetOne = 1;
                                                                            Clength += UnitUtils.ConvertFromInternalUnits(curve.Length, UnitTypeId.Meters);
                                                                            Curves.Add(curve);
                                                                            Logfile.Append($"Curve Id: {curve.Id} -- Curve Length: {curve.Length} ----");
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                            refPlane = planarFace;
                                                        }
                                                    }
                                                }
                                                else
                                                {
                                                    SidePlanes.Add(face);
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            Logfile.Append($"\n---------------------------------------------------------------------------\n");
            Open_StreamFiles.CreatEncryptedLogFile(LogDirectors.GetListOfColumnsCurvesAtViewPalnPath, ".log", Logfile, "Revit Elements", false);
            return (ListOfAllPlannerFaces, Curves, Curves.Count, Clength, refPlane, SidePlanes, Radius, ElementSolid);
        }
        public static (List<XYZ> IntersectionsPoints, List<string> IntesectedGridName) GetTheIntersectionPointsBetweenTowCurves(MElements ele, Curve curve, ViewPlan viewPlan)
        {
            var IntesectedGridName = new List<string>();
            var ColumnsIntersectionPoints = new List<XYZ>();
            if (ele.ELement.Category.Name == "Structural Columns")
            {
                if (viewPlan.Name.Contains(ele.ELementsParameters.ElementViewPlane.Name))
                {
                    var GridssElements = GridsElemints;

                    var Logfile = new StringBuilder();
                    Logfile.Append($"Successed To Get List Of Intersection Points Of Curves At ViewPaln:\n");

                    var point1 = new XYZ(curve.GetEndPoint(0).X, curve.GetEndPoint(0).Y, viewPlan.Origin.Z);
                    var point2 = new XYZ(curve.GetEndPoint(1).X, curve.GetEndPoint(1).Y, viewPlan.Origin.Z);
                    var point3 = new XYZ(curve.ComputeDerivatives(.5, true).Origin.X, curve.ComputeDerivatives(.5, true).Origin.Y, viewPlan.Origin.Z);
                    Curve NewLineC;
                    if (curve is Arc)
                    {
                        NewLineC = Arc.Create(point1, point2, point3);
                    }
                    else
                    {
                        NewLineC = Line.CreateBound(point1, point2);
                    }

                    foreach (Element gridEle in GridssElements)
                    {
                        Grid grid = gridEle as Grid;
                        if (grid != null)
                        {
                            // Get the curve of the grid
                            Curve gridCurve = grid.GetCurvesInView(DatumExtentType.ViewSpecific, viewPlan).FirstOrDefault();
                            if (gridCurve != null)
                            {
                                var Gpoint1 = new XYZ(gridCurve.GetEndPoint(0).X, gridCurve.GetEndPoint(0).Y, viewPlan.Origin.Z);
                                var Gpoint2 = new XYZ(gridCurve.GetEndPoint(1).X, gridCurve.GetEndPoint(1).Y, viewPlan.Origin.Z);
                                var NewGridLine = Line.CreateBound(Gpoint1, Gpoint2);
                                // Find the intersection points between the column curve and the grid curve
                                SetComparisonResult result = NewLineC.Intersect(NewGridLine, out IntersectionResultArray results);
                                if (result == SetComparisonResult.Overlap)
                                {
                                    foreach (IntersectionResult intersection in results)
                                    {
                                        XYZ intersectionPoint = intersection.XYZPoint;
                                        if (!intersectionPoint.Equals(NewLineC.GetEndPoint(0)) || !intersectionPoint.Equals(NewLineC.GetEndPoint(1)))
                                        {
                                            IntesectedGridName.Add(grid.Name);
                                            ColumnsIntersectionPoints.Add(intersectionPoint);
                                        }
                                        Logfile.Append($"IntersectionPoits: ({intersectionPoint.X}, {intersectionPoint.Y},{intersectionPoint.Z})");
                                    }
                                }
                            }
                        }
                    }
                    Open_StreamFiles.CreatEncryptedLogFile(LogDirectors.GetAllInstanceElementsAtSpacficViewPlan, ".log", Logfile, "Revit Elements", false);
                    return (ColumnsIntersectionPoints, IntesectedGridName);
                }
                else
                {
                    ColumnsIntersectionPoints.Add(new XYZ(0, 0, 0));
                    IntesectedGridName.Add("");
                    return (ColumnsIntersectionPoints, IntesectedGridName);
                }
            }
            else
            {
                ColumnsIntersectionPoints.Add(new XYZ(0, 0, 0));
                IntesectedGridName.Add("Null");
                return (ColumnsIntersectionPoints, IntesectedGridName);
            }
        }
        #endregion
    }

}
