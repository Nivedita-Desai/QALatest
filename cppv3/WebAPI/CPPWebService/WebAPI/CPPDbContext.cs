using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using WebAPI.Models;
using WebAPI.Configuration;
using WebAPI.Models.StoredProcedure;

using WebAPI.Models.Api;
using System.Threading.Tasks;

namespace WebAPI
{
    public class CPPDbContext : DbContext
    {
        public CPPDbContext()
            : base("CPP_MySQL")
        {
            this.Configuration.LazyLoadingEnabled = false;
        }
        //asfas
        public override int SaveChanges()
        {
            AddTimestamps();
            return base.SaveChanges();
        }

        public override async Task<int> SaveChangesAsync()
        {
            AddTimestamps();
            return await base.SaveChangesAsync();
        }

        private void AddTimestamps()
        {
            var entities = ChangeTracker.Entries().Where(x => x.Entity is Audit && (x.State == EntityState.Added || x.State == EntityState.Modified));

            var currentUsername = !string.IsNullOrEmpty(System.Web.HttpContext.Current?.User?.Identity?.Name)
                ? HttpContext.Current.User.Identity.Name
                : "Anonymous";

            foreach (var entity in entities)
            {
                if (entity.State == EntityState.Added)
                {
                    ((Audit)entity.Entity).CreatedDate = DateTime.UtcNow;
                    ((Audit)entity.Entity).CreatedBy = currentUsername;
                }

                ((Audit)entity.Entity).UpdatedDate = DateTime.UtcNow;
                ((Audit)entity.Entity).UpdatedBy = currentUsername;
            }
        }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            /*
            modelBuilder.Entity<ProgramElement>()
                .HasRequired(p => p.Program)
                .WithMany()
                .HasForeignKey(p => p.ProgramID);
             */
            //  modelBuilder.Configurations.Add(new ProjectFundConfiguration());

            //Luan experimental - fix migration error "the model backing the..."
            Database.SetInitializer<CPPDbContext>(null);

            base.OnModelCreating(modelBuilder);

            //prevent a new table from being added since we added a mapping
            modelBuilder.Entity<Program>().ToTable("program");
            modelBuilder.Entity<ProgramElement>().ToTable("program_element");
            modelBuilder.Entity<Project>().ToTable("Project");
            modelBuilder.Entity<ProjectLocation>().ToTable("project_location");
            modelBuilder.Entity<GeoLocation>().ToTable("geo_location");
            modelBuilder.Entity<GeoCoordinate>().ToTable("geo_coordinate");
            modelBuilder.Entity<Trend>().ToTable("trend");
            modelBuilder.Entity<TrendStatus>().ToTable("trend_status");
            modelBuilder.Entity<ProgramFund>().ToTable("program_fund");
            modelBuilder.Entity<TrendFund>().ToTable("trend_fund");
            // modelBuilder.Configurations.Add(new ActivityCategoryConfiguration());
            modelBuilder.Entity<ProjectScope>().ToTable("project_scope");

            //Add asset management

            modelBuilder.Entity<Facility>().ToTable("facility");
            modelBuilder.Entity<FacilityAsset>().ToTable("facility_asset");
            modelBuilder.Entity<Facility>().ToTable("facility");
            modelBuilder.Entity<AssetSupplier>().ToTable("asset_supplier");
            modelBuilder.Entity<AssetHistory>().ToTable("asset_history");
            modelBuilder.Entity<AssetComponent>().ToTable("asset_component");
            modelBuilder.Entity<AssetComponentHistory>().ToTable("asset_component_history");
            modelBuilder.Entity<User>().ToTable("user");

            modelBuilder.Entity<RandomToken>().HasKey(r => r.UserID);
            modelBuilder.Entity<User>().HasOptional(r => r.RandomToken).WithRequired(u => u.User);

            modelBuilder.Entity<Organization>().ToTable("organization");
            modelBuilder.Entity<FundType>().ToTable("fund_types");
            modelBuilder.Entity<PhaseCode>().ToTable("phase_lookup");
            modelBuilder.Entity<UnitType>().ToTable("unit_type");
            modelBuilder.Entity<FTEPosition>().ToTable("fte_position");
            modelBuilder.Entity<ActivityCategory>().ToTable("activity_category");
            modelBuilder.Entity<Employee>().ToTable("employee");
            modelBuilder.Entity<CostTrackType>().ToTable("cost_track_type");
            modelBuilder.Entity<Material>().ToTable("material");
            modelBuilder.Entity<Activity>().ToTable("activity");
            modelBuilder.Entity<CostFTE>().ToTable("cost_fte");
            modelBuilder.Entity<CostLumpsum>().ToTable("cost_lumpsum");
            modelBuilder.Entity<CostUnit>().ToTable("cost_unitcost");
            modelBuilder.Entity<CostODC>().ToTable("cost_odc");
            modelBuilder.Entity<CostPercentage>().ToTable("cost_percentagebasis");
            modelBuilder.Entity<ProgramCategory>().ToTable("program_category");

