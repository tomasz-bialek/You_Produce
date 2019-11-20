namespace Production_Facility.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Init : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Items",
                c => new
                    {
                        Number = c.String(nullable: false, maxLength: 128),
                        Name = c.String(),
                        Unit = c.Int(nullable: false),
                        Section = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Number);
            
            CreateTable(
                "dbo.ProductionOrders",
                c => new
                    {
                        OrderID = c.Int(nullable: false, identity: true),
                        ItemKey = c.String(),
                        Quantity = c.Int(nullable: false),
                        OrderDate = c.DateTime(),
                        PlannedDate = c.String(),
                        ProductionDate = c.DateTime(),
                        OrderStatus = c.String(),
                        OrderComposition = c.String(),
                    })
                .PrimaryKey(t => t.OrderID);
            
            CreateTable(
                "dbo.Recipe",
                c => new
                    {
                        RecipeID = c.Int(nullable: false, identity: true),
                        RecipeOwner = c.String(),
                        RecipeComposition = c.String(),
                    })
                .PrimaryKey(t => t.RecipeID);
            
            CreateTable(
                "dbo.StockItems",
                c => new
                    {
                        StockItem_ID = c.Int(nullable: false, identity: true),
                        NumberRef = c.String(),
                        Number = c.String(),
                        Name = c.String(),
                        Unit = c.Int(nullable: false),
                        Section = c.Int(nullable: false),
                        QTotal = c.Double(nullable: false),
                        QReserved = c.Double(nullable: false),
                        QAvailable = c.Double(nullable: false),
                        UnitCost = c.Decimal(nullable: false, precision: 18, scale: 2),
                        TotalCost = c.Decimal(nullable: false, precision: 18, scale: 2),
                        IncomingDate = c.DateTime(nullable: false),
                        ExpirationDate = c.DateTime(),
                        LastActionDate = c.DateTime(nullable: false),
                        Location = c.String(),
                        BatchNumber = c.String(),
                    })
                .PrimaryKey(t => t.StockItem_ID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.StockItems");
            DropTable("dbo.Recipe");
            DropTable("dbo.ProductionOrders");
            DropTable("dbo.Items");
        }
    }
}
