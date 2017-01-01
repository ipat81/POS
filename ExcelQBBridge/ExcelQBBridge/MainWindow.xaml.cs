using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using LinqToExcel;

namespace ExcelQBBridge
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.LoadExpenseRows();
        }

        private void LoadExpenseRows()
        {
            string fullFN = System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(), "Summary.xls");
            var excel = new ExcelQueryFactory(fullFN);
            var txns = from c in excel.Worksheet<ExpenseTxn>("Transaction Details") //worksheet name = 'US Companies'
                               select c;
            foreach (ExpenseTxn txn in txns)
            {
                this.SetAmexTxn(txn);
                if (!string.IsNullOrEmpty(txn.qbAccount))
                    this.PostGJEntry(txn);

            }

        }

        private void PostGJEntry(ExpenseTxn txn)
        {
            throw new NotImplementedException();
        }

        private void SetAmexTxn(ExpenseTxn txn)
        {
            throw new NotImplementedException();
        }

        public string FileName { get; set; }
    }

    public class ExpenseTxn
    {
        public DateTime Date { get; set; }
        public string Receipt { get; set; }
        public string Description { get; set; }
        public string Cardmember { get; set; }
        public string Account { get; set; }
        public string Amount { get; set; }

        public string qbAccount { get; set; }
    }
}
