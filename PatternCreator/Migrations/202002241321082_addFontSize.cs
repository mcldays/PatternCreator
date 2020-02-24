namespace PatternCreator.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addFontSize : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PositionModels", "FontSize", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.PositionModels", "FontSize");
        }
    }
}
