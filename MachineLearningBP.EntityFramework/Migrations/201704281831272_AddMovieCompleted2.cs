namespace MachineLearningBP.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddMovieCompleted2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Movies", "Completed", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Movies", "Completed");
        }
    }
}
