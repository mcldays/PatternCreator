namespace PatternCreator.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class validuntil : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SpecialtyModels", "ValidUntil", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.SpecialtyModels", "ValidUntil");
        }
    }
}
