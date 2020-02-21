namespace PatternCreator.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addKefs : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PictureModels", "KefWidth", c => c.Double(nullable: false));
            AddColumn("dbo.PictureModels", "KefHeight", c => c.Double(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.PictureModels", "KefHeight");
            DropColumn("dbo.PictureModels", "KefWidth");
        }
    }
}
