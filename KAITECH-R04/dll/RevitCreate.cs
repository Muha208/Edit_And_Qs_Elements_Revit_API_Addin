using Autodesk.Revit.DB.Structure;
using Autodesk.Revit.DB;
using DLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.UI;
using KAITECH_R04;
using Autodesk.Revit.DB.Analysis;
using System.Windows.Media;
using System.Windows.Shapes;
using Line = Autodesk.Revit.DB.Line;
using System.Windows.Controls;
using Grid = Autodesk.Revit.DB.Grid;
using System.Security.Cryptography;
using Autodesk.Revit.DB.Architecture;
using System.Windows;
using FoundationRFT.Model;
using System.Windows.Media.Effects;

namespace Dll
{
    public class RevitCreate
    {
        private static Autodesk.Revit.DB.Document document = Commands.document;
        private static UIDocument uidoc = Commands.uidoc;
        private static List<ViewSheet> ListOfColumnsAxisSheetsCreated;
        private static List<ElementId> ListOfDistinctColumnsLevelId;
        public static Dictionary<string, ViewPlan> ListOfViewPalns;
        private static int N;

        public static Level CreateLevel(double Elevation, string LevelName)
        {
            Level DemoLevel = Level.Create(document, UnitUtils.ConvertToInternalUnits(Elevation, UnitTypeId.SquareMeters));
            DemoLevel.Name = LevelName;
            return DemoLevel;
        }
        public static void CreateLevels(List<double> Elevations, List<string> LevelsName)
        {
            var ElevationsList = Elevations.ToList();
            var LevelsNameList = LevelsName.ToList();
            if (Elevations.Count() <= LevelsName.Count())
            {
                for (int i = 0; i < Elevations.Count(); i++)
                {
                    Level DemoLevel = Level.Create(document, UnitUtils.ConvertToInternalUnits(ElevationsList[i], UnitTypeId.SquareMeters));
                    DemoLevel.Name = LevelsNameList[i];
                }
            }
            else if (Elevations.Count() > LevelsName.Count())
            {
                var DefOfCount = Elevations.Count() - LevelsName.Count();
                for (int i = 0; i < LevelsName.Count(); i++)
                {
                    Level DemoLevel = Level.Create(document, UnitUtils.ConvertToInternalUnits(ElevationsList[i], UnitTypeId.SquareMeters));
                    DemoLevel.Name = LevelsNameList[i];
                }
                for (int i = LevelsName.Count(); i < LevelsName.Count() + DefOfCount; i++)
                {
                    Level DemoLevel = Level.Create(document, UnitUtils.ConvertToInternalUnits(ElevationsList[i], UnitTypeId.SquareMeters));
                    DemoLevel.Name = $"Level {i}";
                }
            }
            else
            {
                System.Windows.MessageBox.Show("There is missing data");
            }
        }
        public static ViewPlan CreateViewPlane(ElementId LevelId = null, string ViewName = null)
        {
            var LevelsIDList = new List<ElementId>();
            var LevelsNameList = new List<string>();
            ElementId LevelID;
            string MehtodViewName;

            if (LevelId == null)
            {
                LevelsIDList = (List<ElementId>)RevitFilters.GetAllElementsDataByCategory(BuiltInCategory.OST_Levels, RevitFilters.ReturnElementData.ElementID, "Levels");
                LevelId = LevelsIDList.FirstOrDefault();
                LevelID = LevelId;
            }
            else if (ViewName == null)
            {
                LevelsNameList = (List<string>)RevitFilters.GetAllElementsDataByCategory(BuiltInCategory.OST_Levels, RevitFilters.ReturnElementData.ElementName, "Levels");
                ViewName = $"Level-{LevelsNameList.Count() + 1}";
                MehtodViewName = ViewName;
            }
            else if (LevelId == null && ViewName == null)
            {
                LevelId = LevelsIDList.FirstOrDefault();
                LevelID = LevelId;
                ViewName = $"Level-{LevelsNameList.Count() + 1}";
                MehtodViewName = ViewName;
            }
            else
            {
                LevelID = LevelId;
                MehtodViewName = ViewName;
            }
            var ViewFamilyTypeID = RevitElementsMethods.ViewPalnType.Id;
            var View = ViewPlan.Create(document, ViewFamilyTypeID as ElementId, LevelId as ElementId);
            View.Scale = 100;
            View.Name = ViewName;
            return View;
        }
        public static void CreateViews(List<ElementId> LevelsIDList = null, List<string> LevelsNameList = null)
        {
            var LevelsID = new List<ElementId>();
            var LevelsName = new List<string>();
            if (LevelsIDList == null)
            {
                LevelsIDList = (List<ElementId>)RevitFilters.GetAllElementsDataByCategory(BuiltInCategory.OST_Levels, RevitFilters.ReturnElementData.ElementID, "Levels");
                LevelsID = LevelsIDList;
            }
            else if (LevelsName == null)
            {
                LevelsNameList = (List<string>)RevitFilters.GetAllElementsDataByCategory(BuiltInCategory.OST_Levels, RevitFilters.ReturnElementData.ElementName, "Levels");
                LevelsName = LevelsNameList;
            }
            else if (LevelsID == null && LevelsName == null)
            {
                LevelsName = (List<string>)RevitFilters.GetAllElementsDataByCategory(BuiltInCategory.OST_Levels, RevitFilters.ReturnElementData.ElementName, "Levels");
                LevelsID = (List<ElementId>)RevitFilters.GetAllElementsDataByCategory(BuiltInCategory.OST_Levels, RevitFilters.ReturnElementData.ElementID, "Levels");
                LevelsID = LevelsIDList;
                LevelsName = LevelsNameList;
            }
            else
            {
                LevelsID = LevelsIDList;
                LevelsName = LevelsNameList;
            }
            var ViewFamilyTypeID = RevitElementsMethods.ViewPalnType.Id;
            if (LevelsName.Count() <= LevelsID.Count())
            {
                for (int i = 0; i < LevelsName.Count(); i++)
                {
                    var View = ViewPlan.Create(document, ViewFamilyTypeID, LevelsID[i]);
                    View.Name = LevelsName[i];
                }
                var DeferanceBetweenTwoLists = LevelsID.Count() - LevelsName.Count();
                for (int i = LevelsName.Count() + 1; i <= LevelsID.Count(); i++)
                {
                    var View = ViewPlan.Create(document, ViewFamilyTypeID, LevelsID[i]);
                    View.Name = $"Level {i}";
                }
            }
            else if (LevelsName.Count() > LevelsID.Count())
            {
                for (int i = 0; i < LevelsID.Count(); i++)
                {
                    var View = ViewPlan.Create(document, ViewFamilyTypeID, LevelsID[i]);
                    View.Name = LevelsName[i];
                }
            }
            else
            {
                System.Windows.MessageBox.Show("There is missing data");
            }
        }
        public static void CreateColumn()
        {
            // Get a Column type from Revit
            FilteredElementCollector collector = new FilteredElementCollector(document);
            collector.OfClass(typeof(FamilySymbol)).OfCategory(BuiltInCategory.OST_StructuralColumns);
            FamilySymbol columnType = collector.FirstElement() as FamilySymbol;

            // Create a level
            Level level = Level.Create(document, 20);

            // Create a FamilyInstance
            FamilyInstance instance;
            XYZ origin = new XYZ(0, 0, 0);
            instance = document.Create.NewFamilyInstance(origin, columnType, level, StructuralType.Column);
            instance.LookupParameter("Width").Set(200);
            instance.LookupParameter("Depth").Set(200);
            instance.LookupParameter("Height").Set(100);
        }
        public static void CreateSheets(string SheetName = "Columns Axis", string SheetNumber = "SC10")
        {
            ListOfColumnsAxisSheetsCreated = new List<ViewSheet>();
            var ListOfDistinctColumnsLevel = RevitElementsMethods.ListOfColumnsLevel;
            ListOfDistinctColumnsLevelId = ListOfDistinctColumnsLevel.ConvertAll(X => X.Id).Distinct().ToList();
            var SheetNamingList = OwneRevitMathods.GetAllSheetNaming();
            N = OwneRevitMathods.GetSheetNumbring(OwneRevitMathods.CheckSheetNaming(SheetNamingList)) + 1;
            //Get the title block family symbol for the sheet
            var TiltleBolckElementId = RevitElementsMethods.TiltleBolckElementId;
            for (int i = 0; i < ListOfDistinctColumnsLevelId.Count; i++)
            {
                ViewSheet sheet = ViewSheet.Create(document, TiltleBolckElementId);
                sheet.Name = SheetName;
                sheet.SheetNumber = SheetNumber + N;
                ListOfColumnsAxisSheetsCreated.Add(sheet);
                MainWindow.LogBox.Text = $"Sheet No: {N} - Sheet Name: {sheet.Name} \"Created Successfully\"................";
                N++;
            }
        }
        public static ViewSheet CreateSheet(Level Level, string SheetName = "Columns Axis", string SheetNumber = "SC10")
        {
            var SheetNamingList = OwneRevitMathods.GetAllSheetNaming();
            N = OwneRevitMathods.GetSheetNumbring(OwneRevitMathods.CheckSheetNaming(SheetNamingList)) + 1;
            //Get the title block family symbol for the sheet
            var TiltleBolckElementId = RevitElementsMethods.TiltleBolckElementId;
            ViewSheet sheet = ViewSheet.Create(document, TiltleBolckElementId);
            sheet.Name = SheetName;
            sheet.SheetNumber = SheetNumber + N + Level.Name;
            ListOfColumnsAxisSheetsCreated.Add(sheet);
            N++;
            return sheet;
        }
        public static void CreateSheetAccordingToColumnsLevel(Level Level, string SheetName = "Columns Axis", string SheetNumber = "SC10")
        {
            var sheet = CreateSheet(Level, SheetName, SheetNumber);
            CreateColumnsAxisView(sheet.SheetNumber, sheet, Level);
        }
        public static ViewDrafting CreateDraftingView()
        {
            var viewFamilyType = RevitFilters.GetViewTypes(ViewType.DraftingView).FirstOrDefault().Id;
            ViewDrafting viewDrafting = ViewDrafting.Create(document, viewFamilyType);
            viewDrafting.Scale = 100;
            return viewDrafting;

        }
        public static void CreateSheetsAccordingToColumnsLevel(string SheetName = "Columns Axis", string SheetNumber = "SC10")
        {
            ListOfViewPalns = new Dictionary<string, ViewPlan>();
            CreateSheets(SheetName, SheetNumber);
            MainWindow.LogBox.Text = $"All Sheets Created Successfully................";
            var i = 0;
            foreach (var sheet in ListOfColumnsAxisSheetsCreated)
            {
                var NewViewPalne = CreateColumnsAxisView(sheet.SheetNumber, sheet, document.GetElement(ListOfDistinctColumnsLevelId[i]) as Level);
                ListOfViewPalns.Add(document.GetElement(ListOfDistinctColumnsLevelId[i]).Name, NewViewPalne);
                i++;
            }
        }
        public static void PutDimensionAtViewPlanGridsOnly(ViewPlan viewPlan)
        {
            // Virtical Grids Dimension
            var VerticalGridsSeq = OwneRevitMathods.GetGridOrientation(
                        RevitElementsMethods.GridsElemints).VerticalGridsList;
            CreateGridDimensions(viewPlan, VerticalGridsSeq, OwneRevitMathods.At.Start, OwneRevitMathods.LineOri.Virtical);
            CreateGridDimensions(viewPlan, VerticalGridsSeq, OwneRevitMathods.At.End, OwneRevitMathods.LineOri.Virtical);
            MainWindow.LogBox.Text = "Virtical Grids Lines Get Dimensions Successfully!................";

            // Horizontal Grids Dimension
            var HorizontalGridsSeq = OwneRevitMathods.GetGridOrientation(
            RevitElementsMethods.GridsElemints).HorizontalGridsList;
            CreateGridDimensions(viewPlan, HorizontalGridsSeq, OwneRevitMathods.At.Start, OwneRevitMathods.LineOri.Horizontal);
            CreateGridDimensions(viewPlan, HorizontalGridsSeq, OwneRevitMathods.At.End, OwneRevitMathods.LineOri.Horizontal);
            MainWindow.LogBox.Text = "Horizontal Grids Lines Get Dimensions Successfully!................";
        }
        public static void PutDimensionAtViewPlanColumnsAxisOnly(ViewPlan viewPlan)
        {
            // Columns Dimension
            CreateColumnsGridDimensions(viewPlan);
            MainWindow.LogBox.Text = "All Column Axis View Plane Get Grids Dimesions Successfully!................";
        }
        public static void EditGrids()
        {
            var GridsElemints = RevitElementsMethods.GridsElemints;

            var MaxElevationLevel = RevitElementsMethods.LevelsList.Max(x => x.Elevation);
            var MinElevationLevel = RevitElementsMethods.LevelsList.Min(x => x.Elevation);

            foreach (Grid grid in GridsElemints)
            {
                grid.SetVerticalExtents(MinElevationLevel, MaxElevationLevel);
                MainWindow.LogBox.Text = "Grid Line Reach All Elevations Successfully!................";
            }

            if (ListOfViewPalns != null && ListOfDistinctColumnsLevelId != null)
            {
                for (int i = 0; i < ListOfViewPalns.Count; i++)
                {
                    var lev = document.GetElement(ListOfDistinctColumnsLevelId.Find(x => ListOfViewPalns.Keys.ElementAt(i).Contains(document.GetElement(x).Name))) as Level;

                    foreach (Grid grid in GridsElemints)
                    {
                        OwneRevitMathods.ExtendGridLinesLength(grid, ListOfViewPalns.Values.ElementAt(i));
                        MainWindow.LogBox.Text = "Grid Line Extended Successfully!................";
                    }
                }
            }
            else
            {
                ListOfViewPalns = new Dictionary<string, ViewPlan>();
                var SIndex = MainWindow.ViewCoboBox.SelectedIndex;
                var LevelS = RevitElementsMethods.FloorEle.ElementAt(SIndex);
                RevitCreate.ListOfViewPalns.Add(LevelS.Name, LevelS as ViewPlan);

                for (int i = 0; i < ListOfViewPalns.Count; i++)
                {
                    foreach (Grid grid in GridsElemints)
                    {
                        OwneRevitMathods.ExtendGridLinesLength(grid, ListOfViewPalns.Values.ElementAt(i));
                        MainWindow.LogBox.Text = "Grid Line Extended Successfully!................";
                    }
                }
            }
        }
        public static ViewPlan CreateColumnsAxisView(string SheetNumber, ViewSheet Sheet, Level Level, double ViewPortOffset = 1)
        {
            //get ViewPalnType Of View
            var ViewPalnType = RevitElementsMethods.ViewPalnType;
            //get Levels ID
            var LevelsElementId = Level.Id;
            //create columns view
            var ColumnsView = ViewPlan.Create(document, ViewPalnType.Id, LevelsElementId);
            ColumnsView.DetailLevel = ViewDetailLevel.Fine;
            ColumnsView.DisplayStyle = DisplayStyle.ShadingWithEdges;
            ColumnsView.Discipline = ViewDiscipline.Structural;
            #region Hide Elements
            //get All STR elements And Hide
            var StairsList = RevitElementsMethods.StairsList;
            var FoundationsList = RevitElementsMethods.FoundationsList;
            var WallsList = RevitElementsMethods.WallsList;
            var FloorsList = RevitElementsMethods.FloorsList;
            var FramingList = RevitElementsMethods.FramingList;
            var RampsList = RevitElementsMethods.RampsList;
            var SectionsList = RevitElementsMethods.SectionsList;
            var ElevationsList = RevitElementsMethods.ElevationsList;
            var ImportInstanceList = RevitElementsMethods.ImportInstanceList;
            var GenereicModels = RevitElementsMethods.GenereicModels;
            var ViewFamilesEleIdList = RevitElementsMethods.ViewFamilesEleIdList;
            var SectinsLIstInViewId = ColumnsView.GetReferenceSections();

            if (SectinsLIstInViewId.Count > 0)
            {
                ColumnsView.HideElements(SectinsLIstInViewId);
            }
            if (ViewFamilesEleIdList.Count > 0)
            {
                ColumnsView.HideElements(ViewFamilesEleIdList);
            }
            if (GenereicModels.Count > 0)
            {
                ColumnsView.HideElements(GenereicModels);
            }
            if (StairsList.Count > 0)
            {
                ColumnsView.HideElements(StairsList);
            }
            if (FoundationsList.Count > 0)
            {
                ColumnsView.HideElements(FoundationsList);
            }
            if (WallsList.Count > 0)
            {
                ColumnsView.HideElements(WallsList);
            }
            if (FloorsList.Count > 0)
            {
                ColumnsView.HideElements(FloorsList);
            }
            if (FramingList.Count > 0)
            {
                ColumnsView.HideElements(FramingList);
            }
            if (RampsList.Count > 0)
            {
                ColumnsView.HideElements(RampsList);
            }
            if (SectionsList.Count > 0)
            {
                SectionsList.Remove(ColumnsView.Id);
                ColumnsView.HideElements(SectionsList);
            }
            if (ElevationsList.Count > 0)
            {
                ColumnsView.HideElements(ElevationsList);
            }
            if (ImportInstanceList.Count > 0)
            {
                ColumnsView.HideElements(ImportInstanceList);
            }

            // Get the category for datum planes
            Category datumPlaneCategory = Category.GetCategory(document, BuiltInCategory.OST_CLines);
            // Set the visibility of the datum plane category in the view
            ColumnsView.SetCategoryHidden(datumPlaneCategory.Id, false);
            #endregion
            //Columns 
            var ColumsList = RevitElementsMethods.ColumsList;
            //set View Name And Scale
            ColumnsView.Name = $"{SheetNumber}-{Level.Name}-Columns Axis";
            MainWindow.LogBox.Text = $"View Name: {ColumnsView.Name} \"Created Successfully\"................";
            //Get list of all grids
            List<Grid> GridsElemints = RevitElementsMethods.GridsElemints.Cast<Grid>().ToList();
            //Get list of all Columns Tags
            var ColumnsTags = RevitElementsMethods.ColumnsTags;
            //Set crop box of the view

            //set the visibilty of category of view
            //Columns

            // Define a FilteredElementCollector for the DatumPlane element type
            var datumPlaneCollector = new FilteredElementCollector(document).OfClass(typeof(DatumPlane)).ToElements();
            var ViewTemp = ColumnsView.CreateViewTemplate();

            foreach (var eleID in datumPlaneCollector)
            {
                OverrideGraphicSettings overrideSettings = ColumnsView.GetElementOverrides(eleID.Id);
                overrideSettings.SetProjectionLineColor(new Autodesk.Revit.DB.Color(255, 0, 0)); // Set color to red, for example
                overrideSettings.SetProjectionLineWeight(5); // Set line weight to 5, for example
                ColumnsView.SetElementOverrides(eleID.Id, overrideSettings);
            }

            ColumnsView.SetCategoryHidden(ColumsList.FirstOrDefault().Category.Id, false);
            ColumnsView.SetCategoryHidden(GridsElemints.FirstOrDefault().Category.Id, false);
            //Columns Tags
            foreach (var Cele in ColumnsTags)
            {
                ColumnsView.SetCategoryHidden(Cele.Category.Id, false);
            }
            //Add View To sheet
            XYZ oriPoints = Sheet.Origin;
            XYZ OffsetPoints = new XYZ(oriPoints.X + 1.3 * ViewPortOffset, oriPoints.Y + ViewPortOffset, oriPoints.Z);

            Viewport viewport = Viewport.Create(document, Sheet.Id, ColumnsView.Id, OffsetPoints);
            MainWindow.LogBox.Text = $"Sheet Name: {Sheet.Name} Get ViewPort Name: {viewport.Name} Successfully!................";
            //set ViewportType to Schedule
            return ColumnsView;
        }
        /// <summary>
        /// Create Dimentions Between Grid Lines
        /// </summary>
        /// <param name="viewPlan">Thw ViewPlane That The Dimension Will Created Into</param>
        /// <param name="ListOfGrid">List Of Grids Line (Horizontal Or Virtical Only)</param>
        /// <param name="DimensionAt">Thats According The Grids Oriantaion (Horizontal Or Virtical)</param>
        /// <param name="lineOri">Horizontal Or Virtical Of Grids Lines</param>
        public static void CreateGridDimensions(ViewPlan viewPlan, List<Grid> ListOfGrid, OwneRevitMathods.At DimensionAt, OwneRevitMathods.LineOri lineOri)
        {
            // Create a new DimensionType object for the grid dimensions
            var DimType = CreateDimensionType(DimensionStyleType.Linear);

            // Create a new GridDimension
            var GridDimensionRef = OwneRevitMathods.GetRefrencesArrayAndDimLinesBetweenGrids(ListOfGrid, DimensionAt, lineOri).ReferenceArrayList;
            var GridDimensionLines = OwneRevitMathods.GetRefrencesArrayAndDimLinesBetweenGrids(ListOfGrid, DimensionAt, lineOri).LinesList;
            for (int i = 0; i < GridDimensionRef.Count; i++)
            {
                Dimension GridDimension = document.Create.NewDimension(viewPlan, GridDimensionLines[i], GridDimensionRef[i]);
            }
        }
        public static DimensionType CreateDimensionType(DimensionStyleType dimensionStyleType)
        {
            var dimensionType = new FilteredElementCollector(document)
                               .OfClass(typeof(DimensionType))
                               .Cast<DimensionType>();


            var qDimTypes = from dt in dimensionType
                            where dt.StyleType == dimensionStyleType
                            & dt.GetSimilarTypes().Count > 0
                            orderby dt.Name
                            select dt;
            return qDimTypes.FirstOrDefault();
        }
        public static void CreateColumnsGridDimensions(ViewPlan viewPlan)
        {
            foreach (var cat in OwneRevitMathods.InformationClass.CategoryInformation)
            {
                if (cat.CategoriesName == "Structural Columns")
                {
                    foreach (var ele in cat.Elements)
                    {
                        if (viewPlan.Name.Contains(ele.ELementsParameters.ElementViewPlane.Name))
                        {
                            CreateDimesionsCurves(ele, ele.ELementsParameters.ElementCurvesList, viewPlan);
                        }
                    }
                }
            }

        }
        public static void CreateDimesionsCurves(MElements ele, List<Curve> ListOdCurves, ViewPlan viewPlan, double Offest = 3)
        {
            if (ListOdCurves.Count > 0 && ListOdCurves != null)
            {
                foreach (var curve in ListOdCurves)
                {
                    //var StartPoint = curve.Evaluate(0, true);
                    //double StartOffsetPointy = StartPoint.Y;
                    //double StartOffsetPointX = StartPoint.X;
                    //var EndPoint = curve.Evaluate(1, true);
                    //double EndOffsetPointy = EndPoint.Y;
                    //double EndOffsetPointX = EndPoint.X;
                    //XYZ OffsetStartPoint;
                    //XYZ OffsetEndPoint;
                    //if (curve.ComputeDerivatives(.5, true).BasisX.X != 0 && curve.ComputeDerivatives(.5, true).BasisX.Y == 0)
                    //{
                    //    if (StartPoint.Y >= 0)
                    //    {
                    //        StartOffsetPointy = StartPoint.Y + Offest;
                    //        EndOffsetPointy = EndPoint.Y + Offest;
                    //    }
                    //    else
                    //    {
                    //        StartOffsetPointy = StartPoint.Y - Offest;
                    //        EndOffsetPointy = EndPoint.Y - Offest;
                    //    }
                    //}
                    //else if (curve.ComputeDerivatives(.5, true).BasisX.Y != 0 && curve.ComputeDerivatives(.5, true).BasisX.X == 0)
                    //{
                    //    if (StartPoint.X >= 0)
                    //    {
                    //        StartOffsetPointX = StartPoint.X + Offest;
                    //        EndOffsetPointX = EndPoint.X + Offest;
                    //    }
                    //    else
                    //    {
                    //        StartOffsetPointX = StartPoint.X - Offest;
                    //        EndOffsetPointX = EndPoint.X - Offest;
                    //    }
                    //}
                    //else
                    //{
                    //    var SP = StartPoint * Offest;
                    //    var Ep = EndPoint * Offest;
                    //    StartOffsetPointX = SP.X;
                    //    StartOffsetPointy = SP.Y;
                    //    EndOffsetPointX = Ep.X;
                    //    EndOffsetPointy = Ep.Y;

                    //}
                    //OffsetStartPoint = new XYZ(StartOffsetPointX, StartOffsetPointy, viewPlan.Origin.Z);
                    //OffsetEndPoint = new XYZ(EndOffsetPointX, EndOffsetPointy, viewPlan.Origin.Z);
                    CreateDimensionBetweenTwoPoints(viewPlan, curve, ele);
                }
            }
        }
        public static List<XYZ> GetOffsetPointsOnCurve(Curve curve, List<XYZ> PointsList, ViewPlan viewPlan, double Offest)
        {
            var OffsetPointsList = new List<XYZ>();
            if (PointsList.Count > 0)
            {
                double OffsetPointy;
                double OffsetPointX;
                if (curve.ComputeDerivatives(.5, true).BasisX.X != 0 && curve.ComputeDerivatives(.5, true).BasisX.Y == 0)
                {
                    PointsList.OrderBy(x => x.X);
                    for (int i = 0; i < PointsList.Count; i++)
                    {

                        var Point = PointsList[i];
                        OffsetPointy = Point.Y;

                        if (Point.Y >= 0)
                        {
                            OffsetPointy = Point.Y + Offest;
                        }
                        else
                        {
                            OffsetPointy = Point.Y - Offest;
                        }
                        OffsetPointsList.Add(new XYZ(Point.X, OffsetPointy, viewPlan.Origin.Z));
                    }
                }
                else if (curve.ComputeDerivatives(.5, true).BasisX.Y != 0 && curve.ComputeDerivatives(.5, true).BasisX.X == 0)
                {
                    PointsList.OrderBy(x => x.Y);
                    for (int i = 0; i < PointsList.Count; i++)
                    {

                        var Point = PointsList[i];
                        OffsetPointX = Point.X;
                        if (Point.X >= 0)
                        {
                            OffsetPointX = Point.X + Offest;
                        }
                        else
                        {
                            OffsetPointX = Point.X - Offest;
                        }
                        OffsetPointsList.Add(new XYZ(OffsetPointX, Point.Y, viewPlan.Origin.Z));
                    }
                }
                else
                {

                    for (int i = 0; i < PointsList.Count; i++)
                    {

                        var Point = PointsList[i];
                        OffsetPointy = Point.Y;
                        OffsetPointX = Point.X;
                        Offest /= 1.5;
                        if (Point.Y >= 0)
                        {
                            OffsetPointy = Point.Y + Offest;
                        }
                        else
                        {
                            OffsetPointy = Point.Y - Offest;
                        }
                        if (Point.X >= 0)
                        {
                            OffsetPointX = Point.X + Offest;
                        }
                        else
                        {
                            OffsetPointX = Point.X - Offest;
                        }
                        OffsetPointsList.Add(new XYZ(OffsetPointX, OffsetPointy, viewPlan.Origin.Z));
                    }
                }
            }
            return OffsetPointsList;
        }

