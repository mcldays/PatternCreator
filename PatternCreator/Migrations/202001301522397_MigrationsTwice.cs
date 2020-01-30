namespace PatternCreator.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MigrationsTwice : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.UserModels", "CompanyId", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.UserModels", "CompanyId", c => c.String());
        }
    }
}
