namespace PatternCreator.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class boldalign : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PositionModels", "FontWeight", c => c.String());
            AddColumn("dbo.PositionModels", "Alignment", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.PositionModels", "Alignment");
            DropColumn("dbo.PositionModels", "FontWeight");
        }
    }
}
