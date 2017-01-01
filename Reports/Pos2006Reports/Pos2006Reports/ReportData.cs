using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BOPr;

namespace Pos2006Reports
{
    public class ReportData
    {

        public SaleDay sd ;
        public DateTime SaleDate { get; set; }
        public double BaseAllocationPercent { get; set; }
        public double TipAllocationPercent { get; set; }


        public ReportData()
        {
        //   this.BaseAllocationPercent = .34765; // general
        //this.BaseAllocationPercent = .2142; // FOR 09/2014
        //this.BaseAllocationPercent = .05; // FOR 09/2014
            //Following allocations used until end of may 2016
        //this.BaseAllocationPercent = .2; // FOR 10/2014 + 11/2014
        //this.TipAllocationPercent = .4852; //(out of total $100 cash tip , $48.52 of tip would be allocated to QB2)
            //Following allocations used from July2016 onward
            this.BaseAllocationPercent = 1.0; // FOR 10/2014 + 11/2014
        this.TipAllocationPercent =1.0; //(out of total $100 cash tip , $48.52 of tip would be allocated to QB2)

        }

        //All (non-voided) orders which are paid (by any payment method including House Account, any CC, GC, Check) are included in calculating sale.
        //So, To calculate total sale, we need to add only QB sales (i.e for catering orders or invoices entered in QB) to the numbers produced by this report. 

        // Taxable sale booked in POS
        //NonCash sale are all orders paid by check, CC , Gift Certificate or House Account. Split payments are split by Cash or Non Cash to calculate Cash or NonCash sale.
        public double TaxablePosAmountPaidByCashTotal; // includes base + tax

        //POS does not capture Base  & ST amounts separately by PaymentMethod so we have to split PosAmount into Base & tax
        public double TaxableBasePaidByCashTotal 
         {
            get
            {
                return (this.TaxablePosAmountPaidByCashTotal / 1.07); 
            }
        }
        public double TaxableSTPaidByCashTotal 
         {
            get
            {
                return (this.TaxablePosAmountPaidByCashTotal - this.TaxableBasePaidByCashTotal); 
            }
        }
      public double TaxableTipPaidByCashTotal { get; set; }
        public double TaxablePosAmountPaidByNonCashTotal; // { get; set; } // includes base + tax
        public double TaxableBasePaidByNonCashTotal 
          {
            get
            {
                return (this.TaxablePosAmountPaidByNonCashTotal / 1.07); 
            }
        }
        public double TaxableSTPaidByNonCashTotal
        {
            get
            {
                return (this.TaxablePosAmountPaidByNonCashTotal - this.TaxableBasePaidByNonCashTotal);
            }
        }
        public double TaxableTipPaidByNonCashTotal { get; set; }

        //ST Exempt sale booked in POS
        //NonCash sale are all orders paid by check, CC , Gift Certificate or House Account. Split payments are split by Cash or Non Cash to calculate Cash or NonCash sale.
        public double STExemptBasePaidByCashTotal { get; set; }
        public double STExemptTipPaidByCashTotal { get; set; }
        //public double STExemptSTPaidByCashTotal { get; set; }

        //ST exempt sale paid by (mostly) PU Visa (excludes sale paid by house account and cash) 
        public double STExemptBasePaidByNonCashTotal { get; set; }
        public double STExemptTipPaidByNonCashTotal { get; set; }



        //Allocated to QB2 
        private double TaxableBasePaidByCashAllocatedToQB2 
        {
            get
            {
                return (TaxableBasePaidByCashTotal * BaseAllocationPercent); 
            }
        }
        private double TaxableTipPaidByCashAllocatedToQB2 
        {
            get
            {
                return (TaxableTipPaidByCashTotal * TipAllocationPercent); 
            }
        }
        private double  TaxableSTPaidByCashAllocatedToQB2
        {
            get
            {
                return (TaxableBasePaidByCashAllocatedToQB2 * .07);
            }
        }
        //public double TaxableBasePaidByNonCashAllocatedToQB2 { get; set; }
        //public double TaxableTipPaidByNonCashAllocatedToQB2 { get; set; }
        //public double TaxableSTPaidByNonCashAllocatedToQB2 { get; set; }

