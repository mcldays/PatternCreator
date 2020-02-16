namespace PatternCreator.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class pohuy1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PictureModels", "PosX", c => c.Single(nullable: false));
            AddColumn("dbo.PictureModels", "PosY", c => c.Single(nullable: false));
            AddColumn("dbo.PictureModels", "Width", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.PictureModels", "Width");
            DropColumn("dbo.PictureModels", "PosY");
            DropColumn("dbo.PictureModels", "PosX");
        }
    }
}
