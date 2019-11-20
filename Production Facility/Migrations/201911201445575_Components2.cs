namespace Production_Facility.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Components2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Components", "RecipeId", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Components", "RecipeId");
        }
    }
}
