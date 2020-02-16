namespace PatternCreator.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class pohuy2 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PositionModels",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        PictureId = c.Int(nullable: false),
                        PosX = c.Double(nullable: false),
                        PosY = c.Double(nullable: false),
                        Width = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            DropColumn("dbo.PictureModels", "PosX");
            DropColumn("dbo.PictureModels", "PosY");
            DropColumn("dbo.PictureModels", "Width");
        }
        
        public override void Down()
        {
            AddColumn("dbo.PictureModels", "Width", c => c.Int(nullable: false));
            AddColumn("dbo.PictureModels", "PosY", c => c.Single(nullable: false));
            AddColumn("dbo.PictureModels", "PosX", c => c.Single(nullable: false));
            DropTable("dbo.PositionModels");
        }
    }
}
