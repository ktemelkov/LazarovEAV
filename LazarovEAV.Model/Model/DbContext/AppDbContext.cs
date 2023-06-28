using LazarovEAV.Model.Config;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Data.SQLite;



namespace LazarovEAV.Model
{
    /// <summary>
    /// 
    /// </summary>
    class AppDbContext : DbContext
    {
        /// <summary>
        /// 
        /// </summary>
        public AppDbContext()
            : base(new SQLiteConnection() {
                            ConnectionString = new SQLiteConnectionStringBuilder() { DataSource = ModelConfig.DATABASE_PATH, ForeignKeys = true }.ConnectionString
                        }, true)
        {
            Database.SetInitializer<AppDbContext>(null);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            modelBuilder.Entity<MeridianInfo>().HasKey(o => o.Id);
            modelBuilder.Entity<MeridianPoint>().HasKey(o => o.Id);
            modelBuilder.Entity<SubstanceInfo>().HasKey(o => o.Id);
            modelBuilder.Entity<TestTableInfo>().HasKey(o => o.Id);
            modelBuilder.Entity<SlotInfo>().HasKey(o => o.Id);
            modelBuilder.Entity<PatientInfo>().HasKey(o => o.Id);
            modelBuilder.Entity<PatientSession>().HasKey(o => o.Id);
            modelBuilder.Entity<TestResultLeft>().HasKey(o => o.Id);
            modelBuilder.Entity<TestResultRight>().HasKey(o => o.Id);

            modelBuilder.Entity<SlotInfo>().HasKey(o => o.Id);
            modelBuilder.Entity<TestTableInfo>().HasKey(o => o.Id);
            modelBuilder.Entity<SubstanceFolder>().HasKey(o => o.Id);
            modelBuilder.Entity<SubstanceQuantity>();            
        }
    }
}
