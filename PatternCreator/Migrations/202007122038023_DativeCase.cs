namespace PatternCreator.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DativeCase : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.UserModels", "Name_DativeCase", c => c.String(nullable: false));
            AddColumn("dbo.UserModels", "Surname_DativeCase", c => c.String(nullable: false));
            AddColumn("dbo.UserModels", "Patronymic_DativeCase", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.UserModels", "Patronymic_DativeCase");
            DropColumn("dbo.UserModels", "Surname_DativeCase");
            DropColumn("dbo.UserModels", "Name_DativeCase");
        }
    }
}
