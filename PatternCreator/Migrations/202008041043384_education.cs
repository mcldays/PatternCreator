namespace PatternCreator.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class education : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.DocumentModels",
                c => new
                    {
                        DocumentId = c.Int(nullable: false, identity: true),
                        PatternId = c.Int(nullable: false),
                        UserId = c.Int(nullable: false),
                        Date = c.DateTime(nullable: false, storeType: "date"),
                        UserModel_Id = c.Int(),
                        PictureModel_Id = c.Int(),
                    })
                .PrimaryKey(t => t.DocumentId)
                .ForeignKey("dbo.UserModels", t => t.UserModel_Id)
                .ForeignKey("dbo.PictureModels", t => t.PictureModel_Id)
                .Index(t => t.UserModel_Id)
                .Index(t => t.PictureModel_Id);
            
            AddColumn("dbo.UserModels", "Position", c => c.String());
            AddColumn("dbo.UserModels", "Education", c => c.String());
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.DocumentModels", "PictureModel_Id", "dbo.PictureModels");
            DropForeignKey("dbo.DocumentModels", "UserModel_Id", "dbo.UserModels");
            DropIndex("dbo.DocumentModels", new[] { "PictureModel_Id" });
            DropIndex("dbo.DocumentModels", new[] { "UserModel_Id" });
            DropColumn("dbo.UserModels", "Education");
            DropColumn("dbo.UserModels", "Position");
            DropTable("dbo.DocumentModels");
        }
    }
}
