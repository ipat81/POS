using System;
using System.Windows.Media;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infragistics.Documents.Excel;
using Infragistics.Controls.Grids;
using Infragistics.Themes;

using BOPr;

namespace Pos2006ReportsNew.ViewModels
{
    public class FoodSaleFromOtherRestaurantsVM
    {
        private SaleDays SdList;
        public DateTime? rptFrom { get; set; }
        public DateTime? rptTo { get; set; }
        public Workbook wb { get; set; }
        public Worksheet ws { get; set; }
        private int CurrentRowNum = 2;
        private const int DATE_COL = 0;
        private const int CUSTOMER_COL = 1;
        private const int TYPE_COL = 2;//Phone, Email, BeyondMenu ETC.
        private const int ITEM_COL = 3;
        private const int QTY_COL = 4;
        private const int PRICE_COL = 5;
        private const int TOTAL_COL = 6;
        private const int REFUND_COL = 7;
        private const int INTERNET_FEES_COL = 8;
        private const int CC_FEES_COL = 9;
        private const int NET_COL = 10;
        private const int REFUND_REASON_COL = 11;

        private CellFillPattern cfpTitle;
        private CellFillPattern cfpColHeader;
        private CellFillPattern cfpRow;
        private CellFillPattern cfpGrandTotalRow;
        private CellFillPattern cfpVoidRow;
        public void LoadData()
        {
            if (rptFrom == null || rptTo == null) return;
            this.CreateRowStyles();
            this.CreateSpreadsheet();
            this.LoadSaleDays();
            //this.LoadOrders();
            //this.LoadPayments();
        }
        public string ReportTitle { get; set; }
        private void CreateRowStyles()
        {
            cfpTitle = CellFill.CreateSolidFill(Colors.LightGray); 
            // create the fill pattern for the table header
            cfpColHeader = CellFill.CreateSolidFill(Colors.AliceBlue);
            // create the fill pattern for the table rows

            // create the fill pattern for the table alternate rows
            cfpGrandTotalRow = CellFillPattern.CreateSolidFill(Colors.LightGray);
            cfpVoidRow = CellFillPattern.CreateSolidFill(Colors.Red);

              wb = new Workbook();
              ws = wb.Worksheets.Add("Crown");
              ws.DisplayOptions.PanesAreFrozen = true; 
              ws.DisplayOptions.FrozenPaneSettings.FrozenRows = 3;
        }

