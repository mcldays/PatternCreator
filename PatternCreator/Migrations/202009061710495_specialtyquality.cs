namespace PatternCreator.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class specialtyquality : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SpecialtyModels", "FieldSpecialty", c => c.String());
            AddColumn("dbo.SpecialtyModels", "Quality", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.SpecialtyModels", "Quality");
            DropColumn("dbo.SpecialtyModels", "FieldSpecialty");
        }
    }
}
