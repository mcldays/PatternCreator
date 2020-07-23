namespace PatternCreator.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class stamps : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.StampPositions",
                c => new
                    {
                        StampPositionId = c.Int(nullable: false, identity: true),
                        PosX = c.Double(nullable: false),
                        PosY = c.Double(nullable: false),
                        Width = c.Double(nullable: false),
                        Height = c.Double(nullable: false),
                        StampId = c.Int(nullable: false),
                        PicId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.StampPositionId)
                .ForeignKey("dbo.PictureModels", t => t.PicId, cascadeDelete: true)
                .ForeignKey("dbo.StampModels", t => t.StampId, cascadeDelete: true)
                .Index(t => t.StampId)
                .Index(t => t.PicId);
            
            CreateIndex("dbo.PositionModels", "PictureId");
            AddForeignKey("dbo.PositionModels", "PictureId", "dbo.PictureModels", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.StampPositions", "StampId", "dbo.StampModels");
            DropForeignKey("dbo.StampPositions", "PicId", "dbo.PictureModels");
            DropForeignKey("dbo.PositionModels", "PictureId", "dbo.PictureModels");
            DropIndex("dbo.StampPositions", new[] { "PicId" });
            DropIndex("dbo.StampPositions", new[] { "StampId" });
            DropIndex("dbo.PositionModels", new[] { "PictureId" });
            DropTable("dbo.StampPositions");
        }
    }
}
