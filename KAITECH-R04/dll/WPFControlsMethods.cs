
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Dll;
using FoundationRFT.Model;
using KAITECH_R04;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;
using CheckBox = System.Windows.Controls.CheckBox;
using ComboBox = System.Windows.Controls.ComboBox;
using DataColumn = System.Data.DataColumn;
using DataGrid = System.Windows.Controls.DataGrid;
using DataGridColumn = System.Windows.Controls.DataGridColumn;
using DataTable = System.Data.DataTable;
using ListBox = System.Windows.Controls.ListBox;

namespace DLL
{
    public static class WPFControlsMethods
    {
        public static string BtName = "";
        public static DataTable InterMainDataTable;
        private static Autodesk.Revit.DB.Document document = Commands.document;
        private static UIDocument uidoc = Commands.uidoc;
        public static List<List<XYZ>> ElementIntersectionsPoints;
        const string SelectAllContain = "Select All";
        public static List<List<string>> ListOFIntRowValues;
        public static List<string> ColumnsDataTableName = new List<string>() { "No", "Category Name", "Element Name", "Element Id", "Radius", "Dimeter", "Lines Count", "Parameter", "Area", "Height", "Clear Height", "Volume", "Level" };
        static List<string> IntersectionDataTableName = new List<string>() { "No", "Column Name", "Line No", "Line Length", "Intersection Grids", "No Points", "Points" };

