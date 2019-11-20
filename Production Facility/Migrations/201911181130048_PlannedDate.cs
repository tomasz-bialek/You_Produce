namespace Production_Facility.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PlannedDate : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.ProductionOrders", "PlannedDate", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.ProductionOrders", "PlannedDate", c => c.String());
        }
    }
}
