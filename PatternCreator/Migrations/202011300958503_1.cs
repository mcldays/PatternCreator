namespace PatternCreator.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Organizations", "СhairmanName", c => c.String());
            AddColumn("dbo.Organizations", "TeacherName", c => c.String());
            AddColumn("dbo.Organizations", "SecretaryName", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Organizations", "SecretaryName");
            DropColumn("dbo.Organizations", "TeacherName");
            DropColumn("dbo.Organizations", "СhairmanName");
        }
    }
}
