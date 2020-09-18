namespace PatternCreator.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _float : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.PositionModels", "PosX", c => c.Single(nullable: false));
            AlterColumn("dbo.PositionModels", "PosY", c => c.Single(nullable: false));
            AlterColumn("dbo.PositionModels", "Width", c => c.Single(nullable: false));
            AlterColumn("dbo.PositionModels", "Height", c => c.Single(nullable: false));
            AlterColumn("dbo.StampPositions", "PosX", c => c.Single(nullable: false));
            AlterColumn("dbo.StampPositions", "PosY", c => c.Single(nullable: false));
            AlterColumn("dbo.StampPositions", "Width", c => c.Single(nullable: false));
            AlterColumn("dbo.StampPositions", "Height", c => c.Single(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.StampPositions", "Height", c => c.Double(nullable: false));
            AlterColumn("dbo.StampPositions", "Width", c => c.Double(nullable: false));
            AlterColumn("dbo.StampPositions", "PosY", c => c.Double(nullable: false));
            AlterColumn("dbo.StampPositions", "PosX", c => c.Double(nullable: false));
            AlterColumn("dbo.PositionModels", "Height", c => c.Double(nullable: false));
            AlterColumn("dbo.PositionModels", "Width", c => c.Double(nullable: false));
            AlterColumn("dbo.PositionModels", "PosY", c => c.Double(nullable: false));
            AlterColumn("dbo.PositionModels", "PosX", c => c.Double(nullable: false));
        }
    }
}
