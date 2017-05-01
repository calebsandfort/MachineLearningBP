namespace MachineLearningBP.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddMovieCompleted : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.MovieOpeningWeekendExamples", "WideRelease", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.MovieOpeningWeekendExamples", "WideRelease");
        }
    }
}