            modelBuilder.Entity<ProjectType>().ToTable("project_type");
            modelBuilder.Entity<ProjectClass>().ToTable("project_class");
            modelBuilder.Entity<ServiceClass>().ToTable("services"); //services table
            modelBuilder.Entity<Prime>().ToTable("primes"); //primes table

            modelBuilder.Entity<CertifiedPayroll>().ToTable("certified_payroll");    //Vaishnavi 12-04-2022
            modelBuilder.Entity<ProgramCertifiedPayroll>().ToTable("program_certifiedroll");
            modelBuilder.Entity<ProgramPrevailingWage>().ToTable("program_prevailing_wage");
            modelBuilder.Entity<Wrap>().ToTable("wrap");
            modelBuilder.Entity<Holidays>().ToTable("holidays");
            modelBuilder.Entity<ProgramWrap>().ToTable("program_wrap");
            modelBuilder.Entity<ProgramWarranty>().ToTable("program_warranty");    //Vaishnavi 12-04-2022
            //modelBuilder.Entity<ProjectClassPhase>().ToTable("project_class_phase");
            modelBuilder.Entity<ServiceToSubserviceMapping>().ToTable("project_class_phase");

            modelBuilder.Entity<Contract>().ToTable("contract");
            modelBuilder.Entity<ProgramContract>().ToTable("program_contract");

            //views
            modelBuilder.Entity<TestView>().ToTable("testView");
            modelBuilder.Entity<CurrentProjectTrend>().ToTable("currentproject");
            modelBuilder.Entity<ODCType>().ToTable("odc_type");
            modelBuilder.Entity<TemporaryCost>().ToTable("temporary_cost");
            modelBuilder.Entity<CostLineItemTracker>().ToTable("cost_line_item_tracker");
            modelBuilder.Entity<ProjectNumber>().ToTable("project_number");
            modelBuilder.Entity<HistoricalActualCost>().ToTable("historical_actual_cost");
            modelBuilder.Entity<ApiError>().ToTable("api_error");
            modelBuilder.Entity<ApiLog>().ToTable("api_log");
            modelBuilder.Entity<CostType>().ToTable("cost_type");
            modelBuilder.Entity<CostRateType>().ToTable("cost_rate_type");
            modelBuilder.Entity<CostOverhead>().ToTable("cost_overhead");
            modelBuilder.Entity<TrendCostOverhead>().ToTable("trend_cost_overhead");
            modelBuilder.Entity<CostTypeRateType>().ToTable("costtype_ratetype");
            modelBuilder.Entity<PurchaseOrder>().ToTable("purchase_order");
            modelBuilder.Entity<PurchaseOrderDetail>().ToTable("purchase_order_detail");
            modelBuilder.Entity<POODCEmployeeDetails>().ToTable("poodcemployeedetails");
            modelBuilder.Entity<LineOfBusiness>().ToTable("line_of_business");
            modelBuilder.Entity<LobPhase>().ToTable("lob_phase");
            modelBuilder.Entity<CostOverheadType>().ToTable("cost_overhead_type");
            modelBuilder.Entity<MFAConfiguration>().ToTable("mfa_configuration_detail");
            modelBuilder.Entity<EmployeeTimeCard>().ToTable("employee_time_card");
            modelBuilder.Entity<ProjectApproversDetails>().ToTable("projectapproversdetails");
            modelBuilder.Entity<TrendApproversDetails>().ToTable("trendapproversdetails");
            //modelBuilder.Entity<BillOfMaterials>().ToTable("bill_of_materials"); // Added by Jignesh
            modelBuilder.Entity<EmployeeTimeCard>().ToTable("employee_time_card"); // Added by Jignesh 12-10-2020 For Co-Ad
            modelBuilder.Entity<User_LicenseMapping>().ToTable("UserLicenseMapping");//Added by Nivedita 03-11-2021
            modelBuilder.Entity<ContractModification>().ToTable("contract_modification"); // Added by Jignesh 28-10-2020
            //modelBuilder.Entity<Facility>().HasMany<Asset>(a => a.Assets)
            //                .WithMany(f => f.Facilities).Map(
            //                    m => m.MapLeftKey("AssetID").MapRightKey("ID").ToTable("facility_asset")
            // );
            //throw new Exception("Code first changes are not allowed.");
            modelBuilder.Entity<ModificationType>().ToTable("modification_type");
            modelBuilder.Entity<Versionmaster>().ToTable("versionmaster");
            modelBuilder.Entity<ProjectAccessControl>().ToTable("project_access_control"); // Jignesh-18-10-2021
            modelBuilder.Entity<clientPOC>().ToTable("client_poc");   //Tanmay - 15/12/2021
            modelBuilder.Entity<UserRoleRelation>().ToTable("user_role_relation");
            modelBuilder.Entity<ProgramNotes>().ToTable("program_notes");
        }

