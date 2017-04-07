namespace MachineLearningBP.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MoreGenericExamples : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.NbaPointsExamples", "DelimitedNumericalData", c => c.String());
            AddColumn("dbo.NbaPointsExamples", "DelimitedCategoricalData", c => c.String());
            DropColumn("dbo.NbaPointsExamples", "DelimitedPoints");
        }
        
        public override void Down()
        {
            AddColumn("dbo.NbaPointsExamples", "DelimitedPoints", c => c.String());
            DropColumn("dbo.NbaPointsExamples", "DelimitedCategoricalData");
            DropColumn("dbo.NbaPointsExamples", "DelimitedNumericalData");
        }
    }
}
