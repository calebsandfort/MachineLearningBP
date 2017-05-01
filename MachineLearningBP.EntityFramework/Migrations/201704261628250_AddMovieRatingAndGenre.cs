namespace MachineLearningBP.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddMovieRatingAndGenre : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.MovieStatLines", "MpaaRating", c => c.Int(nullable: false));
            AddColumn("dbo.MovieStatLines", "Genre", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.MovieStatLines", "Genre");
            DropColumn("dbo.MovieStatLines", "MpaaRating");
        }
    }
}
