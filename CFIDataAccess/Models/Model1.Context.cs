﻿//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CFIDataAccess.Models
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class CFIEntities : DbContext
    {
        public CFIEntities()
            : base("name=CFIEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public DbSet<ActionReport> ActionReports { get; set; }
        public DbSet<ActivityData> ActivityDatas { get; set; }
        public DbSet<ActivityList> ActivityLists { get; set; }
        public DbSet<ActivityType> ActivityTypes { get; set; }
        public DbSet<Admin> Admins { get; set; }
        public DbSet<BasicLabor> BasicLabors { get; set; }
        public DbSet<BasicPricing> BasicPricings { get; set; }
        public DbSet<Call> Calls { get; set; }
        public DbSet<Capacity> Capacities { get; set; }
        public DbSet<CapacityPool> CapacityPools { get; set; }
        public DbSet<CapacityPoolType> CapacityPoolTypes { get; set; }
        public DbSet<ChargeBack> ChargeBacks { get; set; }
        public DbSet<CheckCBDetail> CheckCBDetails { get; set; }
        public DbSet<CheckDetail> CheckDetails { get; set; }
        public DbSet<Check> Checks { get; set; }
        public DbSet<CompanyInfo> CompanyInfoes { get; set; }
        public DbSet<ContactTitle> ContactTitles { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<DaysOfYear> DaysOfYears { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<DepartmentsContactTitle> DepartmentsContactTitles { get; set; }
        public DbSet<DepartmentsStoresAssignment> DepartmentsStoresAssignments { get; set; }
        public DbSet<Discrepancy> Discrepancies { get; set; }
        public DbSet<DiscrepanciesPrice> DiscrepanciesPrices { get; set; }
        public DbSet<DiscrepancySubType> DiscrepancySubTypes { get; set; }
        public DbSet<DiscrepancyType> DiscrepancyTypes { get; set; }
        public DbSet<Division> Divisions { get; set; }
        public DbSet<DivisionGroup> DivisionGroups { get; set; }
        public DbSet<dtproperty> dtproperties { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<EntryMethod> EntryMethods { get; set; }
        public DbSet<FurnishedMaterial> FurnishedMaterials { get; set; }
        public DbSet<FurnishedMaterialsThreshold> FurnishedMaterialsThresholds { get; set; }
        public DbSet<Installer_Error> Installer_Errors { get; set; }
        public DbSet<InstallerStatu> InstallerStatus { get; set; }
        public DbSet<Issue> Issues { get; set; }
        public DbSet<IssueStatu> IssueStatus { get; set; }
        public DbSet<IssueType> IssueTypes { get; set; }
        public DbSet<Job> Jobs { get; set; }
        public DbSet<JobAssignment> JobAssignments { get; set; }
        public DbSet<JobDetail> JobDetails { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<Market> Markets { get; set; }
        public DbSet<MarketAndMaterialType> MarketAndMaterialTypes { get; set; }
        public DbSet<Material_Catagory> Material_Catagories { get; set; }
        public DbSet<Material_Status> Material_Status { get; set; }
        public DbSet<MaterialType> MaterialTypes { get; set; }
        public DbSet<MaterialTypesMarketMapping> MaterialTypesMarketMappings { get; set; }
        public DbSet<MatSubCat> MatSubCats { get; set; }
        public DbSet<Measure_Error> Measure_Errors { get; set; }
        public DbSet<MeasureCompCalcData> MeasureCompCalcDatas { get; set; }
        public DbSet<MeasureCompCustData> MeasureCompCustDatas { get; set; }
        public DbSet<MeasureCompLineItemData> MeasureCompLineItemDatas { get; set; }
        public DbSet<MeasureCompOrderData> MeasureCompOrderDatas { get; set; }
        public DbSet<Mill_Error> Mill_Errors { get; set; }
        public DbSet<NoteType> NoteTypes { get; set; }
        public DbSet<OldCarpet> OldCarpets { get; set; }
        public DbSet<OptionPricing> OptionPricings { get; set; }
        public DbSet<Option> Options { get; set; }
        public DbSet<Order_Options_Detail> Order_Options_Details { get; set; }
        public DbSet<OrderBasicLaborDetail> OrderBasicLaborDetails { get; set; }
        public DbSet<OrderBasicLaborDetailsDeleted> OrderBasicLaborDetailsDeleteds { get; set; }
        public DbSet<OrderCustomDetail> OrderCustomDetails { get; set; }
        public DbSet<OrderCustomDetailsDeleted> OrderCustomDetailsDeleteds { get; set; }
        public DbSet<OrderDiagram> OrderDiagrams { get; set; }
        public DbSet<OrderExtra> OrderExtras { get; set; }
        public DbSet<OrderOptionsDetailsDeleted> OrderOptionsDetailsDeleteds { get; set; }
        public DbSet<OrderRegMerchandiseDetail> OrderRegMerchandiseDetails { get; set; }
        public DbSet<OrderRegMerchandiseDetailsDeleted> OrderRegMerchandiseDetailsDeleteds { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrdersDeleted> OrdersDeleteds { get; set; }
        public DbSet<OrderSOMerchandiseDetail> OrderSOMerchandiseDetails { get; set; }
        public DbSet<OrderSOMerchandiseDetailsDeleted> OrderSOMerchandiseDetailsDeleteds { get; set; }
        public DbSet<PayrollMessage> PayrollMessages { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<PermissionType> PermissionTypes { get; set; }
        public DbSet<PhoneNumberType> PhoneNumberTypes { get; set; }
        public DbSet<PONote> PONotes { get; set; }
        public DbSet<POStatusValue> POStatusValues { get; set; }
        public DbSet<PrintedPOData> PrintedPODatas { get; set; }
        public DbSet<ScheduleChangeReasonCode> ScheduleChangeReasonCodes { get; set; }
        public DbSet<Setting> Settings { get; set; }
        public DbSet<SiteAddress> SiteAddresses { get; set; }
        public DbSet<SOSIDocumentType> SOSIDocumentTypes { get; set; }
        public DbSet<SOSIOutgoingDocument> SOSIOutgoingDocuments { get; set; }
        public DbSet<SOSIScheduleWillCallDocument> SOSIScheduleWillCallDocuments { get; set; }
        public DbSet<SOSISentDocument> SOSISentDocuments { get; set; }
        public DbSet<SPNActionQueue> SPNActionQueues { get; set; }
        public DbSet<SPNAction> SPNActions { get; set; }
        public DbSet<SPNImportedPOs> SPNImportedPOs { get; set; }
        public DbSet<SpokeWith> SpokeWiths { get; set; }
        public DbSet<State> States { get; set; }
        public DbSet<StatusDetail> StatusDetails { get; set; }
        public DbSet<Store_Error> Store_Errors { get; set; }
        public DbSet<StoreContact> StoreContacts { get; set; }
        public DbSet<Store> Stores { get; set; }
        public DbSet<StoreType> StoreTypes { get; set; }
        public DbSet<SubContractor> SubContractors { get; set; }
        public DbSet<SubContractorsDivisionAssignment> SubContractorsDivisionAssignments { get; set; }
        public DbSet<sysdiagram> sysdiagrams { get; set; }
        public DbSet<UnitOfMeasure> UnitOfMeasures { get; set; }
        public DbSet<UserMarketDivisionAssignment> UserMarketDivisionAssignments { get; set; }
        public DbSet<UserPermission> UserPermissions { get; set; }
        public DbSet<VendorNumber> VendorNumbers { get; set; }
        public DbSet<Version> Versions { get; set; }
        public DbSet<Week> Weeks { get; set; }
        public DbSet<XMLSource> XMLSources { get; set; }
        public DbSet<POPhoto> POPhotos { get; set; }
    }
}
