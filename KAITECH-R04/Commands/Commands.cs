using Autodesk.Revit.Creation;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace KAITECH_R04
{
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    [Autodesk.Revit.Attributes.Regeneration(Autodesk.Revit.Attributes.RegenerationOption.Manual)]
    public class Commands : IExternalCommand
    {
        public static ExternalEvent exEvent;
        public static UIDocument uidoc;
        public static Autodesk.Revit.DB.Document document;
        Result IExternalCommand.Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {

            exEvent = ExternalEvent.Create(new EventHandler());
            uidoc = commandData.Application.ActiveUIDocument;
            document = uidoc.Document;
            try
            {
                //to make it an active veiw
                MainWindow mainWindow = new MainWindow();
                mainWindow.Show();
                return Result.Succeeded;
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message);
                return Result.Failed;
            }
        }
    }
}
