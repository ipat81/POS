using System;
using System.Collections.Generic;

namespace WpfApplication1.Models
{
    public partial class Audit
    {
        public Audit()
        {
            this.AuditChangeSets = new List<AuditChangeSet>();
            this.CashRegisters = new List<CashRegister>();
            this.CustomerContacts = new List<CustomerContact>();
            this.CRItems = new List<CRItem>();
            this.CRItemCounts = new List<CRItemCount>();
            this.CRTxns = new List<CRTxn>();
            this.Customers = new List<Customer>();
            this.DTables = new List<DTable>();
            this.Employees = new List<Employee>();
            this.EmployeeContacts = new List<EmployeeContact>();
            this.EmployeePayInfoes = new List<EmployeePayInfo>();
            this.Escalations = new List<Escalation>();
            this.GCTxns = new List<GCTxn>();
            this.GiftCards = new List<GiftCard>();
            this.Menus = new List<Menu>();
            this.MenuItems = new List<MenuItem>();
            this.ModItems = new List<ModItem>();
            this.ModsAlloweds = new List<ModsAllowed>();
            this.Orders = new List<Order>();
            this.OrderItemMods = new List<OrderItemMod>();
            this.Overrides = new List<Override>();
            this.Payments = new List<Payment>();
            this.PayPeriods = new List<PayPeriod>();
            this.Payrolls = new List<Payroll>();
            this.PayRollAdjusts = new List<PayRollAdjust>();
            this.Portions = new List<Portion>();
            this.PortionGroups = new List<PortionGroup>();
            this.PortionMenus = new List<PortionMenu>();
            this.POSFunctions = new List<POSFunction>();
            this.PosMenuGroups = new List<PosMenuGroup>();
            this.PosMenuItems = new List<PosMenuItem>();
            this.PosMenuItemUIViews = new List<PosMenuItemUIView>();
            this.PosMenuTabs = new List<PosMenuTab>();
            this.PosUIViews = new List<PosUIView>();
            this.Products = new List<Product>();
            this.ProductDefs = new List<ProductDef>();
            this.PromoItems = new List<PromoItem>();
            this.PromoSchedules = new List<PromoSchedule>();
            this.Roles = new List<Role>();
            this.SDs = new List<SD>();
            this.SDSessions = new List<SDSession>();
            this.SEs = new List<SE>();
            this.ServerTipAllocations = new List<ServerTipAllocation>();
            this.Sessions = new List<Session>();
            this.SETasks = new List<SETask>();
            this.Subscribers = new List<Subscriber>();
            this.SubscriberViews = new List<SubscriberView>();
            this.SubscriberViewPosMenuTabs = new List<SubscriberViewPosMenuTab>();
            this.SubscriberViewPosMenuTabGroupMenuItems = new List<SubscriberViewPosMenuTabGroupMenuItem>();
            this.SubscriberViewPosMenuTabGroups = new List<SubscriberViewPosMenuTabGroup>();
            this.TableSeatings = new List<TableSeating>();
            this.Tasks = new List<Task>();
            this.TimeCards = new List<TimeCard>();
        }

        public long Id { get; set; }
        public virtual Audit Audit1 { get; set; }
        public virtual Audit Audit2 { get; set; }
        public virtual ICollection<AuditChangeSet> AuditChangeSets { get; set; }
        public virtual ICollection<CashRegister> CashRegisters { get; set; }
        public virtual ICollection<CustomerContact> CustomerContacts { get; set; }
        public virtual ICollection<CRItem> CRItems { get; set; }
        public virtual ICollection<CRItemCount> CRItemCounts { get; set; }
        public virtual ICollection<CRTxn> CRTxns { get; set; }
        public virtual ICollection<Customer> Customers { get; set; }
        public virtual ICollection<DTable> DTables { get; set; }
        public virtual ICollection<Employee> Employees { get; set; }
        public virtual ICollection<EmployeeContact> EmployeeContacts { get; set; }
        public virtual ICollection<EmployeePayInfo> EmployeePayInfoes { get; set; }
        public virtual ICollection<Escalation> Escalations { get; set; }
        public virtual ICollection<GCTxn> GCTxns { get; set; }
        public virtual ICollection<GiftCard> GiftCards { get; set; }
        public virtual ICollection<Menu> Menus { get; set; }
        public virtual ICollection<MenuItem> MenuItems { get; set; }
        public virtual ICollection<ModItem> ModItems { get; set; }
        public virtual ICollection<ModsAllowed> ModsAlloweds { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
        public virtual ICollection<OrderItemMod> OrderItemMods { get; set; }
        public virtual ICollection<Override> Overrides { get; set; }
        public virtual ICollection<Payment> Payments { get; set; }
        public virtual ICollection<PayPeriod> PayPeriods { get; set; }
        public virtual ICollection<Payroll> Payrolls { get; set; }
        public virtual ICollection<PayRollAdjust> PayRollAdjusts { get; set; }
        public virtual ICollection<Portion> Portions { get; set; }
        public virtual ICollection<PortionGroup> PortionGroups { get; set; }
        public virtual ICollection<PortionMenu> PortionMenus { get; set; }
        public virtual ICollection<POSFunction> POSFunctions { get; set; }
        public virtual ICollection<PosMenuGroup> PosMenuGroups { get; set; }
        public virtual ICollection<PosMenuItem> PosMenuItems { get; set; }
        public virtual ICollection<PosMenuItemUIView> PosMenuItemUIViews { get; set; }
        public virtual ICollection<PosMenuTab> PosMenuTabs { get; set; }
        public virtual ICollection<PosUIView> PosUIViews { get; set; }
        public virtual ICollection<Product> Products { get; set; }
        public virtual ICollection<ProductDef> ProductDefs { get; set; }
        public virtual ICollection<PromoItem> PromoItems { get; set; }
        public virtual ICollection<PromoSchedule> PromoSchedules { get; set; }
        public virtual ICollection<Role> Roles { get; set; }
        public virtual ICollection<SD> SDs { get; set; }
        public virtual ICollection<SDSession> SDSessions { get; set; }
        public virtual ICollection<SE> SEs { get; set; }
        public virtual ICollection<ServerTipAllocation> ServerTipAllocations { get; set; }
        public virtual ICollection<Session> Sessions { get; set; }
        public virtual ICollection<SETask> SETasks { get; set; }
        public virtual ICollection<Subscriber> Subscribers { get; set; }
        public virtual ICollection<SubscriberView> SubscriberViews { get; set; }
        public virtual ICollection<SubscriberViewPosMenuTab> SubscriberViewPosMenuTabs { get; set; }
        public virtual ICollection<SubscriberViewPosMenuTabGroupMenuItem> SubscriberViewPosMenuTabGroupMenuItems { get; set; }
        public virtual ICollection<SubscriberViewPosMenuTabGroup> SubscriberViewPosMenuTabGroups { get; set; }
        public virtual ICollection<TableSeating> TableSeatings { get; set; }
        public virtual ICollection<Task> Tasks { get; set; }
        public virtual ICollection<TimeCard> TimeCards { get; set; }
    }
}
