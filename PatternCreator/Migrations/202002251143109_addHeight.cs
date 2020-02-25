namespace PatternCreator.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addHeight : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PositionModels", "Height", c => c.Double(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.PositionModels", "Height");
        }
    }
}
