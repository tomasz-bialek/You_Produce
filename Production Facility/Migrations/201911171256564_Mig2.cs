namespace Production_Facility.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Mig2 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.StockItems", "NumberRef");
        }
        
        public override void Down()
        {
            AddColumn("dbo.StockItems", "NumberRef", c => c.String());
        }
    }
}
