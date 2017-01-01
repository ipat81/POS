using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BOPr;


namespace Pos2006Reports
{
    public class CrownReportData
    {
        public Order SaleOrder { get; set; }
        public OrderItem SaleOrderItem { get; set; }
        public DateTime OrderAt { get { return SaleOrder.FirstKOTPrintedAt; } }
        public string OrderNum { get { return SaleOrder.OrderNumber; } }
        public double OurPrice { get { return SaleOrderItem.Price; } }
        public double CrownPrice { get { return SaleOrderItem.MenuItem.CrownPrice; } }
        public double OrderItemTotal { get { return SaleOrderItem.Quantity * OnlinePrice; } }
        public string CustName { get { return SaleOrder.TakeOutCustomerName; } }
        public string CustAddress { get { return SaleOrder.TakeOutCustomerAddress; } }
        public string CustPhone { get { return SaleOrder.TakeOutCustomerPhone; } }
        
        public string OrderType
        {
            get
            {
                if (SaleOrder.IsHouseACOrder || CustName.ToLower().Contains ("beyond")
                    || CustName.ToLower().Contains ("menu")
                    || CustName.ToLower().Contains ("eat")
                    || CustName.ToLower().Contains ("grub"))
                    return "Online" ;
                else
                    return "MG" ;
            }
        }
        public double OnlinePrice 
        {
            get
            {
                if (OrderType == "Online")
                    return CrownPrice * .9;
                else
                    return CrownPrice;
            }
        }

    }
}
