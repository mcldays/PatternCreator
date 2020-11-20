namespace PatternCreator.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class photosunderline : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PhotoModels",
                c => new
                    {
                        PhotoModelId = c.Int(nullable: false, identity: true),
                        PosX = c.Single(nullable: false),
                        PosY = c.Single(nullable: false),
                        Width = c.Single(nullable: false),
                        Height = c.Single(nullable: false),
                        PicId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.PhotoModelId)
                .ForeignKey("dbo.PictureModels", t => t.PicId, cascadeDelete: true)
                .Index(t => t.PicId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PhotoModels", "PicId", "dbo.PictureModels");
            DropIndex("dbo.PhotoModels", new[] { "PicId" });
            DropTable("dbo.PhotoModels");
        }
    }
}
