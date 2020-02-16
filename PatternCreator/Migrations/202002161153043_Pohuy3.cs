namespace PatternCreator.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Pohuy3 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PositionModels", "Type", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.PositionModels", "Type");
        }
    }
}
