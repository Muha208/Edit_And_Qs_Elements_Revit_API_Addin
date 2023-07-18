using Autodesk.Revit.UI;
using DLL;
using System;
using System.IO;
using System.Reflection;
using System.Windows.Media.Imaging;

namespace KAITECH_R04
{
    public class KAITECH_R04_Main : IExternalApplication
    {
        #region external application public methods
        Result IExternalApplication.OnStartup(UIControlledApplication application)
        {
            //plugin's main Tab name (the name of tab that will inculd your tools)
            string tabName = "KAITECH-R04";
            //panel name hosted on ribbon tab (descreption of all tools that inside your rebbon)
            string panelAnnotationName = ("KAITECH-R04-Structure");
            //creat tab on Revit UI
            application.CreateRibbonTab(tabName);
            //Creat panel on Revit rebbon tab (that the panel opend after click on rebbon and it will apear the tools)
            var panelAnnotation = application.CreateRibbonPanel(tabName, panelAnnotationName);
            //Create push buttom and populate it with information
            //Location = the namespace.Classname
            //it's need (the tab that you want to put it in it, the name of your button that you need to apear, ,The class file (namespace+main class) that include your code of this pushbutton)
            //this code for insert data only
            var mTools = new PushButtonData(tabName, "KAITECH_R04", Assembly.GetExecutingAssembly().Location, "KAITECH_R04.Commands")
            {
                //This is the Bitmap Image will appeared in Rebbon (small one)
                ToolTipImage = new BitmapImage(new Uri($@"{LogDirectors.MianIconPath}")),
                ToolTip = "KAITECH_R04 Tool"
            };
            //but this this code to create the pushbutton that will include your data
            //this is the main bitmap (larg 350x350 px)
            var mToolslayer = panelAnnotation.AddItem(mTools) as PushButton;
            mToolslayer.LargeImage = new BitmapImage(new Uri($@"{LogDirectors.LargeIconPath}"));

            return Result.Succeeded;
        }
        Result IExternalApplication.OnShutdown(UIControlledApplication application)
        {
            return Result.Succeeded;
        }
        #endregion
    }
}