        //Cash allocated to be deposited to Bank 
        private double TotalCashIn
        {
            get
            {
                return (TaxableBasePaidByCashTotal + TaxableSTPaidByCashTotal + TaxableTipPaidByCashTotal);

            }
        }
        public double CashAllocatedToQB2
        {
            get
            {
                return (TaxableBasePaidByCashAllocatedToQB2 + TaxableTipPaidByCashAllocatedToQB2 + TaxableSTPaidByCashAllocatedToQB2);
            }
        }
        public double CashAllocatedToBankDeposit
        {
            get
            {
                return (TaxableBasePaidByCashTotal + TaxableSTPaidByCashTotal) - 
                       (TaxableBasePaidByCashAllocatedToQB2 + TaxableSTPaidByCashAllocatedToQB2);
            }
        }

        //Tip that must be paid out to staff in the payroll as Reported Payroll Tips in QB payroll.
        public double ReportedPayrollTips
        {
            get
            {
                return (TaxableTipPaidByNonCashTotal);
            }
        }
        //Tip that must be paid out to staff in cash everyday : goes on payroll checks as Reported Cash Tips in QB payroll.
        //This should not be deposited to bank.
        public double ReportedCashTips
        {
            get
            {
                return (TaxableTipPaidByCashTotal - TaxableTipPaidByCashAllocatedToQB2);
            }
        }

        //Gross Sale from POS to be included in sales tax return 
        //Note this is only POS part of Gross, catering sale booked in QB & POS eat in sale charged to House Account 
        //must be added for ST reporting
        public double GrossSaleST51
        {
            get
            {
                return (TaxableBasePaidByCashTotal - TaxableBasePaidByCashAllocatedToQB2) +
                        TaxableBasePaidByNonCashTotal +
                        STExemptBasePaidByCashTotal +
                        STExemptBasePaidByNonCashTotal +
                        STExemptTipPaidByCashTotal +
                        STExemptTipPaidByNonCashTotal;
            }
        }
        //Deductions from Sale to be included in sales tax return 
        //Note this is only POS part of Deductions, st exempt catering sale booked in QB & POS eat in sale charged to House Account
        //must be added for ST reporting
        public double DeductionsST51
        {
            get
            {
                return STExemptBasePaidByCashTotal +
                        STExemptTipPaidByCashTotal +
                        STExemptBasePaidByNonCashTotal +
                        STExemptTipPaidByNonCashTotal;
            }
        }
        #region verify retport columns
        //ST exempt sale charged to house account and billed from QB 
        public double STExemptBasePaidByHouseAccountTotal { get; set; }
        public double STExemptTipPaidByHouseAccountTotal { get; set; }
        
        //Total Base, Tip, Tax and Payments to verify this report matches with QB rev report
        public double TotalBase
        {
            get
            {
                return (TaxableBasePaidByCashTotal) +
                        TaxableBasePaidByNonCashTotal +
                        STExemptBasePaidByCashTotal +
                        STExemptBasePaidByNonCashTotal +
                        STExemptBasePaidByHouseAccountTotal;
            }
        }
        public double TotalTip
        {
            get
            {
                return (TaxableTipPaidByCashTotal) +
                        TaxableTipPaidByNonCashTotal +
                        STExemptTipPaidByCashTotal +
                        STExemptTipPaidByNonCashTotal +
                        STExemptTipPaidByHouseAccountTotal;
            }
        }
        public double TotalST
        {
            get
            {
                return (TaxableSTPaidByCashTotal) +
                        TaxableSTPaidByNonCashTotal ;
            }
        }

