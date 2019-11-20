namespace Production_Facility
{
    using Production_Facility.Models;
    using System;
    using System.Data.Entity;
    using System.Linq;

    public class FacilityDBContext : DbContext
    {


        public DbSet<Item> Items { get; set; }
        public DbSet<StockItem> StockItems { get; set; }
        public DbSet<Recipe> Recipes { get; set; }
        public DbSet<ProductionOrder> ProductionOrders { get; set; }



        //public FacilityDBContext()
        //    : base("name=FacilityDBContext")
        //{
        //    Database.SetInitializer(new MigrateDatabaseToLatestVersion<FacilityDBContext, Migrations.Configuration>());
        //}

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}