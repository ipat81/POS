using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
using Infragistics.Documents.Excel;
using Infragistics.Controls.Grids;
using Pos2006ReportsNew.ViewModels;

namespace Pos2006ReportsNew
{
    /// <summary>
    /// Interaction logic for FoodSaleFromOtherRestaurants.xaml
    /// </summary>
    public partial class FoodSaleFromOtherRestaurants : UserControl
    {
        public FoodSaleFromOtherRestaurants()
        {
            InitializeComponent();
            this.DataContext = new FoodSaleFromOtherRestaurantsVM();
        }
        private FoodSaleFromOtherRestaurantsVM VM
        {
            get
            {
                return this.DataContext as FoodSaleFromOtherRestaurantsVM;
            }

        }
        private void periodStart_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            this.VM.rptFrom = e.NewValue as DateTime?;
        }
        private void periodEnd_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            this.VM.rptTo = e.NewValue as DateTime?;
        }
        private void runButton_Click(object sender, RoutedEventArgs e)
        {
            this.VM.LoadData();
            this.XamSpreadSheet1.Workbook = this.VM.wb;

        }
        private void printButton_Click(object sender, RoutedEventArgs e)
        {
            //can save it to only the debug folder
            string fileName = "Summary of MG-Crown Sale From " + this.VM.rptFrom.Value.Month + "-" + this.VM.rptFrom.Value.Day + "-" + this.VM.rptFrom.Value.Year +
                              " To " + this.VM.rptTo.Value.Month + "-" + this.VM.rptTo.Value.Day + "-" + this.VM.rptTo.Value.Year + ".xls";
            this.XamSpreadSheet1.Workbook.Save(fileName);
        }


    }
}
