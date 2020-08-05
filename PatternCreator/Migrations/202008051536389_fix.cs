namespace PatternCreator.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class fix : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.DocumentModels", "UserModel_Id", "dbo.UserModels");
            DropForeignKey("dbo.DocumentModels", "PictureModel_Id", "dbo.PictureModels");
            DropIndex("dbo.DocumentModels", new[] { "UserModel_Id" });
            DropIndex("dbo.DocumentModels", new[] { "PictureModel_Id" });
            DropColumn("dbo.DocumentModels", "UserId");
            DropColumn("dbo.DocumentModels", "PatternId");
            RenameColumn(table: "dbo.DocumentModels", name: "UserModel_Id", newName: "UserId");
            RenameColumn(table: "dbo.DocumentModels", name: "PictureModel_Id", newName: "PatternId");
            AlterColumn("dbo.DocumentModels", "UserId", c => c.Int(nullable: false));
            AlterColumn("dbo.DocumentModels", "PatternId", c => c.Int(nullable: false));
            CreateIndex("dbo.DocumentModels", "PatternId");
            CreateIndex("dbo.DocumentModels", "UserId");
            AddForeignKey("dbo.DocumentModels", "UserId", "dbo.UserModels", "Id", cascadeDelete: true);
            AddForeignKey("dbo.DocumentModels", "PatternId", "dbo.PictureModels", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.DocumentModels", "PatternId", "dbo.PictureModels");
            DropForeignKey("dbo.DocumentModels", "UserId", "dbo.UserModels");
            DropIndex("dbo.DocumentModels", new[] { "UserId" });
            DropIndex("dbo.DocumentModels", new[] { "PatternId" });
            AlterColumn("dbo.DocumentModels", "PatternId", c => c.Int());
            AlterColumn("dbo.DocumentModels", "UserId", c => c.Int());
            RenameColumn(table: "dbo.DocumentModels", name: "PatternId", newName: "PictureModel_Id");
            RenameColumn(table: "dbo.DocumentModels", name: "UserId", newName: "UserModel_Id");
            AddColumn("dbo.DocumentModels", "PatternId", c => c.Int(nullable: false));
            AddColumn("dbo.DocumentModels", "UserId", c => c.Int(nullable: false));
            CreateIndex("dbo.DocumentModels", "PictureModel_Id");
            CreateIndex("dbo.DocumentModels", "UserModel_Id");
            AddForeignKey("dbo.DocumentModels", "PictureModel_Id", "dbo.PictureModels", "Id");
            AddForeignKey("dbo.DocumentModels", "UserModel_Id", "dbo.UserModels", "Id");
        }
    }
}