        public static XYZ GetOffsetDimPointsOnCurve(Line curve, ViewPlan viewPlan, double Offest)
        {
            double OffsetPoint;
            double OffsetPointy;
            double OffsetPointX;
            if (curve.ComputeDerivatives(.5, true).BasisX.X != 0 && curve.ComputeDerivatives(.5, true).BasisX.Y == 0)
            {
                var Point = curve.ComputeDerivatives(.5, true).Origin;

                if (curve.Direction.Y >= 0)
                {
                    if (Point.Y >= 0)
                    {
                        OffsetPointy = Point.Y + Offest;
                    }
                    else
                    {
                        OffsetPointy = Point.Y - Offest;
                    }
                }
                else
                {
                    if (Point.Y >= 0)
                    {
                        OffsetPointy = Point.Y + Offest;
                    }
                    else
                    {
                        OffsetPointy = Point.Y - Offest;
                    }
                }
                return new XYZ(Point.X, OffsetPointy, viewPlan.Origin.Z);

            }
            else if (curve.ComputeDerivatives(.5, true).BasisX.Y != 0 && curve.ComputeDerivatives(.5, true).BasisX.X == 0)
            {

                var Point = curve.ComputeDerivatives(.5, true).Origin;
                OffsetPointX = Point.X;

                if (curve.Direction.X >= 0)
                {
                    if (Point.X >= 0)
                    {
                        OffsetPointX = Point.X + Offest;
                    }
                    else
                    {
                        OffsetPointX = Point.X - Offest;
                    }
                }
                else
                {
                    if (Point.X >= 0)
                    {
                        OffsetPointX = Point.X + Offest;
                    }
                    else
                    {
                        OffsetPointX = Point.X - Offest;
                    }
                }
                return new XYZ(OffsetPointX, Point.Y, viewPlan.Origin.Z);

            }
            else
            {


                var Point = curve.ComputeDerivatives(.5, true).Origin;
                OffsetPointy = Point.Y;
                OffsetPointX = Point.X;
                Offest /= 1.5;
                if (curve.Direction.Y >= 0)
                {
                    if (Point.Y >= 0)
                    {
                        OffsetPointy = Point.Y + Offest;
                    }
                    else
                    {
                        OffsetPointy = Point.Y - Offest;
                    }
                }
                else
                {
                    if (Point.Y >= 0)
                    {
                        OffsetPointy = Point.Y + Offest;
                    }
                    else
                    {
                        OffsetPointy = Point.Y - Offest;
                    }
                }

                if (curve.Direction.X >= 0)
                {
                    if (Point.X >= 0)
                    {
                        OffsetPointX = Point.X + Offest;
                    }
                    else
                    {
                        OffsetPointX = Point.X - Offest;
                    }
                }
                else
                {
                    if (Point.X >= 0)
                    {
                        OffsetPointX = Point.X + Offest;
                    }
                    else
                    {
                        OffsetPointX = Point.X - Offest;
                    }
                }
                return new XYZ(OffsetPointX, OffsetPointy, viewPlan.Origin.Z);

            }

        }

