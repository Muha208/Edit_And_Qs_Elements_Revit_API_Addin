using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Net.Security;

namespace DLL
{
    public static class LogDirectors
    {
        public static string FullDirctory = GetDirc(); 
        private static string creatingExcelfilepath = @"\Excel Files\Total_View.xlsx";
        public static string CreatingExcelfilepath { get => FullDirctory + creatingExcelfilepath; set => creatingExcelfilepath = value; }
        #region Main
        private static string largeIconPath = @"\Icons\KLarg.png";
        public static string LargeIconPath { get => FullDirctory + largeIconPath; set => largeIconPath = value; }
        private static string mianIconPath = @"\Icons\KMain.png";
        public static string MianIconPath { get => FullDirctory + mianIconPath; set => mianIconPath = value; }
        #endregion
        #region Collection Methods
        private static string getListWithNoneOrSelectAllFilePath = @"\Logs\GetListWithNoneOrSelectAll.txt";
        public static string GetListWithNoneOrSelectAllFilePath { get => FullDirctory + getListWithNoneOrSelectAllFilePath; set => getListWithNoneOrSelectAllFilePath = value; }
        #endregion
        #region Open Stream Files
        private static string eXEcellFilepath = @"\ExcelFiles\";
        private static string logNameFilepath = @"\Logs\LogNameFile.txt";
        private static string logNumberFilepath = @"\Logs\LogNumberFile.txt";
        private static string logExcelSheetsNameFilepath = @"\Logs\LogExcelSheetsNameFile.txt";
        private static string logEncryptFilepath = @"\Logs\LogEncryptFile.txt";
        public static string EXEcellFilepath { get => FullDirctory + eXEcellFilepath; set => eXEcellFilepath = value; }
        #region Prop
        public static string LogNameFilepath { get => Open_StreamFiles.IsDriectoryExists(FullDirctory + logNameFilepath); set => logNameFilepath = value; }
        public static string LogNumberFilepath { get => Open_StreamFiles.IsDriectoryExists(FullDirctory + logNumberFilepath); set => logNumberFilepath = value; }
        public static string LogExcelSheetsNameFilepath { get => Open_StreamFiles.IsDriectoryExists(FullDirctory + logExcelSheetsNameFilepath); set => logExcelSheetsNameFilepath = value; }
        public static string LogEncryptFilepath { get => Open_StreamFiles.IsDriectoryExists(FullDirctory + logEncryptFilepath); set => logEncryptFilepath = value; }
        #endregion
        #endregion
        #region Owne Revit Methods
        private static string logSelectedElementsIdFilepath = @"\Logs\logSelectedElementsIdFile.txt";
        private static string logSelectedElementsIdFilterFilepath = @"\Logs\logSelectedElementsIdFilterFile.txt";
        private static string getParametersElementsByListOfIdElementsFilePath = @"\Logs\GetParametersElementsByListOfIdElementsFile.txt";
        private static string getFilteredParameterElementsByListOfIdElementsFilePath = @"\Logs\GetFilteredParameterElementsByListOfIdElementsFile.txt";
        #region Props
        public static string LogSelectedElementsIdFilepath { get => FullDirctory + logSelectedElementsIdFilepath; set => logSelectedElementsIdFilepath = value; }
        public static string LogSelectedElementsIdFilterFilepath { get => FullDirctory + logSelectedElementsIdFilterFilepath; set => logSelectedElementsIdFilterFilepath = value; }
        public static string GetParametersElementsByListOfIdElementsFilePath { get => FullDirctory + getParametersElementsByListOfIdElementsFilePath; set => getParametersElementsByListOfIdElementsFilePath = value; }
        public static string GetFilteredParameterElementsByListOfIdElementsFilePath { get => FullDirctory + getFilteredParameterElementsByListOfIdElementsFilePath; set => getFilteredParameterElementsByListOfIdElementsFilePath = value; }

