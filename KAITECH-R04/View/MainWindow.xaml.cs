using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Dll;
using DLL;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Messaging;
using System.Security.Policy;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace KAITECH_R04
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static TextBlock LogBox;
        public static DataGrid dataGrid;
        public static DataGrid InterscDataGrid;
        public static string SelectionFilterComboBox;
        public static List<List<string>> ListOFRowValues;
        public static List<string> ListOFComboBox;
        public static List<Element> ListOfColumnsElements;
        public static List<Element> FilterdListElements;
        public static System.Data.DataTable MainDataTable;
        public static Button AllElementButton;
        public static Button EditGridsB;
        public static Button ColElementButton;
        public static System.Windows.Controls.ComboBox MainCoboBox;
        public static System.Windows.Controls.ComboBox ViewCoboBox;
        public static object Bsender;
        public static object Csender;
        ExternalEvent exEvent = Commands.exEvent;

        public MainWindow()
        {
            InitializeComponent();
            IconTool.Source = new BitmapImage(new Uri(LogDirectors.MianIconPath));
            dataGrid = DataTestGrid;
            LogBox = Tx_Log;
            ViewCoboBox = Views_cb;
            EditGrids_Bt.IsEnabled = false;
            EditGridsB = EditGrids_Bt;
            AllElementButton = SelectAllElement_Bt;
            MainCoboBox = Category_cb;
            RevitElementsMethods.GetElements();
        }
        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {

            if (e.ButtonState == MouseButtonState.Pressed)
            {
                this.DragMove();
            }
        }
        #region Header Close-Min-Max Buttons

        private void Close_Tip_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }
        private void Max_Tip_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (this.WindowState == WindowState.Maximized)
            {
                this.WindowState = WindowState.Normal;
            }
            else
                this.WindowState = WindowState.Maximized;
        }
        private void Mini_Tip_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }
        #endregion

        private void SelectOneElement_Bt_Click(object sender, RoutedEventArgs e)
        {
            Bsender = sender;

            EventHandler.methodsName = MethodsName.FillingAndCreateLevels;
            exEvent.Raise();
        }
        private void CreateSheetsClick_Bt(object sender, RoutedEventArgs e)
        {
            EventHandler.methodsName = MethodsName.CreateSheets;
            exEvent.Raise();
        }

        private void GetIntersectioPoint_Bt_Click(object sender, RoutedEventArgs e)
        {
            InterscDataGrid = DataIntersectionGrid;
            EventHandler.methodsName = MethodsName.FillingIntersecTable;
            exEvent.Raise();

        }

        private void GridDim_Bt_Click(object sender, RoutedEventArgs e)
        {
            EventHandler.methodsName = MethodsName.GridDimension;
            exEvent.Raise();
        }

        private void ColDim_Bt_Click(object sender, RoutedEventArgs e)
        {
            Csender = sender;
            AllElementButton = SelectAllElement_Bt;
            ColElementButton = ColDim_Bt;
            EventHandler.methodsName = MethodsName.ColumnAxisDimension;
            exEvent.Raise();
        }

        private void ExData_Bt_Click(object sender, RoutedEventArgs e)
        {
            EventHandler.methodsName = MethodsName.ExportDataToExcelFile;
            exEvent.Raise();
        }

        private void EditGrids_Bt_Click(object sender, RoutedEventArgs e)
        {
            EventHandler.methodsName = MethodsName.EditGrids;
            exEvent.Raise();
        }

        private void ExDData_Bt_Click(object sender, RoutedEventArgs e)
        {
            EventHandler.methodsName = MethodsName.ExportDetailedDataToExcelFile;
            exEvent.Raise();
        }

        private void Category_cb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SelectionFilterComboBox = Category_cb.SelectedValue.ToString();
            WPFControlsMethods.FillingTableAcoordingClass();

        }

        private void Views_cb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Views_cb.ItemsSource = RevitElementsMethods.FloorEleName;
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            EditGrids_Bt.IsEnabled = true;
        }

        private void chk_Unchecked(object sender, RoutedEventArgs e)
        {
            EditGrids_Bt.IsEnabled = false;
        }

        private void DataTestGrid_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            WPFControlsMethods.FillingTextColumnShape(ColumnH_tx, OwneRevitMathods.InformationClass.CategoryInformation
                .FirstOrDefault(x => x.CategoriesName == "Structural Columns"), DataTestGrid);
        }
    }
}