        #endregion
        public void LoadRevenueData()
        {
            List<Payment.enumPaymentMethod>  PaymentMethods = Enum.GetValues(typeof(Payment.enumPaymentMethod)).Cast<Payment.enumPaymentMethod>().ToList();
             sd.SetAllCurrentPayments(sd.AllSaleDaySessions.get_Item(Session.enumSessionName.Lunch).SaleDaySessionFrom, sd.AllSaleDaySessions.get_Item(Session.enumSessionName.Dinner).SaleDaySessionTo);
             sd.SetAllNonCurrentPayments(sd.AllSaleDaySessions.get_Item(Session.enumSessionName.Lunch).SaleDaySessionFrom, sd.AllSaleDaySessions.get_Item(Session.enumSessionName.Dinner).SaleDaySessionTo);
             double oPosamt = 0;
            foreach (SaleDaySession sdSession in sd.AllSaleDaySessions)
            {
                //sd.SetAllCurrentPayments(sdSession.SaleDaySessionFrom, sdSession.SaleDaySessionTo);
                //sd.SetAllNonCurrentPayments(sdSession.SaleDaySessionFrom, sdSession.SaleDaySessionTo);
                foreach (Order o in sdSession.AllOrders)
                {
                    oPosamt = 0;

                    switch (o.SalesTaxStatus)
                    {
                        case Order.enumOrderSalesTaxStatus.Taxable:
                             foreach (var pm in PaymentMethods)
                            {
                                switch (pm)
                                {
                                    case Payment.enumPaymentMethod.Cash:
                                        oPosamt += o.get_PosAmountByPaymentMethod((int)pm);
                                               this.TaxablePosAmountPaidByCashTotal += o.get_PosAmountByPaymentMethod((int)pm);
                                                this.TaxableTipPaidByCashTotal += o.get_TipAmountByPaymentMethod((int)pm);
                                                break;
                                   default:
                                        oPosamt += o.get_PosAmountByPaymentMethod((int)pm);
                                                this.TaxablePosAmountPaidByNonCashTotal += o.get_PosAmountByPaymentMethod((int)pm);
                                                this.TaxableTipPaidByNonCashTotal += o.get_TipAmountByPaymentMethod((int)pm);
                                                break;  
                                }
                             }
                            break;
                        default:
                             foreach (var pm in PaymentMethods)
                                {
                                    switch (pm)
                                    {
                                        case Payment.enumPaymentMethod.Cash:
                                                                                  oPosamt += o.get_PosAmountByPaymentMethod((int)pm);
                                                     this.STExemptBasePaidByCashTotal += o.get_PosAmountByPaymentMethod((int)pm);
                                                    this.STExemptTipPaidByCashTotal += o.get_TipAmountByPaymentMethod((int)pm);
                                                    break;
                                        case Payment.enumPaymentMethod.HouseAccount:
                                                    oPosamt += o.get_PosAmountByPaymentMethod((int)pm);
                                                    this.STExemptBasePaidByHouseAccountTotal += o.get_PosAmountByPaymentMethod((int)pm);
                                                    this.STExemptTipPaidByHouseAccountTotal += o.get_TipAmountByPaymentMethod((int)pm);
                                                    break;
                                        default:
                                        oPosamt += o.get_PosAmountByPaymentMethod((int)pm);
                                                   this.STExemptBasePaidByNonCashTotal += o.get_PosAmountByPaymentMethod((int)pm);
                                                    this.STExemptTipPaidByNonCashTotal += o.get_TipAmountByPaymentMethod((int)pm);
                                                    break;  
                                    }
                                 }

                             break;
                    }
                    var diff1 = Math.Abs( oPosamt - o.OrderBaseAmount - o.OrderSalesTaxAmount - o.DiscountAmount);
                    var diff2 = Math.Abs( oPosamt - o.POSAmountPaid);
                    var err= " ";
                    if ( diff2 > 1 || diff1 > 1)
                        err = "???";
                }

            }
        }
        public void LoadEatOutData()
        {
            List<Payment.enumPaymentMethod> PaymentMethods = Enum.GetValues(typeof(Payment.enumPaymentMethod)).Cast<Payment.enumPaymentMethod>().ToList();
            sd.SetAllCurrentPayments(sd.AllSaleDaySessions.get_Item(Session.enumSessionName.Lunch).SaleDaySessionFrom, sd.AllSaleDaySessions.get_Item(Session.enumSessionName.Dinner).SaleDaySessionTo);
            sd.SetAllNonCurrentPayments(sd.AllSaleDaySessions.get_Item(Session.enumSessionName.Lunch).SaleDaySessionFrom, sd.AllSaleDaySessions.get_Item(Session.enumSessionName.Dinner).SaleDaySessionTo);
            double oBaseDelivery = 0;
            double oBaseTakeOut = 0;
            double oTipDelivery = 0;
            double oTipTakeOut = 0;
            foreach (SaleDaySession sdSession in sd.AllSaleDaySessions)
            {
                //sd.SetAllCurrentPayments(sdSession.SaleDaySessionFrom, sdSession.SaleDaySessionTo);
                //sd.SetAllNonCurrentPayments(sdSession.SaleDaySessionFrom, sdSession.SaleDaySessionTo);
                foreach (Order o in sdSession.AllOrders)
                {
                    oBaseDelivery = 0;
                    oBaseTakeOut = 0;
                    switch (o.OrderType)
                    {
                        case Order.enumOrderType.Delivery:
                            oBaseDelivery += o.OrderBaseAmount;
                            oTipDelivery += o.TipAmountPaid;
                            break;
                        case Order.enumOrderType.Pickup:
                            oBaseTakeOut += o.OrderBaseAmount;
                            oTipTakeOut += o.TipAmountPaid;
                            break;

                    }
                }

            }
        }

    }
}
