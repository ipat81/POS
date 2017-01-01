using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using WpfApplication1.Models.Mapping;

namespace WpfApplication1.Models
{
    public partial class Pos09Context : DbContext
    {
        static Pos09Context()
        {
            Database.SetInitializer<Pos09Context>(null);
        }

        public Pos09Context()
            : base("Name=Pos09Context")
        {
        }

        public DbSet<Audit> Audits { get; set; }
        public DbSet<AuditChangeSet> AuditChangeSets { get; set; }
        public DbSet<AuditDetail> AuditDetails { get; set; }
        public DbSet<CashRegister> CashRegisters { get; set; }
        public DbSet<CRItem> CRItems { get; set; }
        public DbSet<CRItemCount> CRItemCounts { get; set; }
        public DbSet<CRTxn> CRTxns { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<CustomerContact> CustomerContacts { get; set; }
        public DbSet<DTable> DTables { get; set; }
        public DbSet<dtproperty> dtproperties { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<EmployeeContact> EmployeeContacts { get; set; }
        public DbSet<EmployeePayInfo> EmployeePayInfoes { get; set; }
        public DbSet<Escalation> Escalations { get; set; }
        public DbSet<GCTxn> GCTxns { get; set; }
        public DbSet<GiftCard> GiftCards { get; set; }
        public DbSet<Menu> Menus { get; set; }
        public DbSet<MenuItem> MenuItems { get; set; }
        public DbSet<ModItem> ModItems { get; set; }
        public DbSet<ModsAllowed> ModsAlloweds { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<OrderItemMod> OrderItemMods { get; set; }
        public DbSet<Override> Overrides { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<PaymentMethod> PaymentMethods { get; set; }
        public DbSet<PayPeriod> PayPeriods { get; set; }
        public DbSet<Payroll> Payrolls { get; set; }
        public DbSet<PayRollAdjust> PayRollAdjusts { get; set; }
        public DbSet<Portion> Portions { get; set; }
        public DbSet<PortionGroup> PortionGroups { get; set; }
        public DbSet<PortionMenu> PortionMenus { get; set; }
        public DbSet<POSFunction> POSFunctions { get; set; }
        public DbSet<PosMenuGroup> PosMenuGroups { get; set; }
        public DbSet<PosMenuItem> PosMenuItems { get; set; }
        public DbSet<PosMenuItemUIView> PosMenuItemUIViews { get; set; }
        public DbSet<PosMenuTab> PosMenuTabs { get; set; }
        public DbSet<PosUIView> PosUIViews { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductDef> ProductDefs { get; set; }
        public DbSet<PromoCouponIssued> PromoCouponIssueds { get; set; }
        public DbSet<PromoCouponRedeemed> PromoCouponRedeemeds { get; set; }
        public DbSet<PromoItem> PromoItems { get; set; }
        public DbSet<PromoSchedule> PromoSchedules { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<SD> SDs { get; set; }
        public DbSet<SDSession> SDSessions { get; set; }
        public DbSet<SE> SEs { get; set; }
        public DbSet<ServerTipAllocation> ServerTipAllocations { get; set; }
        public DbSet<Session> Sessions { get; set; }
        public DbSet<SETask> SETasks { get; set; }
        public DbSet<Subscriber> Subscribers { get; set; }
        public DbSet<SubscriberView> SubscriberViews { get; set; }
        public DbSet<SubscriberViewPosMenuTab> SubscriberViewPosMenuTabs { get; set; }
        public DbSet<SubscriberViewPosMenuTabGroup> SubscriberViewPosMenuTabGroups { get; set; }
        public DbSet<SubscriberViewPosMenuTabGroupMenuItem> SubscriberViewPosMenuTabGroupMenuItems { get; set; }
        public DbSet<sysdiagram> sysdiagrams { get; set; }
        public DbSet<TableSeating> TableSeatings { get; set; }
        public DbSet<Task> Tasks { get; set; }
        public DbSet<TimeCard> TimeCards { get; set; }
        public DbSet<OrderItemView> OrderItemViews { get; set; }
        public DbSet<OrderView> OrderViews { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new AuditMap());
            modelBuilder.Configurations.Add(new AuditChangeSetMap());
            modelBuilder.Configurations.Add(new AuditDetailMap());
            modelBuilder.Configurations.Add(new CashRegisterMap());
            modelBuilder.Configurations.Add(new CRItemMap());
            modelBuilder.Configurations.Add(new CRItemCountMap());
            modelBuilder.Configurations.Add(new CRTxnMap());
            modelBuilder.Configurations.Add(new CustomerMap());
            modelBuilder.Configurations.Add(new CustomerContactMap());
            modelBuilder.Configurations.Add(new DTableMap());
            modelBuilder.Configurations.Add(new dtpropertyMap());
            modelBuilder.Configurations.Add(new EmployeeMap());
            modelBuilder.Configurations.Add(new EmployeeContactMap());
            modelBuilder.Configurations.Add(new EmployeePayInfoMap());
            modelBuilder.Configurations.Add(new EscalationMap());
            modelBuilder.Configurations.Add(new GCTxnMap());
            modelBuilder.Configurations.Add(new GiftCardMap());
            modelBuilder.Configurations.Add(new MenuMap());
            modelBuilder.Configurations.Add(new MenuItemMap());
            modelBuilder.Configurations.Add(new ModItemMap());
            modelBuilder.Configurations.Add(new ModsAllowedMap());
            modelBuilder.Configurations.Add(new OrderMap());
            modelBuilder.Configurations.Add(new OrderItemMap());
            modelBuilder.Configurations.Add(new OrderItemModMap());
            modelBuilder.Configurations.Add(new OverrideMap());
            modelBuilder.Configurations.Add(new PaymentMap());
            modelBuilder.Configurations.Add(new PaymentMethodMap());
            modelBuilder.Configurations.Add(new PayPeriodMap());
            modelBuilder.Configurations.Add(new PayrollMap());
            modelBuilder.Configurations.Add(new PayRollAdjustMap());
            modelBuilder.Configurations.Add(new PortionMap());
            modelBuilder.Configurations.Add(new PortionGroupMap());
            modelBuilder.Configurations.Add(new PortionMenuMap());
            modelBuilder.Configurations.Add(new POSFunctionMap());
            modelBuilder.Configurations.Add(new PosMenuGroupMap());
            modelBuilder.Configurations.Add(new PosMenuItemMap());
            modelBuilder.Configurations.Add(new PosMenuItemUIViewMap());
            modelBuilder.Configurations.Add(new PosMenuTabMap());
            modelBuilder.Configurations.Add(new PosUIViewMap());
            modelBuilder.Configurations.Add(new ProductMap());
            modelBuilder.Configurations.Add(new ProductDefMap());
            modelBuilder.Configurations.Add(new PromoCouponIssuedMap());
            modelBuilder.Configurations.Add(new PromoCouponRedeemedMap());
            modelBuilder.Configurations.Add(new PromoItemMap());
            modelBuilder.Configurations.Add(new PromoScheduleMap());
            modelBuilder.Configurations.Add(new RoleMap());
            modelBuilder.Configurations.Add(new SDMap());
            modelBuilder.Configurations.Add(new SDSessionMap());
            modelBuilder.Configurations.Add(new SEMap());
            modelBuilder.Configurations.Add(new ServerTipAllocationMap());
            modelBuilder.Configurations.Add(new SessionMap());
            modelBuilder.Configurations.Add(new SETaskMap());
            modelBuilder.Configurations.Add(new SubscriberMap());
            modelBuilder.Configurations.Add(new SubscriberViewMap());
            modelBuilder.Configurations.Add(new SubscriberViewPosMenuTabMap());
            modelBuilder.Configurations.Add(new SubscriberViewPosMenuTabGroupMap());
            modelBuilder.Configurations.Add(new SubscriberViewPosMenuTabGroupMenuItemMap());
            modelBuilder.Configurations.Add(new sysdiagramMap());
            modelBuilder.Configurations.Add(new TableSeatingMap());
            modelBuilder.Configurations.Add(new TaskMap());
            modelBuilder.Configurations.Add(new TimeCardMap());
            modelBuilder.Configurations.Add(new OrderItemViewMap());
            modelBuilder.Configurations.Add(new OrderViewMap());
        }
    }
}
