using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Structure;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using Dll;
using FoundationRFT.Model;
using KAITECH_R04;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Xml.Linq;
using static Dll.RevitCreate;
using Grid = Autodesk.Revit.DB.Grid;

namespace DLL
{
    public static class OwneRevitMathods
    {
        #region Parameters
        private static Autodesk.Revit.DB.Document document = Commands.document;
        private static UIDocument uidoc = Commands.uidoc;

        public static Informations InformationClass;

        public static List<Element> ListOfSelectedElements;
        public static string SelectAllString = "All Elements";
        public static List<string> FilterSelectionList;
        public static List<string> ListOfSelectedElementsFilteredParameter = new List<string>();
        private static int SN = 1;
        #endregion
        #region Enums
        public enum At
        {
            End,
            Start
        }
        public enum LineOri
        {
            Virtical,
            Horizontal
        }
        #endregion
        //_________________________________________________________________________________________________________//
        #region Sheets Methods
        /// <summary>
        /// Get The All Sheet Naming in The Project 
        /// </summary>
        /// <returns></returns>
        public static List<string> GetAllSheetNaming()
        {
            var listOfTitleBlockName = new List<string>();
            var ListOfTitleBlock = RevitFilters.GetAllSymbolElements(BuiltInCategory.OST_TitleBlocks);
            foreach (var item in ListOfTitleBlock)
            {
                foreach (Parameter para in item.Parameters)
                {
                    switch (para.StorageType)
                    {
                        case StorageType.None:
                            break;
                        case StorageType.Integer:
                            break;
                        case StorageType.Double:
                            break;
                        case StorageType.String:
                            if (para.Definition.Name == "Sheet Number")
                            {
                                listOfTitleBlockName.Add(para.AsValueString());
                            }
                            break;
                        case StorageType.ElementId:
                            break;
                        default:
                            break;
                    }
                }
            }
            return listOfTitleBlockName;
        }
        /// <summary>
        /// Check with a list of sheets naming,
        /// Check Every sheet naming That has the same Sheet Name (Input Name) to get the serial number of the next one
        /// </summary>
        /// <param name="listOfTitleBlockName">Naming Sheets List</param>
        /// <param name="CheckCondtion">Name To Check</param>
        /// <returns></returns>
        public static List<bool> CheckSheetNaming(List<string> listOfTitleBlockName, string CheckCondtion = "SC10")
        {
            var CheckList = new List<bool>();
            var CheckCondtionList = CheckCondtion.ToList();
            foreach (var name in listOfTitleBlockName)
            {
                bool CheckChar = false;
                List<char> CharList = name.ToList();
                for (int i = 0; i < 4; i++)
                {
                    if (CharList[i] == CheckCondtionList[i])
                    {
                        CheckChar = true;
                    }
                    else
                    {
                        CheckChar = false;
                        break;
                    }
                }
                CheckList.Add(CheckChar);
            }
            return CheckList;
        }
        /// <summary>
        /// Get the total number of sheets that have the same im=nout name
        /// </summary>
        /// <param name="CheckList"></param>
        /// <returns></returns>
        public static int GetSheetNumbring(List<bool> CheckList)
        {
            int TrueNamingNumbering = 0;
            foreach (var item in CheckList)
            {
                if (item == true)
                {
                    TrueNamingNumbering++;
                }
            }
            return TrueNamingNumbering;
        }
        #endregion
        //---------------------------------------------------------------------------------------------------------//
        #region Grids Methods
        public static void ExtendGridLinesLength(Grid grid, ViewPlan viewPlan)
        {
            var Tor = .05;
            grid.Pinned = false;
            grid.ShowBubbleInView(DatumEnds.End1, viewPlan);
            grid.ShowBubbleInView(DatumEnds.End0, viewPlan);
            var CurvGrid = grid.GetCurvesInView(DatumExtentType.ViewSpecific, viewPlan).FirstOrDefault();

            if (CurvGrid.ComputeDerivatives(.5, true).BasisX.X == 0)
            {
                //Get Max And Min Poit form All grids As Datum
                var StartY = RevitElementsMethods.StartY;
                var EndY = RevitElementsMethods.EndY;
                //Get Grid Points
                var GStart = CurvGrid.GetEndPoint(0);
                var GEnd = CurvGrid.GetEndPoint(1);

                if (Math.Abs(Math.Round(StartY, 3) - Math.Round(GStart.Y, 3)) >= Tor || Math.Abs(Math.Round(EndY, 3) - Math.Round(GStart.Y, 3)) >= Tor ||
                    Math.Abs(Math.Round(StartY, 3) - Math.Round(GEnd.Y, 3)) >= Tor || Math.Abs(Math.Round(EndY, 3) - Math.Round(GEnd.Y, 3)) >= Tor)
                {
                    //Get the Closest Distance Between Grid and Datum points
                    var ClosestStart = Math.Abs(GStart.Y - StartY);
                    var ClosestStart2 = Math.Abs(GStart.Y - EndY);
                    if (ClosestStart < ClosestStart2)
                    {
                        var NewStart = new XYZ(GStart.X, StartY, GStart.Z);
                        var ClosestEnd1 = Math.Abs(GEnd.Y - StartY);
                        var ClosestEnd2 = Math.Abs(GEnd.Y - EndY);
                        XYZ NewEnd;
                        if (ClosestEnd1 < ClosestEnd2)
                        {
                            NewEnd = new XYZ(GEnd.X, StartY, GEnd.Z);
                        }
                        else
                        {
                            NewEnd = new XYZ(GEnd.X, EndY, GEnd.Z);
                        }

                        if (Math.Abs(NewStart.Y - NewEnd.Y) >= Tor)
                        {
                            var NewCurve = Line.CreateBound(NewStart, NewEnd);
                            grid.SetCurveInView(DatumExtentType.ViewSpecific, viewPlan, NewCurve);
                        }
                    }
                    else if (ClosestStart > ClosestStart2)
                    {
                        XYZ NewStart;
                        var ClosestEnd1 = Math.Abs(GEnd.Y - StartY);
                        var ClosestEnd2 = Math.Abs(GEnd.Y - EndY);
                        XYZ NewEnd;
                        if (ClosestEnd1 < ClosestEnd2)
                        {
                            NewEnd = new XYZ(GEnd.X, StartY, GEnd.Z);
                            NewStart = new XYZ(GStart.X, EndY, GStart.Z);
                        }
                        else
                        {
                            NewEnd = new XYZ(GEnd.X, EndY, GEnd.Z);
                            NewStart = new XYZ(GStart.X, StartY, GStart.Z);
                        }

                        if (Math.Abs(NewStart.Y - NewEnd.Y) >= Tor)
                        {
                            var NewCurve = Line.CreateBound(NewStart, NewEnd);
                            grid.SetCurveInView(DatumExtentType.ViewSpecific, viewPlan, NewCurve);
                        }
                    }
                    else
                    {
                        var ClosestEnd1 = Math.Abs(GEnd.Y - StartY);
                        var ClosestEnd2 = Math.Abs(GEnd.Y - EndY);
                        XYZ NewEnd;
                        XYZ NewStart;
                        if (ClosestEnd1 < ClosestEnd2)
                        {
                            NewEnd = new XYZ(GEnd.X, StartY, GEnd.Z);
                            NewStart = new XYZ(GStart.X, EndY, GStart.Z);
                        }
                        else
                        {
                            NewEnd = new XYZ(GEnd.X, EndY, GEnd.Z);
                            NewStart = new XYZ(GStart.X, StartY, GStart.Z);
                        }
                        if (Math.Abs(NewStart.Y - NewEnd.Y) >= Tor)
                        {
                            var NewCurve = Line.CreateBound(NewStart, NewEnd);
                            grid.SetCurveInView(DatumExtentType.ViewSpecific, viewPlan, NewCurve);
                        }
                    }
                }
            }
            else if (grid.Curve.ComputeDerivatives(.5, true).BasisX.Y == 0)
            {
                var StartX = RevitElementsMethods.StartX;
                var EndX = RevitElementsMethods.EndX;

                var GStart = CurvGrid.GetEndPoint(0);
                var GEnd = CurvGrid .GetEndPoint(1);

                if (Math.Abs(Math.Round(StartX, 3) - Math.Round(GStart.X, 3)) >= Tor || Math.Abs(Math.Round(EndX, 3) - Math.Round(GStart.X, 3)) >= Tor ||
                    Math.Abs(Math.Round(StartX, 3) - Math.Round(GEnd.X, 3)) >= Tor || Math.Abs(Math.Round(EndX, 3) - Math.Round(GEnd.X, 3)) >= Tor)
                {
                    var ClosestStart = Math.Abs(GStart.X - StartX);
                    var ClosestStart2 = Math.Abs(GStart.X - EndX);
                    if (ClosestStart < ClosestStart2)
                    {
                        var NewStart = new XYZ(StartX, GStart.Y, GStart.Z);
                        var ClosestEnd1 = Math.Abs(GEnd.X - StartX);
                        var ClosestEnd2 = Math.Abs(GEnd.X - EndX);
                        XYZ NewEnd;
                        if (ClosestEnd1 < ClosestEnd2)
                        {
                            NewEnd = new XYZ(StartX, GEnd.Y, GEnd.Z);
                        }
                        else
                        {
                            NewEnd = new XYZ(EndX, GEnd.Y, GEnd.Z);
                        }
                        if (Math.Abs(NewStart.X - NewEnd.X) >= Tor)
                        {
                            var NewCurve = Line.CreateBound(NewStart, NewEnd);
                            grid.SetCurveInView(DatumExtentType.ViewSpecific, viewPlan, NewCurve);
                        }
                    }
                    else if (ClosestStart > ClosestStart2)
                    {
                        XYZ NewStart;
                        var ClosestEnd1 = Math.Abs(GEnd.X - StartX);
                        var ClosestEnd2 = Math.Abs(GEnd.X - EndX);
                        XYZ NewEnd;
                        if (ClosestEnd1 < ClosestEnd2)
                        {
                            NewEnd = new XYZ(StartX, GEnd.Y, GEnd.Z);
                            NewStart = new XYZ(EndX, GStart.Y, GStart.Z);
                        }
                        else
                        {
                            NewEnd = new XYZ(EndX, GEnd.Y, GEnd.Z);
                            NewStart = new XYZ(StartX, GStart.Y, GStart.Z);
                        }

                        if (Math.Abs(NewStart.X - NewEnd.X) >= Tor)
                        {
                            var NewCurve = Line.CreateBound(NewStart, NewEnd);
                            var PCurve = NewCurve.Project(viewPlan.Origin);
                            var d = PCurve.Distance;
                            grid.SetCurveInView(DatumExtentType.ViewSpecific, viewPlan, NewCurve);
                        }
                    }
                    else
                    {
                        XYZ NewStart;
                        var ClosestEnd1 = Math.Abs(GEnd.X - StartX);
                        var ClosestEnd2 = Math.Abs(GEnd.X - EndX);
                        XYZ NewEnd;
                        if (ClosestEnd1 < ClosestEnd2)
                        {
                            NewEnd = new XYZ(StartX, GEnd.Y, GEnd.Z);
                            NewStart = new XYZ(EndX, GStart.Y, GStart.Z);
                        }
                        else
                        {
                            NewEnd = new XYZ(EndX, GEnd.Y, GEnd.Z);
                            NewStart = new XYZ(StartX, GStart.Y, GStart.Z);
                        }

                        if (Math.Abs(NewStart.X - NewEnd.X) >= Tor)
                        {
                            var NewCurve = Line.CreateBound(NewStart, NewEnd);
                            grid.SetCurveInView(DatumExtentType.ViewSpecific, viewPlan, NewCurve);
                        }
                    }
                }
            }
        }