        public DbSet<Program> Program { get; set; }
        public DbSet<ProgramElement> ProgramElement { get; set; }
        public DbSet<Project> Project { get; set; }
        public DbSet<ProjectLocation> ProjectLocation { get; set; }
        public DbSet<GeoLocation> GeoLocation { get; set; }
        public DbSet<GeoCoordinate> GeoCoordinate { get; set; }
        public DbSet<Trend> Trend { get; set; }
        public DbSet<TrendStatus> TrendStatus { get; set; }

        //new table
        public DbSet<ProgramFund> ProgramFund { get; set; }
        public DbSet<TrendFund> TrendFund { get; set; }
        public DbSet<ProjectScope> ProjectScope { get; set; }

        //Asset MAnagement
        public DbSet<Asset> Asset { get; set; }
        public DbSet<FacilityAsset> FacilityAsset { get; set; }
        public DbSet<Facility> Facility { get; set; }
        public DbSet<AssetSupplier> AssetSupplier { get; set; }
        public DbSet<AssetHistory> AssetHistory { get; set; }
        public DbSet<AssetComponent> AssetComponent { get; set; }
        public DbSet<AssetComponentHistory> AssetComponentHistory { get; set; }
        //public DbSet<ProjectScope> ProjectScope { get; set; }
        public DbSet<RandomToken> RandomToken { get; set; }
        public DbSet<User> User { get; set; }
        

        public DbSet<ApprovalMatrix> ApprovalMatrix { get; set; }
        public DbSet<ProgramCategory> ProgramCategory { get; set; }
        public DbSet<Organization> Organization { get; set; }
        public DbSet<FundType> FundType { get; set; }
        public DbSet<PhaseCode> PhaseCode { get; set; }
        public DbSet<UnitType> UnitType { get; set; }
        public DbSet<FTEPosition> FtePosition { get; set; }
        public DbSet<ActivityCategory> ActivityCategory { get; set; }
        public DbSet<Activity> Activity { get; set; }
        public DbSet<CostFTE> CostFte { get; set; }
        public DbSet<CostLumpsum> CostLumpsum { get; set; }
        public DbSet<CostUnit> CostUnit { get; set; }
        public DbSet<CostPercentage> CostPercentage { get; set; }
        public DbSet<CostODC> CostODC { get; set; }
        public DbSet<Employee> Employee { get; set; }

