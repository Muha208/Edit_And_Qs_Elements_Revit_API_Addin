namespace DLL
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Text;
    using System.Threading.Tasks;
    using System.Xml.Linq;
    using Autodesk.Revit.DB;
    using Autodesk.Revit.UI;
    using FoundationRFT.Model;
    using KAITECH_R04;
    using Element = Autodesk.Revit.DB.Element;

    public static class RevitFilters
    {
        private static Autodesk.Revit.DB.Document document = Commands.document;
        private static UIDocument uidoc = Commands.uidoc;
        public enum ReturnElementData
        {
            ElementID,
            ElementName
        }
        public enum ReturnViewData
        {
            ViewFamilyTpeID,
            ViewFamilyTpeName
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="document"></param>
        /// <param name="builtInCategory"></param>
        /// <param name="ElementReturnName"></param>
        /// <returns></returns>
        public static (List<Element> ELements, List<ElementId> ELementsID, List<string> ELementsName) GetAllInstanceElements(BuiltInCategory builtInCategory, string ElementReturnName = "All Instance Elements")
        {
            var Logfile = new StringBuilder();
            Logfile.Append($"Successed To Get {ElementReturnName}:\n");
            var CollectorElementID = new FilteredElementCollector(document);
            var ELements = CollectorElementID.OfCategory(builtInCategory).WhereElementIsNotElementType()
                .ToElements().ToList();
            var ELementsID = new List<ElementId>();
            var ELementsName = new List<string>();
            int index = 0;
            foreach (var Element in ELements)
            {
                ELementsID.Add(Element.Id);
                ELementsName.Add(Element.Name);
                Logfile.Append($"Name: {ELementsName[index]} ----- ID: {ELementsID[index]}\n");
                index++;
            }
            Open_StreamFiles.CreatEncryptedLogFile(LogDirectors.GetAllInstanceElementsFilePath, ".log", Logfile, "Revit Filters", false);
            return (ELements, ELementsID, ELementsName);
        }
        /// <summary>
        /// Get All Elements Without adding any Filter Data as list of (ID <ElementID> Or Name <string>). 
        /// </summary>
        /// <param name="document">The decoumant that include the database.</param>
        /// <param name="builtInCategory">The Catecory Type want to get the data from it.</param>
        /// <param name="returnData">ElemantID as list<ElementID> Or ElemnetName as list<string>.</param>
        /// <param name="ElementReturnName">This for Log File Only.</param>
        /// <returns></returns>
        public static object GetAllElementsDataByCategory(BuiltInCategory builtInCategory, ReturnElementData returnData, string ElementReturnName = "Levels")
        {
            var Logfile = new StringBuilder();
            Logfile.Append($"Successed To Get {ElementReturnName}:\n");
            var CollectorElementID = new FilteredElementCollector(document);
            var ELements = CollectorElementID.OfCategory(builtInCategory)
                .ToElements();
            var ELementsID = new List<ElementId>();
            var ELementsName = new List<string>();
            int index = 0;
            foreach (var Element in ELements)
            {
                ELementsID.Add(Element.Id);
                ELementsName.Add(Element.Name);
                Logfile.Append($"Name: {ELementsName[index]} ----- ID: {ELementsID[index].ToString()}\n");
                index++;
            }
            Open_StreamFiles.CreatEncryptedLogFile(LogDirectors.LogElementNameWithIdFilepath, ".log", Logfile, "Revit Filters", false);
            switch (returnData)
            {
                case ReturnElementData.ElementID:
                    return ELementsID;
                default:
                    return ELementsName;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="document"></param>
        /// <param name="returnData"></param>
        /// <returns></returns>

        public static (List<Element> ELements, List<ElementId> ELementsID, List<string> ELementsName) GetElementsDataByClass(Type type, string ElementReturnName = "Families")
        {
            var Logfile = new StringBuilder();
            Logfile.Append($"Successed To Get {ElementReturnName}:\n");
            var CollectorElementID = new FilteredElementCollector(document);
            var ELements = CollectorElementID.OfClass(type)
                .ToElements().ToList();
            var ELementsID = new List<ElementId>();
            var ELementsName = new List<string>();
            int index = 0;
            foreach (var Element in ELements)
            {
                ELementsID.Add(Element.Id);
                ELementsName.Add(Element.Name);
                Logfile.Append($"Name: {ELementsName[index]} ----- ID: {ELementsID[index].ToString()}\n");
                index++;
            }
            Open_StreamFiles.CreatEncryptedLogFile(LogDirectors.LogGetElementsDataByClassAndCategoryFilepath, ".log", Logfile, "Revit Filters", false);
            return (ELements, ELementsID, ELementsName);
        }
        public static FamilySymbol GetSymbolElement(BuiltInCategory builtInCategory, string ElementReturnName = "Family Instance")
        {
            var Logfile = new StringBuilder();
            var CollectorCategory = new FilteredElementCollector(document);
            var Familylist = CollectorCategory.OfClass(typeof(FamilySymbol)).OfCategory(builtInCategory).Cast<FamilySymbol>();
            var FamilyBlockTitle = Familylist.FirstOrDefault();
            Logfile.Append($"Family Name: {FamilyBlockTitle.Name} -- Family Id: {FamilyBlockTitle.Id}\n");
            //Open_StreamFiles.CreatEncryptedLogFile(logGetFamilyInstanceByTypeOfFamilyFilepath, ".log", Logfile, "Revit Filters", false);
            return FamilyBlockTitle;
        }
        public static (List<ImportInstance> FamilySymbolList, List<ElementId> ELementsID, List<string> ELementsName) GetSymbolElements(string ElementReturnName = "Family Symbol Instance")
        {
            var Logfile = new StringBuilder();
            var CollectorCategory = new FilteredElementCollector(document);
            var Familylist = CollectorCategory.OfClass(typeof(ImportInstance)).WhereElementIsNotElementType()
                .Cast<ImportInstance>().ToList();
            var ELementsID = new List<ElementId>();
            var ELementsName = new List<string>();
            int index = 0;
            foreach (var Element in Familylist)
            {
                ELementsID.Add(Element.Id);
                ELementsName.Add(Element.Name);
                Logfile.Append($"Name: {ELementsName[index]} ----- ID: {ELementsID[index]}\n");
                index++;
            }
            Open_StreamFiles.CreatEncryptedLogFile(LogDirectors.GetAllSymbolIElementsFilePath, ".log", Logfile, "Revit Filters", false);
            return (Familylist, ELementsID, ELementsName); ;
        }
        public static List<FamilyInstance> GetAllSymbolElements(BuiltInCategory builtInCategory, string ElementReturnName = "Family Instance")
        {
            var Logfile = new StringBuilder();
            var CollectorCategory = new FilteredElementCollector(document);
            var Familylist = CollectorCategory.OfClass(typeof(FamilyInstance)).OfCategory(builtInCategory).Cast<FamilyInstance>().ToList();
            foreach (var item in Familylist)
            {
                Logfile.Append($"Family Name: {item.Name} -- Family Id: {item.Id}\n");
            }
            Open_StreamFiles.CreatEncryptedLogFile(LogDirectors.LogGetFamilyInstanceByTypeOfFamilyFilepath, ".log", Logfile, "Revit Filters", false);
            return Familylist;
        }
        public static (List<Element> ELements, List<ElementId> ELementsID, List<string> ELementsName) GetAllElements(string ElementReturnName = "All Instance Elements")
        {
            var Logfile = new StringBuilder();
            Logfile.Append($"Successed To Get {ElementReturnName}:\n");
            var CollectorElementID = new FilteredElementCollector(document);
            var ELements = CollectorElementID.OfClass(typeof(FamilyInstance)).WhereElementIsNotElementType().ToElements().ToList();
            var ELementsID = new List<ElementId>();
            var ELementsName = new List<string>();
            int index = 0;
            foreach (var Element in ELements)
            {
                ELementsID.Add(Element.Id);
                ELementsName.Add(Element.Name);
                Logfile.Append($"Name: {Element.Name} ----- ID: {Element.Id} - Category Name: {Element.Category.Name} ----- Category ID: {Element.Category.Id}\n");
                index++;
            }
            Open_StreamFiles.CreatEncryptedLogFile(LogDirectors.GetAllIElementsFilePath, ".log", Logfile, "Revit Filters", false);
            return (ELements, ELementsID, ELementsName);
        }
        private static List<BuiltInCategory> GetSTRCategories()
        {
            var CategoriesList = new List<BuiltInCategory>();
            string STR01 = "Stair";
            string STR02 = "Colum";
            string STR03 = "Floor";
            string STR04 = "Struc";
            string STR05 = "Ramps";
            string STR06 = "Secti";
            string STR07 = "Rebar";
            foreach (BuiltInCategory cat in Enum.GetValues(typeof(BuiltInCategory)))
            {
                if (Enum.GetValues(typeof(BuiltInCategory)).ToString().Substring(0, 9) == $"OST_{STR01}" ||
                    Enum.GetValues(typeof(BuiltInCategory)).ToString().Substring(0, 9) == $"OST_{STR02}" ||
                    Enum.GetValues(typeof(BuiltInCategory)).ToString().Substring(0, 9) == $"OST_{STR03}" ||
                    Enum.GetValues(typeof(BuiltInCategory)).ToString().Substring(0, 9) == $"OST_{STR04}" ||
                    Enum.GetValues(typeof(BuiltInCategory)).ToString().Substring(0, 9) == $"OST_{STR05}" ||
                    Enum.GetValues(typeof(BuiltInCategory)).ToString().Substring(0, 9) == $"OST_{STR06}" ||
                    Enum.GetValues(typeof(BuiltInCategory)).ToString().Substring(0, 9) == $"OST_{STR07}")
                {

                }
                CategoriesList.Add(cat);
            }
            return CategoriesList;
        }
        public static (List<View> Views, List<ElementId> ViewsId, List<string> ViewName) GetAllViewElements(string ElementReturnName = "All Instance Elements")
        {
            var Logfile = new StringBuilder();
            Logfile.Append($"Successed To Get {ElementReturnName}:\n");
            var CollectorElementID = new FilteredElementCollector(document);
            var ELements = CollectorElementID.OfClass(typeof(View)).ToList();
            var ListOfViews = new List<View>();
            var ELementsID = new List<ElementId>();
            var ELementsName = new List<string>();
            int index = 0;
            foreach (View Element in ELements)
            {
                ELementsID.Add(Element.Id);
                ELementsName.Add(Element.Name);
                ListOfViews.Add(Element);
                Logfile.Append($"Name: {ELementsName[index]} ----- ID: {ELementsID[index]}\n");
                index++;
            }
            Open_StreamFiles.CreatEncryptedLogFile(LogDirectors.GetAllInstanceElementsFilePath, ".log", Logfile, "Revit Filters", false);
            return (ListOfViews, ELementsID, ELementsName);
        }
        public static (List<Level> levels, List<ElementId> LevelsId, List<string> LevelsName) GetAllLevelElements(BuiltInCategory builtInCategory, string ElementReturnName = "All Instance Elements")
        {
            var Logfile = new StringBuilder();
            Logfile.Append($"Successed To Get {ElementReturnName}:\n");
            var CollectorElementID = new FilteredElementCollector(document);
            var ELements = CollectorElementID.OfClass(typeof(Level)).WhereElementIsNotElementType().Cast<Level>().ToList();
            var ELementsID = new List<ElementId>();
            var ELementsName = new List<string>();
            int index = 0;
            foreach (var Element in ELements)
            {
                ELementsID.Add(Element.Id);
                ELementsName.Add(Element.Name);
                Logfile.Append($"Name: {ELementsName[index]} ----- ID: {ELementsID[index]}\n");
                index++;
            }
            Open_StreamFiles.CreatEncryptedLogFile(LogDirectors.GetAllInstanceElementsFilePath, ".log", Logfile, "Revit Filters", false);
            return (ELements, ELementsID, ELementsName);
        }
        public static IEnumerable<ViewFamilyType> GetViewTypes(ViewType viewType)
        {
            IEnumerable<ViewFamilyType> ret = new FilteredElementCollector(document)
            .WherePasses(new ElementClassFilter(typeof(ViewFamilyType), false))
            .Cast<ViewFamilyType>();

            switch (viewType)
            {
                case ViewType.AreaPlan:
                    return ret.Where(e => e.ViewFamily == ViewFamily.AreaPlan);
                case ViewType.CeilingPlan:
                    return ret.Where(e => e.ViewFamily == ViewFamily.CeilingPlan);
                case ViewType.ColumnSchedule:
                    return ret.Where(e => e.ViewFamily == ViewFamily.GraphicalColumnSchedule);
                case ViewType.CostReport:
                    return ret.Where(e => e.ViewFamily == ViewFamily.CostReport);
                case ViewType.Detail:
                    return ret.Where(e => e.ViewFamily == ViewFamily.Detail);
                case ViewType.DraftingView:
                    return ret.Where(e => e.ViewFamily == ViewFamily.Drafting);
                case ViewType.DrawingSheet:
                    return ret.Where(e => e.ViewFamily == ViewFamily.Sheet);
                case ViewType.Elevation:
                    return ret.Where(e => e.ViewFamily == ViewFamily.Elevation);
                case ViewType.EngineeringPlan:
                    return ret.Where(e => e.ViewFamily == ViewFamily.StructuralPlan);
                case ViewType.FloorPlan:
                    return ret.Where(e => e.ViewFamily == ViewFamily.FloorPlan);
                case ViewType.Legend:
                    return ret.Where(e => e.ViewFamily == ViewFamily.Legend);
                case ViewType.LoadsReport:
                    return ret.Where(e => e.ViewFamily == ViewFamily.LoadsReport);
                case ViewType.PanelSchedule:
                    return ret.Where(e => e.ViewFamily == ViewFamily.PanelSchedule);
                case ViewType.PresureLossReport:
                    return ret.Where(e => e.ViewFamily == ViewFamily.PressureLossReport);
                case ViewType.Rendering:
                    return ret.Where(e => e.ViewFamily == ViewFamily.ImageView);
                case ViewType.Schedule:
                    return ret.Where(e => e.ViewFamily == ViewFamily.Schedule);
                case ViewType.Section:
                    return ret.Where(e => e.ViewFamily == ViewFamily.Section);
                case ViewType.ThreeD:
                    return ret.Where(e => e.ViewFamily == ViewFamily.ThreeDimensional);
                case ViewType.Undefined:
                    return ret.Where(e => e.ViewFamily == ViewFamily.Invalid);
                case ViewType.Walkthrough:
                    return ret.Where(e => e.ViewFamily == ViewFamily.Walkthrough);
                default:
                    return ret;
            }
        }
    }
}
