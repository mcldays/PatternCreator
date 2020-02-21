namespace PatternCreator.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class removeKefs : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.PictureModels", "KefWidth");
            DropColumn("dbo.PictureModels", "KefHeight");
        }
        
        public override void Down()
        {
            AddColumn("dbo.PictureModels", "KefHeight", c => c.Double(nullable: false));
            AddColumn("dbo.PictureModels", "KefWidth", c => c.Double(nullable: false));
        }
    }
}