        private void CreateSpreadsheet()
        {
            int smallCol = 2560;
            int medCol = 2560 *2;
            int longCol = 2560 * 3;
            //wb = new Workbook();
            //ws = wb.Worksheets.Add("Crown");
            var titleCells= ws.MergedCellsRegions.Add(0, 0, 0, 11);
            //Title
            this.ReportTitle = "Sale From " + ((DateTime)rptFrom).ToString("d") + " To " + ((DateTime)rptTo).ToString("d");
            titleCells.Value = this.ReportTitle;
            titleCells.CellFormat.Fill = cfpTitle;
            titleCells.CellFormat.Alignment = HorizontalCellAlignment.CenterAcrossSelection;
            titleCells.CellFormat.Font.Bold = ExcelDefaultableBoolean.True;
            titleCells.CellFormat.Font.Height = 300;
            //Column Headers
            ws.Columns[DATE_COL].Width = smallCol;
            ws.Columns[DATE_COL].CellFormat.FormatString = "mm/dd/yy";
            ws.Rows[CurrentRowNum].Cells[DATE_COL].Value = "Date";

            ws.Columns[CUSTOMER_COL].Width = medCol;
            ws.Rows[CurrentRowNum].Cells[CUSTOMER_COL].Value = "Customer";

            ws.Columns[TYPE_COL].Width = smallCol;
            ws.Rows[CurrentRowNum].Cells[TYPE_COL].Value = "Type";

            ws.Columns[ITEM_COL].Width = longCol;
            ws.Rows[CurrentRowNum].Cells[ITEM_COL].Value = "Item";

            ws.Columns[QTY_COL].Width = smallCol;
            ws.Columns[QTY_COL].CellFormat.FormatString = "0";
            ws.Columns[QTY_COL].CellFormat.FormatOptions = WorksheetCellFormatOptions.ApplyNumberFormatting;
            ws.Rows[CurrentRowNum].Cells[QTY_COL].Value = "Qty";

            ws.Columns[PRICE_COL].Width = smallCol;
            ws.Columns[PRICE_COL].CellFormat.FormatString = "\"$\"#,##0.00"; ;
            ws.Columns[PRICE_COL].CellFormat.FormatOptions = WorksheetCellFormatOptions.ApplyNumberFormatting;
            ws.Rows[CurrentRowNum].Cells[PRICE_COL].Value = "Price";

            ws.Columns[TOTAL_COL].Width = smallCol;
            ws.Columns[TOTAL_COL].CellFormat.SetFormatting(ws.Columns[PRICE_COL].CellFormat) ;
            ws.Rows[CurrentRowNum].Cells[TOTAL_COL].Value = "Total";

            ws.Columns[REFUND_COL].Width = smallCol;
            ws.Columns[REFUND_COL].CellFormat.SetFormatting(ws.Columns[PRICE_COL].CellFormat);
            ws.Rows[CurrentRowNum].Cells[REFUND_COL].Value = "Refund";

            ws.Columns[INTERNET_FEES_COL].Width = smallCol;
            ws.Columns[INTERNET_FEES_COL].CellFormat.SetFormatting(ws.Columns[PRICE_COL].CellFormat);
            ws.Rows[CurrentRowNum].Cells[INTERNET_FEES_COL].Value = "Internet Fee";

            ws.Columns[CC_FEES_COL].Width = smallCol;
            ws.Columns[CC_FEES_COL].CellFormat.SetFormatting(ws.Columns[PRICE_COL].CellFormat);
            ws.Rows[CurrentRowNum].Cells[CC_FEES_COL].Value = "CC Fee";

            ws.Columns[NET_COL].Width = smallCol;
            ws.Columns[NET_COL].CellFormat.SetFormatting(ws.Columns[PRICE_COL].CellFormat);
            ws.Rows[CurrentRowNum].Cells[NET_COL].Value = "Net";

            ws.Columns[REFUND_REASON_COL].Width = longCol;
            ws.Rows[CurrentRowNum].Cells[REFUND_REASON_COL].Value = "Refund Reason";
            //ws.Rows[CurrentRowNum].CellFormat.Fill = cfpColHeader;
            ApplyCellFormat(CurrentRowNum,"A", cfpColHeader);

        }
        private void ApplyCellFormat(int CurrentRowNum, string startCol,CellFillPattern cellFillFormat)
        {
            string regionName = startCol + (CurrentRowNum + 1).ToString() + ":L" + (CurrentRowNum + 1).ToString();
            WorksheetRegion region = ws.GetRegion(regionName);
            foreach (WorksheetCell wCell in region)
            {
                wCell.CellFormat.Fill = cellFillFormat;
            }
        }
        private void LoadSaleDays()
        {
            DateTime rptdate = (DateTime)rptFrom;
            SaleDay sd = null;
            do
            {
                SdList = SaleDays.CreateSaleDay(); 
                sd = SdList.get_Item(rptdate, true);
                rptdate = rptdate.AddDays(1);
            } while (rptdate <= rptTo);
            if (SdList.Count > 0)
                LoadOrders();
        }