        public static void CreateDimensionBetweenTwoPoints(ViewPlan viewPlan, Curve curve, MElements ele)
        {
            //First Line
            var ViewSketchPlan = viewPlan.SketchPlane;

            var CStartPoint = new XYZ(curve.GetEndPoint(0).X, curve.GetEndPoint(0).Y, viewPlan.Origin.Z);
            var CEndPoint = new XYZ(curve.GetEndPoint(1).X, curve.GetEndPoint(1).Y, viewPlan.Origin.Z);

            var NewLine = Line.CreateBound(CStartPoint, CEndPoint);

            //ModelCurve modelCurve = document.Create.NewModelCurve(NewLine, ViewSketchPlan);

            //ReArray.Append(modelCurve.GeometryCurve.GetEndPointReference(0));
            //ReArray.Append(modelCurve.GeometryCurve.GetEndPointReference(1));

            var intersectionPionts = RevitElementsMethods.GetTheIntersectionPointsBetweenTowCurves(ele, NewLine, viewPlan).IntersectionsPoints;
            var AllPoints = new List<XYZ>();
            AllPoints.Add(CStartPoint);
            if (intersectionPionts.Count > 3)
            {
                AllPoints.AddRange(intersectionPionts);
            }
            AllPoints.Add(CEndPoint);

            var AllOffsetPoints = GetOffsetPointsOnCurve(curve, AllPoints, viewPlan, 0);

            var DesList = new List<Line>();
            for (int i = 0; i < AllOffsetPoints.Count; i++)
            {
                if (i != AllOffsetPoints.Count - 1)
                {
                    if (AllOffsetPoints[i].DistanceTo(AllOffsetPoints[i + 1]) < .05 && i < AllOffsetPoints.Count - 2)
                    {
                        DesList.Add(Line.CreateBound(AllOffsetPoints[i], AllOffsetPoints[i + 2]));
                        i++;
                    }
                    else
                    {
                        DesList.Add(Line.CreateBound(AllOffsetPoints[i], AllOffsetPoints[i + 1]));
                    }
                }
            }

            foreach (var Dline in DesList)
            {
                ModelCurve modelCurve = document.Create.NewModelCurve(Dline, ViewSketchPlan);
                var ReArray = new ReferenceArray();
                ReArray.Append(modelCurve.GeometryCurve.GetEndPointReference(0));
                ReArray.Append(modelCurve.GeometryCurve.GetEndPointReference(1));

                if (Dline.Length >= .01)
                {
                    var Dim = document.Create.NewDimension(viewPlan, Dline, ReArray);
                    Dim.LeaderEndPosition = GetOffsetDimPointsOnCurve(Dline,viewPlan,3);
                    }
            }

            //for (int i = 0; i < DesList.Count; i++)
            //{
            //    if (i == 0)
            //    {
            //        var DimNewLine = Line.CreateBound(AllPoints[0], AllPoints[i]);
            //        document.Create.NewDimension(viewPlan, DimNewLine, ReArray);
            //    }
            //    else if (i == AllPoints.Count - 1)
            //    {

            //    }
            //    else
            //    {
            //        var DimNewLine = Line.CreateBound(AllPoints[i], AllPoints[i + 1]);
            //        document.Create.NewDimension(viewPlan, DimNewLine, ReArray);
            //    }
            //}
        }
    }

}
