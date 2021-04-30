namespace PatternCreator.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class stamps : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.StampPositions", "StampId", "dbo.StampModels");
            DropIndex("dbo.StampPositions", new[] { "StampId" });
            RenameColumn(table: "dbo.StampPositions", name: "StampId", newName: "StampModel_Id");
            AddColumn("dbo.StampPositions", "Type", c => c.Int(nullable: false));
            AlterColumn("dbo.StampPositions", "StampModel_Id", c => c.Int());
            CreateIndex("dbo.StampPositions", "StampModel_Id");
            AddForeignKey("dbo.StampPositions", "StampModel_Id", "dbo.StampModels", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.StampPositions", "StampModel_Id", "dbo.StampModels");
            DropIndex("dbo.StampPositions", new[] { "StampModel_Id" });
            AlterColumn("dbo.StampPositions", "StampModel_Id", c => c.Int(nullable: false));
            DropColumn("dbo.StampPositions", "Type");
            RenameColumn(table: "dbo.StampPositions", name: "StampModel_Id", newName: "StampId");
            CreateIndex("dbo.StampPositions", "StampId");
            AddForeignKey("dbo.StampPositions", "StampId", "dbo.StampModels", "Id", cascadeDelete: true);
        }
    }
}