        private void LoadOrders()
        {
            string curRowString ="";
            double itemTotal, orderTotal,refundTotal, internetFees, ccFees, net;
            WorksheetRow curRow;
            foreach (SaleDay sd in this.SdList)
            {
                foreach (SaleDaySession sdSession in sd.AllSaleDaySessions)
                {
                    foreach (Order o in sdSession.AllOrders)
                    {
                        if (o.POSAmount <= 0)
                            continue;
                        CurrentRowNum++;
                        curRow = ws.Rows[CurrentRowNum];
                        curRow.Cells[DATE_COL].Value = o.Saledate.ToShortDateString();
                        curRow.Cells[CUSTOMER_COL].Value = o.TakeOutCustomerName;
                        ws.Rows[CurrentRowNum + 1].Cells[CUSTOMER_COL].Value = o.TakeOutCustomerAddress;
                        curRow.Cells[TYPE_COL].Value = o.OrderReceivedFrom;
                        orderTotal = 0;
                        refundTotal=0;
                        string orderStartRow = (CurrentRowNum + 1).ToString();
                        foreach (OrderItem item in o.AllOrderItems)
                        {
                            CurrentRowNum++;
                            
                            curRow = ws.Rows[CurrentRowNum];
                            curRow.Cells[ITEM_COL].Value =item.OrderItemName;
                            curRow.Cells[QTY_COL].Value = item.Quantity;
                            if (item.CrownPrice > 0)
                                curRow.Cells[PRICE_COL].Value = item.CrownPrice;
                            else
                                curRow.Cells[PRICE_COL].Value = item.MenuItem.CrownPrice;
                            itemTotal = item.Quantity * item.MenuItem.CrownPrice;
                            orderTotal += itemTotal;
                            if (item.Status == OrderItem.enumStatus.Voided)
                            {
                                ApplyCellFormat(CurrentRowNum, "D", cfpVoidRow);
                                refundTotal += itemTotal;
                                curRow.Cells[REFUND_COL].Value = itemTotal;
                            }
                            //else
                            //{
                            //   refundTotal=0;
                            //}
                            //curRow.Cells[TOTAL_COL].Value = itemTotal;
                            curRow.Cells[TOTAL_COL].ApplyFormula(@"=E" + (CurrentRowNum +1).ToString() + " * F" + (CurrentRowNum +1).ToString());
                            //curRow.Cells[REFUND_COL].Value = 0;  //ToDo: add this to POS orderentry
                            //curRow.Cells[NET_COL].Value = 0;
                        }
                        //write the order total row
                        CurrentRowNum++;
                        curRowString = (CurrentRowNum + 1).ToString();
                        curRow = ws.Rows[CurrentRowNum];
                        curRow.Cells[ITEM_COL].Value = "Total";
                        curRow.Cells[TOTAL_COL].Value = orderTotal;
                        //curRow.Cells[TOTAL_COL].ApplyFormula(@"=sum(G" + orderStartRow.ToString() +
                        //                                      " :G" + curRowString +
                        //                                      " )");
                        curRow.Cells[TOTAL_COL].ApplyFormula(this.GetSumFormula("G",orderStartRow, CurrentRowNum.ToString()));

                        if (o.VoidStatus == Order.enumOrderVoidStatus.Voided)
                        {
                            ApplyCellFormat(CurrentRowNum, "D", cfpVoidRow);
                            internetFees = 0;
                            //refundTotal = orderTotal;
                        }
                        else
                        {
                            ApplyCellFormat(CurrentRowNum, "D", cfpColHeader);
                            //internetFees = orderTotal * 0.1;
                            if (string.IsNullOrEmpty(o.OrderReceivedFrom) || o.OrderReceivedFrom.ToLower().Contains("mg"))
                                curRow.Cells[INTERNET_FEES_COL].Value = 0;
                            else
                                if (string.IsNullOrEmpty(o.OrderReceivedFrom) || o.OrderReceivedFrom.ToLower().Contains("grubhub"))
                                    curRow.Cells[INTERNET_FEES_COL].ApplyFormula(@"=G" + curRowString + " * 0.125");
                                else
                                    curRow.Cells[INTERNET_FEES_COL].ApplyFormula(@"=G" + curRowString + " * 0.1");
                        }

                        curRow.Cells[REFUND_COL].ApplyFormula(this.GetSumFormula("H", orderStartRow, CurrentRowNum.ToString()));
                        if (!String.IsNullOrEmpty(o.PaidBy) && o.PaidBy.Contains("Amex"))
                            curRow.Cells[CC_FEES_COL].ApplyFormula(@"=G" + curRowString + " * 0.045");
                        else
                            curRow.Cells[CC_FEES_COL].ApplyFormula(@"=G" + curRowString + " * 0.035");
                        //curRowString = (CurrentRowNum +1).ToString();
                        curRow.Cells[NET_COL].ApplyFormula(@"=G" + curRowString +
                                                              " - H" + curRowString +
                                                              " - I" + curRowString +
                                                              " - J" + curRowString);
                        curRow.Cells[REFUND_REASON_COL].Value = 0;  //ToDo: add this to POS orderentry
                        if (o.VoidStatus == Order.enumOrderVoidStatus.Voided)
                            ApplyCellFormat(CurrentRowNum,"D", cfpVoidRow);
                        else
                            ApplyCellFormat(CurrentRowNum, "D", cfpColHeader);
                    }
                }
            }
            //write the grand total row
            CurrentRowNum += 5;
            curRow = ws.Rows[CurrentRowNum];
            curRow.Cells[ITEM_COL].Value = "Weekly Total";
            curRow.Cells[NET_COL].ApplyFormula(@"=SUM(K" + (5).ToString() + ":K" + (CurrentRowNum - 4).ToString() + ")");
            ApplyCellFormat(CurrentRowNum, "D", cfpGrandTotalRow);

            CurrentRowNum++;
            curRow = ws.Rows[CurrentRowNum];
            curRow.Cells[ITEM_COL].Value = "Less: MG Fees";
            curRow.Cells[NET_COL].ApplyFormula(@"=K" + (CurrentRowNum).ToString() + " * -0.1" );
            ApplyCellFormat(CurrentRowNum, "D", cfpGrandTotalRow);

            CurrentRowNum++;
            curRow = ws.Rows[CurrentRowNum];
            curRow.Cells[ITEM_COL].Value = "Net Payable";
            curRow.Cells[NET_COL].ApplyFormula(@"=K" + (CurrentRowNum - 1).ToString() + " + K" + (CurrentRowNum).ToString());

            ApplyCellFormat(CurrentRowNum, "D", cfpGrandTotalRow);
        }
        private string GetSumFormula(string colName, string rowFrom, string rowTo)
        {
            return @"=sum(" + colName + rowFrom + " :" + colName + rowTo + " )";
        }
    }
}