        public static void FillingTextColumnShape(System.Windows.Controls.TextBox TextShapeBox, MCategories mCategories,DataGrid dataGrid)
        {
            string CShape = "";
            DataRowView rowV = dataGrid.SelectedItem as DataRowView;
            if (rowV != null)
            {
                DataRow row = rowV.Row;
                if (row != null)
                {
                    if (row.ItemArray[1].ToString() == "Structural Columns")
                    {
                        foreach (var ele in mCategories.Elements)
                        {
                            if (ele.ELementsId.Value.ToString() == row.ItemArray[3].ToString())
                            {
                                CShape = ele.ELementsParameters.ElementColumnShape;
                                break;
                            }
                        }
                    }
                }
            }
            TextShapeBox.Text = CShape;
        }
        public static void FillingData(object sender, Button SelectAllElement_Bt, ComboBox Category_cb)
        {
            MainWindow.ListOfColumnsElements = new List<Element>();
            MainWindow.ListOFComboBox = new List<string>();
            Button b = sender as Button;
            if (b.Name == SelectAllElement_Bt.Name)
            {
                MainWindow.ListOfColumnsElements = RevitFilters.GetElementsDataByClass(typeof(FamilyInstance)).ELements;
                MainWindow.ListOfColumnsElements.AddRange(RevitElementsMethods.FloorsEleList);
                MainWindow.ListOfColumnsElements.AddRange(RevitElementsMethods.WallsEleList);
            }
            else
            {
                BtName = "All";
                MainWindow.ListOfColumnsElements = RevitFilters.GetAllInstanceElements(BuiltInCategory.OST_StructuralColumns).ELements;
            }
            MainWindow.ListOFComboBox.Add("All Elements");
            MainWindow.ListOFComboBox.AddRange(MainWindow.ListOfColumnsElements.Select(x => x.Category.Name).Distinct());
        }
        public static void FillingTableAcoordingClass()
        {
            MainWindow.ListOFRowValues = new List<List<string>>();
            MainWindow.MainDataTable = CreateDataTableColumns(ColumnsDataTableName);
            if (OwneRevitMathods.InformationClass.CategoryInformation != null || OwneRevitMathods.InformationClass.CategoryInformation.Count > 0)
            {
                int SN = 1;
                foreach (var category in OwneRevitMathods.InformationClass.CategoryInformation)
                {
                    foreach (var ele in category.Elements)
                    {
                        double ValueLength_Radius = 0;
                        double ValueWidth_Dimeter = 0;
                        string VName;
                        double Parameter = 0;
                        if (ele.ELementFamilyName == "Concrete Round")
                        {
                            ValueWidth_Dimeter = ele.ELementsParameters.ElementDaimeter;
                            ValueLength_Radius = ele.ELementsParameters.ElementRadius;
                        }
                        else
                        {
                            ValueLength_Radius = ele.ELementsParameters.ElementLength;
                            ValueWidth_Dimeter = ele.ELementsParameters.ElementWidth;
                        }
                        if (ele.ELementsParameters.ElementViewPlane == null)
                        {
                            VName = "Null";
                        }
                        else
                        {
                            VName = ele.ELementsParameters.ElementViewPlane.Name;
                        }
                        if (category.CategoriesName == "Structural Framing")
                        {
                            Parameter = ((ele.ELementsParameters.ElementHeight) + (ele.ELementsParameters.ElementArea / ele.ELementsParameters.ElementHeight)) * 2;
                        }
                        else
                        {
                            Parameter = ele.ELementsParameters.ElementCurvesCount;
                        }
                        if (MainWindow.SelectionFilterComboBox == "All Elements")
                        {

                            MainWindow.ListOFRowValues.Add(CollectionMethods.GetListOfStringListFromdata($"{SN}",
                            $"{category.CategoriesName}",
                            $"{ele.ELementsName}",
                            $"{ele.ELementsId}",
                            $"{Math.Round(ValueLength_Radius, 2)}",
                            $"{Math.Round(ValueWidth_Dimeter, 2)}",
                            $"{Math.Round(Parameter, 2)}",
                            $"{Math.Round(ele.ELementsParameters.ElementTotalCurvesLength, 2)}",
                            $"{Math.Round(ele.ELementsParameters.ElementArea, 2)}",
                            $"{Math.Round(ele.ELementsParameters.ElementHeight, 2)}",
                            $"{Math.Round(ele.ELementsParameters.ElementClearHeight, 2)}",
                            $"{Math.Round(ele.ELementsParameters.ElementVolume, 2)}",
                            $"{VName}"));
                            SN++;
                        }
                        else
                        {
                            if (ele.ELement.Category.Name == MainWindow.SelectionFilterComboBox)
                            {
                                MainWindow.ListOFRowValues.Add(CollectionMethods.GetListOfStringListFromdata($"{SN}",
                                $"{category.CategoriesName}",
                                $"{ele.ELementsName}",
                                $"{ele.ELementsId}",
                                $"{Math.Round(ValueLength_Radius, 2)}",
                                $"{Math.Round(ValueWidth_Dimeter, 2)}",
                                $"{ele.ELementsParameters.ElementCurvesCount}",
                                $"{Math.Round(ele.ELementsParameters.ElementTotalCurvesLength, 2)}",
                                $"{Math.Round(ele.ELementsParameters.ElementArea, 2)}",
                                $"{Math.Round(ele.ELementsParameters.ElementHeight, 2)}",
                                $"{Math.Round(ele.ELementsParameters.ElementClearHeight, 2)}",
                                $"{Math.Round(ele.ELementsParameters.ElementVolume, 2)}",
                                $"{VName}"));
                                SN++;
                            }
                        }
                    }
                }
                MainWindow.MainDataTable.Clear();
                MainWindow.dataGrid.ItemsSource = FillDataTablesRowFromList(MainWindow.MainDataTable, MainWindow.ListOFRowValues, ColumnsDataTableName.Count).DefaultView;
                MainWindow.LogBox.Text = "Data Is Ready To Use..........!";
            }
        }
        public static void FillingIntersecTableAcoordingClass()
        {
            ElementIntersectionsPoints = new List<List<XYZ>>();
            var InterListOFRowValues = new List<List<string>>();
            InterMainDataTable = CreateDataTableColumns(IntersectionDataTableName);
            if (OwneRevitMathods.InformationClass.CategoryInformation != null || OwneRevitMathods.InformationClass.CategoryInformation.Count > 0)
            {
                int SN = 1;
                foreach (var category in OwneRevitMathods.InformationClass.CategoryInformation)
                {
                    if (category.CategoriesName == "Structural Columns")
                    {
                        int EleNo = 1;
                        foreach (var ele in category.Elements)
                        {
                            int CurveCount = 1;
                            var InterSectionPoints = new List<XYZ>();
                            var CGridLIst = new List<string>();
                            InterListOFRowValues.Add(CollectionMethods.GetListOfStringListFromdata($"{EleNo}",
                                                                           $"{ele.ELementsName}",
                                                                           $"",
                                                                           $"",
                                                                           $"",
                                                                           $"",
                                                                           $""));

                            foreach (Curve curve in ele.ELementsParameters.ElementCurvesList)
                            {

                                InterSectionPoints = RevitElementsMethods.GetTheIntersectionPointsBetweenTowCurves(ele, curve, ele.ELementsParameters.ElementViewPlane)
                                     .IntersectionsPoints;
                                CGridLIst = RevitElementsMethods.GetTheIntersectionPointsBetweenTowCurves(ele, curve, ele.ELementsParameters.ElementViewPlane)
                                     .IntesectedGridName;

                                ElementIntersectionsPoints.Add(InterSectionPoints);
                                InterListOFRowValues.Add(CollectionMethods.GetListOfStringListFromdata($"",
                                                                           $"",
                                                                           $"Line {ele.ELementsParameters.ElementCurvesList.Count}-{CurveCount}",
                                                                           $"{Math.Round(curve.Length, 2)}",
                                                                           $"",
                                                                           $"",
                                                                           $""));
                                for (int i = 0; i < InterSectionPoints.Count; i++)
                                {
                                    InterListOFRowValues.Add(CollectionMethods.GetListOfStringListFromdata($"",
                                                                                                           $"",
                                                                                                           $"",
                                                                                                           $"",
                                                                                                           $"{CGridLIst[i]}",
                                                                                                           $"Point {InterSectionPoints.Count}-{i+1}",
                                                                                                           $"{InterSectionPoints[i]}"));
                                }
                                CurveCount++;
                            }
                            InterListOFRowValues.Add(CollectionMethods.GetListOfStringListFromdata($"",
                                                                           $"Total Line Length",
                                                                           $"=",
                                                                           $"{ele.ELementsParameters.ElementCurvesList.Sum(x => x.Length)}",
                                                                           $"",
                                                                           $"",
                                                                           $""));
                            SN++;
                            EleNo++;
                        }
                    }
                }
                MainWindow.InterscDataGrid.ItemsSource = FillDataTablesRowFromList(InterMainDataTable, InterListOFRowValues, IntersectionDataTableName.Count).DefaultView;
                MainWindow.LogBox.Text = $"Table Get Data Successfully!................";
            }
        }
        public static DataTable CreateDataTableColumns(List<string> ColumnsName)
        {
            DataTable dataTable = new DataTable();
            foreach (var ColName in ColumnsName)
            {
                dataTable.Columns.Add(new DataColumn(ColName));
            }
            return dataTable;
        }
        public static DataTable FillDataTablesRowFromList(DataTable dataTable, List<List<string>> Rows, int ColumnsNumbers)
        {
            if (dataTable != null)
            {
                dataTable.Clear();
            }
            if (Rows != null)
            {
                foreach (var RowArray in Rows)
                {
                    var row = dataTable.NewRow();
                    for (int i = 0; i < ColumnsNumbers; i++)
                    {
                        row[i] = RowArray[i].ToString();
                    }
                    dataTable.Rows.Add(row);
                }
            }
            return dataTable;
        }
    }

}