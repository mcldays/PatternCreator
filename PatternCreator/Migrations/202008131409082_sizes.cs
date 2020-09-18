namespace PatternCreator.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class sizes : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PictureModels", "NaturalWidth", c => c.Int(nullable: false));
            AddColumn("dbo.PictureModels", "NaturalHeight", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.PictureModels", "NaturalHeight");
            DropColumn("dbo.PictureModels", "NaturalWidth");
        }
    }
}
