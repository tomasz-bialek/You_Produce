namespace Production_Facility.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Components3 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Components", "OwnerName", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("dbo.Components", "ComponentName", c => c.String(nullable: false, maxLength: 128));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Components", "ComponentName", c => c.String(nullable: false, maxLength: 64));
            AlterColumn("dbo.Components", "OwnerName", c => c.String(nullable: false, maxLength: 64));
        }
    }
}
