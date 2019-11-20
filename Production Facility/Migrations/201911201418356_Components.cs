namespace Production_Facility.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Components : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Components",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Line = c.Byte(nullable: false),
                        OwnerKey = c.String(nullable: false, maxLength: 32),
                        OwnerName = c.String(nullable: false, maxLength: 64),
                        ComponentKey = c.String(nullable: false, maxLength: 32),
                        ComponentName = c.String(nullable: false, maxLength: 64),
                        Quantity = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Components");
        }
    }
}
