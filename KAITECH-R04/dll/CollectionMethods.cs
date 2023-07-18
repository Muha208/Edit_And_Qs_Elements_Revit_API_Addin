
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using DataColumn = System.Data.DataColumn;
using DataTable = System.Data.DataTable;

namespace DLL
{
    public static class CollectionMethods
    {
        public static List<string> GetListOfStringListFromdata(params string[] StringValueAsOneRow)
        {
           return StringValueAsOneRow.ToList();
        }
        public static List<string> GetListWithNoneOrSelectAll(List<string> MainList, string StringToList = "None")
        {
            var ColumnsListWithSelectAllOrNone = new StringBuilder();
            var ListOfFilterSelection = new List<string>();
            ListOfFilterSelection.Add(StringToList);
            ListOfFilterSelection.AddRange(MainList);
            foreach (var item in ListOfFilterSelection)
            {
                ColumnsListWithSelectAllOrNone.AppendLine(item);
            }
            File.WriteAllText(LogDirectors.GetListWithNoneOrSelectAllFilePath, ColumnsListWithSelectAllOrNone.ToString());
            return ListOfFilterSelection;
        }
    }
}
