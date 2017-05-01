namespace MachineLearningBP.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddMovieBrandAndSeries : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.MovieStatLines", "MicroSeries", c => c.Int(nullable: false));
            AddColumn("dbo.MovieStatLines", "MacroSeries", c => c.Int(nullable: false));
            AddColumn("dbo.MovieStatLines", "Brand", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.MovieStatLines", "Brand");
            DropColumn("dbo.MovieStatLines", "MacroSeries");
            DropColumn("dbo.MovieStatLines", "MicroSeries");
        }
    }
}
