namespace MachineLearningBP.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddPointsExampleDate : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.NbaPointsExamples", "Date", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.NbaPointsExamples", "Date");
        }
    }
}
