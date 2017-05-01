namespace MachineLearningBP.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddMovieMojoId : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Movies", "MojoId", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Movies", "MojoId");
        }
    }
}
