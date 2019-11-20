namespace Production_Facility.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class OrdersModify : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.OrderComponents",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        OrderId = c.Int(nullable: false),
                        Line = c.Byte(nullable: false),
                        OwnerKey = c.String(nullable: false, maxLength: 32),
                        OwnerName = c.String(nullable: false, maxLength: 128),
                        ComponentKey = c.String(nullable: false, maxLength: 32),
                        ComponentName = c.String(nullable: false, maxLength: 128),
                        Quantity = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Orders", t => t.OrderId, cascadeDelete: true)
                .Index(t => t.OrderId);
            
            CreateTable(
                "dbo.Orders",
                c => new
                    {
                        OrderID = c.Int(nullable: false, identity: true),
                        OwnerKey = c.String(),
                        Quantity = c.Int(nullable: false),
                        EntryDate = c.DateTime(),
                        PlannedDate = c.DateTime(nullable: false),
                        ClosingDate = c.DateTime(),
                        OrderStatus = c.String(),
                    })
                .PrimaryKey(t => t.OrderID);
            
            DropTable("dbo.ProductionOrders");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.ProductionOrders",
                c => new
                    {
                        OrderID = c.Int(nullable: false, identity: true),
                        ItemKey = c.String(),
                        Quantity = c.Int(nullable: false),
                        OrderDate = c.DateTime(),
                        PlannedDate = c.DateTime(nullable: false),
                        ProductionDate = c.DateTime(),
                        OrderStatus = c.String(),
                        OrderComposition = c.String(),
                    })
                .PrimaryKey(t => t.OrderID);
            
            DropForeignKey("dbo.OrderComponents", "OrderId", "dbo.Orders");
            DropIndex("dbo.OrderComponents", new[] { "OrderId" });
            DropTable("dbo.Orders");
            DropTable("dbo.OrderComponents");
        }
    }
}
