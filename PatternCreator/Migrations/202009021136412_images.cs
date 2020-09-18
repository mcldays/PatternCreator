namespace PatternCreator.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class images : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PictureModels", "PathToImg", c => c.String());
            DropColumn("dbo.PictureModels", "Image");
        }
        
        public override void Down()
        {
            AddColumn("dbo.PictureModels", "Image", c => c.Binary());
            DropColumn("dbo.PictureModels", "PathToImg");
        }
    }
}
