namespace Production_Facility.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FKz : DbMigration
    {
        public override void Up()
        {
            //AlterColumn("dbo.StockItems", "Number", c => c.String(maxLength: 128));
            //CreateIndex("dbo.StockItems", "Number");
            AddForeignKey("dbo.StockItems", "Number", "dbo.Items", "Number");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.StockItems", "Number", "dbo.Items");
            //DropIndex("dbo.StockItems", new[] { "Number" });
            //AlterColumn("dbo.StockItems", "Number", c => c.String());
        }
    }
}
