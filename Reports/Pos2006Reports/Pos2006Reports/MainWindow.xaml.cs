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
using Infragistics.Windows.DataPresenter.ExcelExporter;
using Infragistics.Documents.Excel;
using Infragistics.Windows.DataPresenter;
using Infragistics.Windows.Reporting;

namespace Pos2006Reports
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            this.DataContext = new MainWindowVM();
            InitializeComponent();
        }
        private MainWindowVM VM
        {
             get
             {
                 return this.DataContext as MainWindowVM;
            }

        }
        private void runButton_Click(object sender, RoutedEventArgs e)
        {
            this.VM.LoadReportData();

        }

        private void printButton_Click(object sender, RoutedEventArgs e)
        {
            Report report1 = new Report();
            EmbeddedVisualReportSection section1 = new EmbeddedVisualReportSection(this.dg1);
            report1.Sections.Add(section1);

            report1.ReportSettings.HorizontalPaginationMode = HorizontalPaginationMode.Scale;
            report1.Print();

        }
        private void exportButton_Click(object sender, RoutedEventArgs e)
        {
            DataPresenterExcelExporter exporter = new DataPresenterExcelExporter();
            exporter.ExportAsync(this.dg1,"C:\\Users\\Himanshu\\Documents\\SaleAllocationReports\\PosSaleReport.xlsx", WorkbookFormat.Excel2007);
        }


        private void periodStart_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            this.VM.rptFrom = e.NewValue as DateTime?;
        }
        private void periodEnd_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            this.VM.rptTo = e.NewValue as DateTime?;
        }

        private void dg1_FieldLayoutInitialized(object sender, Infragistics.Windows.DataPresenter.Events.FieldLayoutInitializedEventArgs e)
        {
            foreach (Field f in dg1.FieldLayouts[0].Fields)
            {
                SummaryDefinition sum = new SummaryDefinition();
                sum.SourceFieldName = f.Name; 
                sum.Calculator = SummaryCalculator.Sum;
                sum.StringFormat = "{0:c}";
                this.dg1.FieldLayouts[0].SummaryDefinitions.Add(sum);
            }

        }
    }
}
