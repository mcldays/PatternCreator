namespace PatternCreator.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class migrateAuth : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AutModels",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        Login = c.String(nullable: false),
                        Password = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.CompanyModels",
                c => new
                    {
                        CompanyId = c.Int(nullable: false, identity: true),
                        CompanyName = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.CompanyId);
            
            CreateTable(
                "dbo.UserModels",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Surname = c.String(nullable: false),
                        Patronymic = c.String(),
                        CompanyId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.CompanyModels", t => t.CompanyId, cascadeDelete: true)
                .Index(t => t.CompanyId);
            
            CreateTable(
                "dbo.PictureModels",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Image = c.Binary(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.PositionModels",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        PictureId = c.Int(nullable: false),
                        PosX = c.Double(nullable: false),
                        PosY = c.Double(nullable: false),
                        Width = c.Double(nullable: false),
                        Type = c.String(),
                        FontSize = c.Int(nullable: false),
                        Height = c.Double(nullable: false),
                        Text = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserModels", "CompanyId", "dbo.CompanyModels");
            DropIndex("dbo.UserModels", new[] { "CompanyId" });
            DropTable("dbo.PositionModels");
            DropTable("dbo.PictureModels");
            DropTable("dbo.UserModels");
            DropTable("dbo.CompanyModels");
            DropTable("dbo.AutModels");
        }
    }
}
