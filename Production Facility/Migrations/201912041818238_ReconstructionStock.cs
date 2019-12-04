namespace Production_Facility.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ReconstructionStock : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.StockItems", "Name");
            DropColumn("dbo.StockItems", "Unit");
            DropColumn("dbo.StockItems", "Section");
        }
        
        public override void Down()
        {
            AddColumn("dbo.StockItems", "Section", c => c.Int(nullable: false));
            AddColumn("dbo.StockItems", "Unit", c => c.Int(nullable: false));
            AddColumn("dbo.StockItems", "Name", c => c.String());
        }
    }
}
