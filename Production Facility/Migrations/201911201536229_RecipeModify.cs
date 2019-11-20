namespace Production_Facility.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RecipeModify : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.Components", newName: "RecipeComponents");
            CreateIndex("dbo.RecipeComponents", "RecipeId");
            AddForeignKey("dbo.RecipeComponents", "RecipeId", "dbo.Recipe", "RecipeID", cascadeDelete: true);
            DropColumn("dbo.Recipe", "RecipeComposition");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Recipe", "RecipeComposition", c => c.String());
            DropForeignKey("dbo.RecipeComponents", "RecipeId", "dbo.Recipe");
            DropIndex("dbo.RecipeComponents", new[] { "RecipeId" });
            RenameTable(name: "dbo.RecipeComponents", newName: "Components");
        }
    }
}