        public static (List<Grid> VerticalGridsList, List<Grid> HorizontalGridsList, List<Grid> DiagonalGridsList)
        GetGridOrientation(List<Grid> GridsList)
        {
            var VerticalGridsList = new List<Grid>();
            var HorizontalGridsList = new List<Grid>();
            var DiagonalGridsList = new List<Grid>();

            if (GridsList != null)
            {
                foreach (var grid in GridsList)
                {
                    // Get the curve of the Grid
                    Curve curve = grid.Curve;

                    // Get the start and end points of the curve
                    XYZ startPoint = curve.GetEndPoint(0);
                    XYZ endPoint = curve.GetEndPoint(1);

                    // Calculate the vector between the start and end points
                    XYZ vector = endPoint - startPoint;

                    // Determine the orientation based on the vector direction
                    if (vector.Y == 0 && vector.Z == 0)
                    {
                        HorizontalGridsList.Add(grid);
                    }
                    else if (vector.X == 0 && vector.Z == 0)
                    {
                        VerticalGridsList.Add(grid);
                    }
                    else
                    {
                        DiagonalGridsList.Add(grid);
                    }
                }
            }
            return (VerticalGridsList, HorizontalGridsList, DiagonalGridsList);
        }
        #endregion
        //---------------------------------------------------------------------------------------------------------//
        #region Dimension Methods
        /// <summary>
        /// Return The ReferenceArray (Line That Indicate The Dimension Line Will Be From Where To Where)
        /// Or
        /// Return The Line (Dimension Line Created)
        /// </summary>
        /// <param name="GridList">List Of Grids</param>
        /// <param name="GetItAt">To Get Dimension At End Or Strart Of Grid Line</param>
        /// <param name="line">Get The Horizontal Or Virtical Dimension</param>
        /// <param name="Offset">Offset Between End Of Grid Line And The Dimension Line</param>
        /// <returns></returns>
        public static (List<ReferenceArray> ReferenceArrayList, List<Line> LinesList)
            GetRefrencesArrayAndDimLinesBetweenGrids(List<Grid> GridList, At GetItAt, LineOri line, double Offset = 5)
        {
            var ListOfRefArray = new List<ReferenceArray>();
            var LinesList = new List<Line>();

            double PointAtEndLineAVGX = 0;
            double PointAtEndLineAVGY = 0;

            GridList.OrderBy(x => x.Curve.GetEndPoint(1));

            for (int i = 0; i < GridList.Count; i++)
            {
                if (i != GridList.Count - 1)
                {
                    if (GridList[i + 1] != null)
                    {
                        XYZ PointAtEndLine1Max = GridList[i].Curve.GetEndPoint(1);
                        XYZ PointAtEndLine2Max = GridList[i + 1].Curve.GetEndPoint(1);

                        XYZ PointAtEndLine1Min = GridList[i].Curve.GetEndPoint(0);
                        XYZ PointAtEndLine2Min = GridList[i + 1].Curve.GetEndPoint(0);

                        XYZ PointAtEndLine1;
                        XYZ PointAtEndLine2;
                        switch (GetItAt)
                        {
                            case At.End:
                                PointAtEndLine1 = PointAtEndLine1Max;
                                PointAtEndLine2 = PointAtEndLine2Max;
                                break;
                            default:
                                PointAtEndLine1 = PointAtEndLine1Min;
                                PointAtEndLine2 = PointAtEndLine2Min;
                                break;
                        }

                        // Create a new dimension line End
                        XYZ OffsetPoint1;
                        XYZ OffsetPoint2;
                        switch (line)
                        {
                            case LineOri.Horizontal:
                                if (i == 0)
                                {
                                    double OffsetPointX1;
                                    double OffsetPointX2;
                                    if (PointAtEndLine1.X == Math.Abs(PointAtEndLine1.X))
                                    {
                                        OffsetPointX1 = PointAtEndLine1.X + Offset;
                                        if (OffsetPointX1 >= PointAtEndLine1Max.X)
                                        {
                                            OffsetPointX1 = PointAtEndLine1.X - Offset;
                                        }
                                    }
                                    else
                                    {
                                        OffsetPointX1 = PointAtEndLine1.X - Offset;
                                        if (OffsetPointX1 <= PointAtEndLine1Min.X)
                                        {
                                            OffsetPointX1 = PointAtEndLine1.X + Offset;
                                        }
                                    }
                                    if (PointAtEndLine2.X == Math.Abs(PointAtEndLine2.X))
                                    {
                                        OffsetPointX2 = PointAtEndLine2.X + Offset;
                                        if (OffsetPointX2 >= PointAtEndLine2Max.X)
                                        {
                                            OffsetPointX2 = PointAtEndLine2.X - Offset;
                                        }
                                    }
                                    else
                                    {
                                        OffsetPointX2 = PointAtEndLine2.X - Offset;
                                        if (OffsetPointX2 <= PointAtEndLine2Min.X)
                                        {
                                            OffsetPointX2 = PointAtEndLine2.X + Offset;
                                        }
                                    }
                                    PointAtEndLineAVGX = Math.Min(OffsetPointX1, OffsetPointX2);
                                }
                                OffsetPoint1 = new XYZ(PointAtEndLineAVGX, PointAtEndLine1.Y, PointAtEndLine1.Z);
                                OffsetPoint2 = new XYZ(PointAtEndLineAVGX, PointAtEndLine2.Y, PointAtEndLine2.Z);
                                break;
                            default:
                                double OffsetPointY1;
                                double OffsetPointY2;
                                if (i == 0)
                                {
                                    if (PointAtEndLine1.Y == Math.Abs(PointAtEndLine1.Y))
                                    {
                                        OffsetPointY1 = PointAtEndLine1.Y + Offset;
                                        if (OffsetPointY1 >= PointAtEndLine1Max.Y)
                                        {
                                            OffsetPointY1 = PointAtEndLine1.Y - Offset;
                                        }
                                    }
                                    else
                                    {
                                        OffsetPointY1 = PointAtEndLine1.Y - Offset;
                                        if (OffsetPointY1 <= PointAtEndLine1Min.Y)
                                        {
                                            OffsetPointY1 = PointAtEndLine1.Y + Offset;
                                        }
                                    }
                                    if (PointAtEndLine2.Y == Math.Abs(PointAtEndLine2.Y))
                                    {
                                        OffsetPointY2 = PointAtEndLine2.Y + Offset;
                                        if (OffsetPointY2 >= PointAtEndLine2Max.Y)
                                        {
                                            OffsetPointY2 = PointAtEndLine2.Y - Offset;
                                        }
                                    }
                                    else
                                    {
                                        OffsetPointY2 = PointAtEndLine2.Y - Offset;
                                        if (OffsetPointY2 <= PointAtEndLine2Min.Y)
                                        {
                                            OffsetPointY2 = PointAtEndLine2.Y + Offset;
                                        }
                                    }
                                    PointAtEndLineAVGY = Math.Min(OffsetPointY1, OffsetPointY2);
                                }
                                OffsetPoint1 = new XYZ(PointAtEndLine1.X, PointAtEndLineAVGY, PointAtEndLine1.Z);
                                OffsetPoint2 = new XYZ(PointAtEndLine2.X, PointAtEndLineAVGY, PointAtEndLine2.Z);
                                break;
                        }

                        Line DimensionLine = Line.CreateBound(OffsetPoint1, OffsetPoint2);

                        // Create a new reference array with the two grid lines End
                        ReferenceArray GridLineRefsList = new ReferenceArray();

                        GridLineRefsList.Append(new Reference(GridList[i]));
                        GridLineRefsList.Append(new Reference(GridList[i + 1]));

                        LinesList.Add(DimensionLine);
                        ListOfRefArray.Add(GridLineRefsList);
                    }
                }
            }
            return (ListOfRefArray, LinesList);
        }
        #endregion

    }
}
