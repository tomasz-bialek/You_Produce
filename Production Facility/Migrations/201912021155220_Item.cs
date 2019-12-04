namespace Production_Facility.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Item : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.StockItems", "ItemNumber", c => c.String(maxLength: 128));
            CreateIndex("dbo.StockItems", "ItemNumber");
            AddForeignKey("dbo.StockItems", "ItemNumber", "dbo.Items", "Number");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.StockItems", "ItemNumber", "dbo.Items");
            DropIndex("dbo.StockItems", new[] { "ItemNumber" });
            DropColumn("dbo.StockItems", "ItemNumber");
        }
    }
}
