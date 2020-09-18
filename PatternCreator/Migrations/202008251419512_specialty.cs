namespace PatternCreator.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class specialty : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.SpecialtyModels",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        SpecialityName = c.String(nullable: false),
                        Prefix = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.SpecialtyModelPictureModels",
                c => new
                    {
                        SpecialtyModel_Id = c.Int(nullable: false),
                        PictureModel_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.SpecialtyModel_Id, t.PictureModel_Id })
                .ForeignKey("dbo.SpecialtyModels", t => t.SpecialtyModel_Id, cascadeDelete: true)
                .ForeignKey("dbo.PictureModels", t => t.PictureModel_Id, cascadeDelete: true)
                .Index(t => t.SpecialtyModel_Id)
                .Index(t => t.PictureModel_Id);
            
            AddColumn("dbo.DocumentModels", "SpecialityName", c => c.String());
            AddColumn("dbo.DocumentModels", "SpecialtyModel_Id", c => c.Int());
            CreateIndex("dbo.DocumentModels", "SpecialtyModel_Id");
            AddForeignKey("dbo.DocumentModels", "SpecialtyModel_Id", "dbo.SpecialtyModels", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.DocumentModels", "SpecialtyModel_Id", "dbo.SpecialtyModels");
            DropForeignKey("dbo.SpecialtyModelPictureModels", "PictureModel_Id", "dbo.PictureModels");
            DropForeignKey("dbo.SpecialtyModelPictureModels", "SpecialtyModel_Id", "dbo.SpecialtyModels");
            DropIndex("dbo.SpecialtyModelPictureModels", new[] { "PictureModel_Id" });
            DropIndex("dbo.SpecialtyModelPictureModels", new[] { "SpecialtyModel_Id" });
            DropIndex("dbo.DocumentModels", new[] { "SpecialtyModel_Id" });
            DropColumn("dbo.DocumentModels", "SpecialtyModel_Id");
            DropColumn("dbo.DocumentModels", "SpecialityName");
            DropTable("dbo.SpecialtyModelPictureModels");
            DropTable("dbo.SpecialtyModels");
        }
    }
}
