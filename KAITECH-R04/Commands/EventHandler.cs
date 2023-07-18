using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Dll;
using DLL;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

namespace KAITECH_R04
{
    public enum MethodsName
    {
        CreateSheets,
        CreateDim,
        FillingAndCreateLevels,
        FillingIntersecTable,
        GridDimension,
        ColumnAxisDimension,
        ExportDataToExcelFile,
        EditGrids,
        ExportDetailedDataToExcelFile
    }
    public class EventHandler : IExternalEventHandler
    {
        public static MethodsName methodsName;

        [Obsolete]
        public void Execute(UIApplication app)
        {
            Document doc = Commands.document;
            try
            {
                //notifying me of raised event
                Trace.WriteLine("Raised");
                try
                {
                    if (methodsName == MethodsName.CreateSheets)
                    {
                        using (Transaction tx = new Transaction(doc, "Create Sheets"))
                        {
                            tx.Start();
                            MethodsHandler();
                            tx.Commit();
                        }
                        MainWindow.LogBox.Text = "Sheets And Views Created Successfully!";
                    }
                    else if (methodsName == MethodsName.FillingAndCreateLevels)
                    {
                        using (Transaction tx = new Transaction(doc, "Filling And Create Levels"))
                        {
                            tx.Start();
                            MethodsHandler();
                            tx.Commit();
                        }
                    }
                    else if (methodsName == MethodsName.FillingIntersecTable)
                    {
                        using (Transaction tx = new Transaction(doc, "Filling Intersec Table"))
                        {
                            tx.Start();
                            MethodsHandler();
                            tx.Commit();
                        }
                    }
                    else if (methodsName == MethodsName.GridDimension)
                    {
                        using (Transaction tx = new Transaction(doc, "Grid Dimension"))
                        {
                            tx.Start();
                            MethodsHandler();
                            tx.Commit();
                        }
                        MainWindow.LogBox.Text = "The Grids Get Dimensions Successfully!";
                    }
                    else if (methodsName == MethodsName.ColumnAxisDimension)
                    {
                        using (Transaction tx = new Transaction(doc, "Column Axis Dimension"))
                        {
                            tx.Start();
                            MethodsHandler();
                            tx.Commit();
                        }
                        MainWindow.LogBox.Text = "The Coulmns Get Dimensions Successfully!";
                    }
                    else if (methodsName == MethodsName.ExportDataToExcelFile)
                    {
                        using (Transaction tx = new Transaction(doc, "Export Data To Excel File"))
                        {
                            tx.Start();
                            MethodsHandler();
                            tx.Commit();
                        }
                    }
                    else if (methodsName == MethodsName.EditGrids)
                    {
                        using (Transaction tx = new Transaction(doc, "Edit Grids"))
                        {
                            tx.Start();
                            MethodsHandler();
                            tx.Commit();
                        }
                        MainWindow.LogBox.Text ="The Grids Edit Successfully!";
                    }
                    else if (methodsName == MethodsName.ExportDetailedDataToExcelFile)
                    {
                        using (Transaction tx = new Transaction(doc, "Edit Grids"))
                        {
                            tx.Start();
                            MethodsHandler();
                            tx.Commit();
                        }
                    }
                }
                catch (Exception e)
                {
                    //catch whatever exception
                    throw e;
                }
            }
            catch (InvalidOperationException)
            {

                throw;
            }
        }

        [Obsolete]
        private static void MethodsHandler()
        {
            switch (methodsName)
            {
                case MethodsName.FillingAndCreateLevels:
                    WPFControlsMethods.FillingData(MainWindow.Bsender, MainWindow.AllElementButton, MainWindow.MainCoboBox);
                    RevitElementsMethods.FillClassesByLIstOfElements(MainWindow.ListOfColumnsElements);
                    WPFControlsMethods.FillingTableAcoordingClass();
                    MainWindow.MainCoboBox.ItemsSource = MainWindow.ListOFComboBox;
                    MainWindow.MainCoboBox.SelectedIndex = 0;
                    break;
                case MethodsName.EditGrids:
                    RevitCreate.EditGrids();
                    break;
                case MethodsName.CreateSheets:
                    
                    RevitCreate.CreateSheetsAccordingToColumnsLevel();
                    MainWindow.EditGridsB.IsEnabled = true;
                    break;
                case MethodsName.FillingIntersecTable:
                    WPFControlsMethods.FillingIntersecTableAcoordingClass();
                    break;
                case MethodsName.GridDimension:

                    foreach (var ViewPlaneColumn in RevitCreate.ListOfViewPalns)
                    {
                        RevitCreate.PutDimensionAtViewPlanGridsOnly(ViewPlaneColumn.Value);
                    }
                    break;
                case MethodsName.ColumnAxisDimension:
                    if (RevitCreate.ListOfViewPalns == null && MainWindow.ListOfColumnsElements == null)
                    {
                        WPFControlsMethods.FillingData(MainWindow.Csender, MainWindow.AllElementButton, MainWindow.MainCoboBox);
                        RevitElementsMethods.FillClassesByLIstOfElements(MainWindow.ListOfColumnsElements);
                        RevitCreate.CreateSheetsAccordingToColumnsLevel();
                        RevitCreate.EditGrids();
                    }
                    foreach (var ViewPlaneColumn in RevitCreate.ListOfViewPalns)
                    {
                        RevitCreate.PutDimensionAtViewPlanColumnsAxisOnly(ViewPlaneColumn.Value);
                        MainWindow.LogBox.Text = $"For Sheet With View Plane Named: {ViewPlaneColumn.Key}\n" +
                                                 $"Columns Axis Dimesions Created Successfully!..................................";
                    }
                    break;
                case MethodsName.ExportDataToExcelFile:
                    var FilePath = LogDirectors.EXEcellFilepath + "DataExcelFile.csv";
                    Open_StreamFiles.ExportDataToExcelFile(MainWindow.MainDataTable , FilePath);
                    using (var NewProsses = new Process())
                    {
                        NewProsses.StartInfo.FileName = LogDirectors.EXEcellFilepath;
                        if (NewProsses.Start())
                        {
                            NewProsses.Dispose();
                        }
                        NewProsses.Start();
                    }
                    MainWindow.LogBox.Text = $"Data Exported Successfully To {FilePath}................";
                    break;
                case MethodsName.ExportDetailedDataToExcelFile:
                    var FilePath2 = LogDirectors.EXEcellFilepath + "DataDExcelFile.csv";
                    Open_StreamFiles.ExportDataToExcelFile(WPFControlsMethods.InterMainDataTable, FilePath2);
                    using (var NewProsses = new Process())
                    {
                        NewProsses.StartInfo.FileName = LogDirectors.EXEcellFilepath;
                        if (NewProsses.Start())
                        {
                            NewProsses.Dispose();
                        }
                        NewProsses.Start();
                    }
                    MainWindow.LogBox.Text = $"Data Exported Successfully To {FilePath2}................";
                    break;
                default:
                    break;
            }
        }
        public string GetName()
        {
            return " Event Handler";
        }

    }

}
