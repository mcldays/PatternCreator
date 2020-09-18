namespace PatternCreator.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class license : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Organizations", "License", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Organizations", "License");
        }
    }
}
