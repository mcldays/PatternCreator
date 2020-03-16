namespace PatternCreator.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addFieldText : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PositionModels", "Text", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.PositionModels", "Text");
        }
    }
}