        //Views
        public DbSet<TestView> TestView { get; set; }
        public DbSet<CurrentProjectTrend> CurrentProjectTrend { get; set; }
        public DbSet<CostTrackType> CostTrackType { get; set; }
        public DbSet<Material> Material { get; set; }
        public DbSet<MaterialCategory> MaterialCategory { get; set; }
        public DbSet<ProjectWhiteList> ProjectWhiteList { get; set; }
        public DbSet<ODCType> ODCType { get; set; }
        public DbSet<SubcontractorType> SubcontractorType { get; set; }
        public DbSet<Subcontractor> Subcontractor { get; set; }
        public DbSet<ProjectType> ProjectType { get; set; }
        public DbSet<ProjectClass> ProjectClass { get; set; }
        public DbSet<ServiceClass> ServiceClass { get; set; }
        public DbSet<Prime> Prime { get; set; }
        public DbSet<CertifiedPayroll> CertifiedPayroll { get; set; }    //Vaishnavi 12-04-2022
        public DbSet<ProgramCertifiedPayroll> ProgramCertifiedPayroll { get; set; }
        public DbSet<ProgramPrevailingWage> ProgramPrevailingWage { get; set; }
        public DbSet<Wrap> Wrap { get; set; }
        public DbSet<Holidays> Holidays { get; set; }
        public DbSet<ProgramWrap> ProgramWrap { get; set; }
        public DbSet<ProgramWarranty> ProgramWarranty { get; set; }     //Vaishnavi 12-04-2022
        public DbSet<Contract> Contract { get; set; }
        public DbSet<Milestone> Milestone { get; set; }
        public DbSet<ChangeOrder> ChangeOrder { get; set; }
        public DbSet<ProgramContract> ProgramContract { get; set; }
        //public DbSet<ProjectClassPhase> ProjectClassPhase { get; set; }
        public DbSet<ServiceToSubserviceMapping> ServiceToSubserviceMapping { get; set; }
        public DbSet<Client> Client { get; set; }
        public DbSet<clientPOC> ClientPOC { get; set; }  //Tanmay - 15/12/2021
        public DbSet<Location> Location { get; set; }
        public DbSet<DocumentType> DocumentType { get; set; }
        public DbSet<Document> Document { get; set; }
        public DbSet<DocumentData> DocumentData { get; set; }
        public DbSet<Vendor> Vendor { get; set; }
        public DbSet<BOMRequest> BOMRequest { get; set; }
        public DbSet<Manufacturer> Manufacturer { get; set; }
        public DbSet<Inventory> Inventory { get; set; }


        public DbSet<TemporaryCost> TemporaryCost { get; set; }
        public DbSet<CostLineItemTracker> CostLineItemTracker { get; set; }
        public DbSet<ProjectNumber> ProjectNumber { get; set; }

        public DbSet<TrendStatusCode> TrendStatusCode { get; set; }
        public DbSet<HistoricalActualCost> HistoricalActualCost { get; set; }
        public DbSet<ApiError> ApiError { get; set; }
        public DbSet<ApiLog> ApiLog { get; set; }
        public DbSet<CostType> CostType { get; set; }
        public DbSet<CostRateType> CostRateType { get; set; }
        public DbSet<CostOverhead> CostOverhead { get; set; }
        public DbSet<TrendCostOverhead> TrendCostOverhead { get; set; }
        public DbSet<CostTypeRateType> CostTypeRateType { get; set; }
        public DbSet<PurchaseOrder> PurchaseOrder { get; set; }
        public DbSet<PurchaseOrderDetail> PurchaseOrderDetail { get; set; }
        public DbSet<POODCEmployeeDetails> POODCEmployeeDetails { get; set; }
        public DbSet<LineOfBusiness> LineOfBusiness { get; set; }
        public DbSet<LobPhase> LobPhase { get; set; }
        public DbSet<CostOverheadType> CostOverheadType { get; set; }
        public DbSet<MFAConfiguration> MFAConfigurationDetails { get; set; }
        public DbSet<TrendApprovalTrackLog> TrendApprovalTrackLog { get; set; }
        public DbSet<EmployeeTimeCard> EmployeeTimeCard { get; set; }
        public DbSet<ProjectApproversDetails> ProjectApproversDetails { get; set; }
        public DbSet<TrendApproversDetails> TrendApproversDetails { get; set; }
        //public DbSet<BillOfMaterials> BillOfMaterials { get; set; } // Added by Jignesh
        public DbSet<ContractModification> ContractModification { get; set; } // Added by Jignesh 28-10-2020

        public DbSet<TemporaryCostMigration> TemporaryCostMigration { get; set; }   //Manasi 28-12-2020

        public DbSet<ModificationType> ModificationType { get; set; }
        public DbSet<User_LicenseMapping> User_LicenseMapping { get; set; } // Added by Nivedita 03-11-2021


        public DbSet<Versionmaster> VersionMaster { get; set; }
        public DbSet<ProjectAccessControl> ProjectAccessControl { get; set; } // Jignesh-18-10-2021

        public DbSet<NotificationDays> NotificationDays { get; set; }   // Narayan - 03-03-2022

        public DbSet<UserRoleRelation> UserRoleRelation { get; set; }
        public DbSet<ProgramNotes> ProgramNotes { get; set; }

        public DbSet<PrelimnaryNotice> PrelimnaryNotices { get; set; }   // Narayan - 04-04-2022

        public DbSet<ContractInsurance> ContractInsurances { get; set; }   // Narayan - 07-04-2022

        public DbSet<ContractProjectManager> ContractProjectManagers { get; set; } // Narayan - 03-05-2022

    }

}