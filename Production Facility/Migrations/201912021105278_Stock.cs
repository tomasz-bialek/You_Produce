namespace Production_Facility.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Stock : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Stocks",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ItemNumber = c.String(maxLength: 128),
                        Quantity = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Items", t => t.ItemNumber)
                .Index(t => t.ItemNumber);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Stocks", "ItemNumber", "dbo.Items");
            DropIndex("dbo.Stocks", new[] { "ItemNumber" });
            DropTable("dbo.Stocks");
        }
    }
}
