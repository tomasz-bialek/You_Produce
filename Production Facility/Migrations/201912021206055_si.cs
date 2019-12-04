namespace Production_Facility.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class si : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.StockItems", new[] { "ItemNumber" });
            DropColumn("dbo.StockItems", "Number");
            RenameColumn(table: "dbo.StockItems", name: "ItemNumber", newName: "Number");
            AlterColumn("dbo.StockItems", "Number", c => c.String(maxLength: 128));
            CreateIndex("dbo.StockItems", "Number");
        }
        
        public override void Down()
        {
            DropIndex("dbo.StockItems", new[] { "Number" });
            AlterColumn("dbo.StockItems", "Number", c => c.String());
            RenameColumn(table: "dbo.StockItems", name: "Number", newName: "ItemNumber");
            AddColumn("dbo.StockItems", "Number", c => c.String());
            CreateIndex("dbo.StockItems", "ItemNumber");
        }
    }
}
