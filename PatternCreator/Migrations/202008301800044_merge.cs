namespace PatternCreator.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class merge : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.DocumentModels", "SpecialtyModel_Id", "dbo.SpecialtyModels");
            DropIndex("dbo.DocumentModels", new[] { "SpecialtyModel_Id" });
            RenameColumn(table: "dbo.DocumentModels", name: "SpecialtyModel_Id", newName: "SpecialtyId");
            CreateTable(
                "dbo.Organizations",
                c => new
                    {
                        OrganizationId = c.Int(nullable: false, identity: true),
                        OrganizationName = c.String(),
                    })
                .PrimaryKey(t => t.OrganizationId);
            
            CreateTable(
                "dbo.ProtocolModels",
                c => new
                    {
                        ProtocolName = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.ProtocolName);
            
            AddColumn("dbo.DocumentModels", "StartDate", c => c.DateTime(nullable: false, storeType: "date"));
            AddColumn("dbo.DocumentModels", "OrganizationId", c => c.Int(nullable: false));
            AddColumn("dbo.DocumentModels", "ProtocolName", c => c.String(maxLength: 128));
            AddColumn("dbo.DocumentModels", "EducationTime", c => c.String());
            AddColumn("dbo.DocumentModels", "HandWriteFields", c => c.String());
            AlterColumn("dbo.DocumentModels", "SpecialtyId", c => c.Int(nullable: false));
            CreateIndex("dbo.DocumentModels", "OrganizationId");
            CreateIndex("dbo.DocumentModels", "ProtocolName");
            CreateIndex("dbo.DocumentModels", "SpecialtyId");
            AddForeignKey("dbo.DocumentModels", "OrganizationId", "dbo.Organizations", "OrganizationId", cascadeDelete: true);
            AddForeignKey("dbo.DocumentModels", "ProtocolName", "dbo.ProtocolModels", "ProtocolName");
            AddForeignKey("dbo.DocumentModels", "SpecialtyId", "dbo.SpecialtyModels", "Id", cascadeDelete: true);
            DropColumn("dbo.DocumentModels", "SpecialityName");
        }
        
        public override void Down()
        {
            AddColumn("dbo.DocumentModels", "SpecialityName", c => c.String());
            DropForeignKey("dbo.DocumentModels", "SpecialtyId", "dbo.SpecialtyModels");
            DropForeignKey("dbo.DocumentModels", "ProtocolName", "dbo.ProtocolModels");
            DropForeignKey("dbo.DocumentModels", "OrganizationId", "dbo.Organizations");
            DropIndex("dbo.DocumentModels", new[] { "SpecialtyId" });
            DropIndex("dbo.DocumentModels", new[] { "ProtocolName" });
            DropIndex("dbo.DocumentModels", new[] { "OrganizationId" });
            AlterColumn("dbo.DocumentModels", "SpecialtyId", c => c.Int());
            DropColumn("dbo.DocumentModels", "HandWriteFields");
            DropColumn("dbo.DocumentModels", "EducationTime");
            DropColumn("dbo.DocumentModels", "ProtocolName");
            DropColumn("dbo.DocumentModels", "OrganizationId");
            DropColumn("dbo.DocumentModels", "StartDate");
            DropTable("dbo.ProtocolModels");
            DropTable("dbo.Organizations");
            RenameColumn(table: "dbo.DocumentModels", name: "SpecialtyId", newName: "SpecialtyModel_Id");
            CreateIndex("dbo.DocumentModels", "SpecialtyModel_Id");
            AddForeignKey("dbo.DocumentModels", "SpecialtyModel_Id", "dbo.SpecialtyModels", "Id");
        }
    }
}
