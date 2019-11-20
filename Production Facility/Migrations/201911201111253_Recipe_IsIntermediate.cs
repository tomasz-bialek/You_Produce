namespace Production_Facility.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Recipe_IsIntermediate : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Recipe", "IsIntermediate", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Recipe", "IsIntermediate");
        }
    }
}
