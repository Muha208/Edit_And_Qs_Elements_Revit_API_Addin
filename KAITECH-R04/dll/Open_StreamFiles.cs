namespace DLL
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.IO;
    //using OfficeOpenXml;
    using System.Windows;
    using System.Security.AccessControl;
    using System.Windows.Shapes;
    using Autodesk.Revit.DB;
    using System.Data;

    public static class Open_StreamFiles 
    {
        public static void CreatEncryptedLogFile(string FullPath,string NewExtension, StringBuilder LogMessage,string FolderName,bool AddGUID)
        {
            FileInfo logfi = new FileInfo(LogDirectors.LogEncryptFilepath);
            var logEncryptFile = new StringBuilder();
            try
            {
                string NewFileName;
                switch (AddGUID)
                {
                    case true:
                        NewFileName = $"{System.IO.Path.GetFileNameWithoutExtension(@FullPath)}-{Guid.NewGuid()}{NewExtension}";
                        break;
                    default:
                        NewFileName = $"{System.IO.Path.GetFileNameWithoutExtension(@FullPath)}{NewExtension}";
                        break;
                }
                var FullDirctory = IsDriectoryExists(new DirectoryInfo(FullPath).Parent.FullName) + @"\Logs";
                FileInfo fi = new FileInfo(FullDirctory + NewFileName);
                using (var WriteLog = fi.CreateText())
                {
                    WriteLog.Write(LogMessage);
                }
                System.IO.Path.ChangeExtension(FullDirctory, NewExtension);
                logEncryptFile.Append($"Given Path:{@FullPath}\n"+
                    $"New File Name:{NewFileName}\n"+
                    $"Parent Path:{FullDirctory}\n"+
                    $"Full New Path:{FullDirctory}\n").ToString();
                using (var WriteLog = logfi.CreateText())
                {
                    WriteLog.Write(logEncryptFile);
                }
            }
            catch (Exception ex)
            {
                logEncryptFile.Append($"Failed To Encrypt Data!\n{ex.Message}").ToString();
                using (var WriteLog = logfi.CreateText())
                {
                    WriteLog.Write(logEncryptFile);
                }
                throw;
            }    
        }
        internal static string IsDriectoryExists(string FullDirectoryPath)
        {
            DirectoryInfo DirectoryParentPath = new DirectoryInfo(FullDirectoryPath);
            var Path = DirectoryParentPath.Parent.FullName;
            if (!Directory.Exists(Path))
            {
                DirectoryInfo NewDirctory = new DirectoryInfo(Path);
                NewDirctory.Create();
                DirectorySecurity security = NewDirctory.GetAccessControl();
                security.AddAccessRule(new FileSystemAccessRule(Environment.UserName, FileSystemRights.FullControl,
                    InheritanceFlags.ContainerInherit | InheritanceFlags.ObjectInherit, PropagationFlags.InheritOnly, AccessControlType.Allow));
                NewDirctory.SetAccessControl(security);
                if (NewDirctory.Attributes == FileAttributes.ReadOnly)
                {
                    NewDirctory.Attributes = FileAttributes.Normal;
                }
            }
            return FullDirectoryPath;
        }
        public static void ExportDataToExcelFile(DataTable dataTable, string FilePath)
        {
            var NewFilePath = IsDriectoryExists(FilePath);
            var lines = new List<string>();

            string[] columnNames = dataTable.Columns
                .Cast<DataColumn>()
                .Select(column => column.ColumnName)
                .ToArray();

            var header = string.Join(",", columnNames.Select(name => $"\"{name}\""));
            lines.Add(header);

            var valueLines = dataTable.AsEnumerable()
                .Select(row => string.Join(",", row.ItemArray.Select(val => $"\"{val}\"")));

            lines.AddRange(valueLines);

            File.WriteAllLines(NewFilePath, lines);
        }
    }
}