        #endregion
        #endregion
        #region Revit Converter
        private static string logGetTypeFromFamilyByTextFilepath = @"\Logs\LogGetTypeFromFamilyByTextFile.txt";
        public static string LogGetTypeFromFamilyByTextFilepath { get => FullDirctory + logGetTypeFromFamilyByTextFilepath; set => logGetTypeFromFamilyByTextFilepath = value; }
        #endregion
        #region Revit Elements Methods
        private static string getFamilyName = @"\Logs\GetFamilyNameFile.txt";
        private static string getAllInstanceElementsAtSpacficViewPlan = @"\Logs\GetAllInstanceElementsAtSpacficViewPlanFile.txt";
        private static string getListOfColumnsCurvesAtViewPalnPath = @"\Logs\GetListOfColumnsCurvesAtViewPalnFile.txt";
        private static string elementsToFillFilePath = @"\Logs\ElementsToFillFile.txt";
        public static string ElementsToFillFilePath { get => FullDirctory + elementsToFillFilePath; set => elementsToFillFilePath = value; }
        public static string GetListOfColumnsCurvesAtViewPalnPath { get => FullDirctory + getListOfColumnsCurvesAtViewPalnPath; set => getListOfColumnsCurvesAtViewPalnPath = value; }
        public static string GetAllInstanceElementsAtSpacficViewPlan { get => FullDirctory + getAllInstanceElementsAtSpacficViewPlan; set => getAllInstanceElementsAtSpacficViewPlan = value; }
        public static string GetFamilyName { get => FullDirctory + getFamilyName; set => getFamilyName = value; }

        #endregion
        #region Revit Filters
        private static string getAllInstanceElementsAtSpacficViewPlanPath = @"\Logs\LogGetAllInstanceElementsAtSpacficViewPlanFile.txt";
        private static string getAllIElementsFilePath = @"\Logs\LogGetAllElementsFile.txt";
        private static string getAllSymbolIElementsFilePath = @"\Logs\LogGetAllSymbolIElementsFilePath.txt";
        private static string logElementNameWithIdFilepath = @"\Logs\LogElementNameWithIDFile.txt";
        private static string logGetElementIdFilepath = @"\Logs\logGetElementIdFile.txt";
        private static string logGetElementNameFilepath = @"\Logs\logGetElementNameFile.txt";
        private static string logViewFamilyTypeNameWithIdFilepath = @"\Logs\LogViewFamilyTypeNameWithIDFile.txt";
        private static string logGetElementsDataByClassFilepath = @"\Logs\LogGetElementsDataByClassFile.txt";
        private static string logGetElementsDataByClassAndCategoryFilepath = @"\Logs\LogGetElementsDataByClassAndCategoryFile.txt";
        private static string logGetFamilyInstanceByTypeOfFamilyFilepath = @"\Logs\LogGetFamilyInstanceByTypeOfFamilyFile.txt";
        private static string getAllInstanceElementsFilePath = @"\Logs\GetAllInstanceElementsFile.txt";

        public static string LogElementNameWithIdFilepath { get => FullDirctory + logElementNameWithIdFilepath; set => logElementNameWithIdFilepath = value; }
        public static string LogGetElementIdFilepath { get => FullDirctory + logGetElementIdFilepath; set => logGetElementIdFilepath = value; }
        public static string LogGetElementNameFilepath { get => FullDirctory + logGetElementNameFilepath; set => logGetElementNameFilepath = value; }
        public static string LogViewFamilyTypeNameWithIdFilepath { get => FullDirctory + logViewFamilyTypeNameWithIdFilepath; set => logViewFamilyTypeNameWithIdFilepath = value; }
        public static string LogGetElementsDataByClassFilepath { get => FullDirctory + logGetElementsDataByClassFilepath; set => logGetElementsDataByClassFilepath = value; }
        public static string LogGetElementsDataByClassAndCategoryFilepath { get => FullDirctory + logGetElementsDataByClassAndCategoryFilepath; set => logGetElementsDataByClassAndCategoryFilepath = value; }
        public static string LogGetFamilyInstanceByTypeOfFamilyFilepath { get => FullDirctory + logGetFamilyInstanceByTypeOfFamilyFilepath; set => logGetFamilyInstanceByTypeOfFamilyFilepath = value; }
        public static string GetAllInstanceElementsFilePath { get => FullDirctory + getAllInstanceElementsFilePath; set => getAllInstanceElementsFilePath = value; }
        public static string GetAllIElementsFilePath { get => FullDirctory + getAllIElementsFilePath; set => getAllIElementsFilePath = value; }
        public static string GetAllSymbolIElementsFilePath { get => FullDirctory + getAllSymbolIElementsFilePath; set => getAllSymbolIElementsFilePath = value; }
        public static string GetAllInstanceElementsAtSpacficViewPlanPath { get => FullDirctory + getAllInstanceElementsAtSpacficViewPlanPath; set => getAllInstanceElementsAtSpacficViewPlanPath = value; }

        #endregion
        private static string GetDirc()
        {
            if (new DirectoryInfo(@".\").Root.FullName == @"C:\")
            {
                return @"C:\";
            }
            else
            {
                return new DirectoryInfo(@".\").Parent.Parent.FullName;
            }
        }
    }
}
