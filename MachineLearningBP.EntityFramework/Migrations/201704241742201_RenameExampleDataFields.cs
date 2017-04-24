namespace MachineLearningBP.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RenameExampleDataFields : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.NbaAtsTreeExamples", "DelimitedOrdinalData", c => c.String());
            AddColumn("dbo.NbaAtsTreeExamples", "DelimitedNominalData", c => c.String());
            AddColumn("dbo.NbaAtsTreeExamples", "DelimitedBinaryData", c => c.String());
            AddColumn("dbo.NbaPointsExamples", "DelimitedOrdinalData", c => c.String());
            AddColumn("dbo.NbaPointsExamples", "DelimitedNominalData", c => c.String());
            AddColumn("dbo.NbaPointsExamples", "DelimitedBinaryData", c => c.String());
            DropColumn("dbo.NbaAtsTreeExamples", "DelimitedNumericalData");
            DropColumn("dbo.NbaAtsTreeExamples", "DelimitedCategoricalData");
            DropColumn("dbo.NbaPointsExamples", "DelimitedNumericalData");
            DropColumn("dbo.NbaPointsExamples", "DelimitedCategoricalData");
        }
        
        public override void Down()
        {
            AddColumn("dbo.NbaPointsExamples", "DelimitedCategoricalData", c => c.String());
            AddColumn("dbo.NbaPointsExamples", "DelimitedNumericalData", c => c.String());
            AddColumn("dbo.NbaAtsTreeExamples", "DelimitedCategoricalData", c => c.String());
            AddColumn("dbo.NbaAtsTreeExamples", "DelimitedNumericalData", c => c.String());
            DropColumn("dbo.NbaPointsExamples", "DelimitedBinaryData");
            DropColumn("dbo.NbaPointsExamples", "DelimitedNominalData");
            DropColumn("dbo.NbaPointsExamples", "DelimitedOrdinalData");
            DropColumn("dbo.NbaAtsTreeExamples", "DelimitedBinaryData");
            DropColumn("dbo.NbaAtsTreeExamples", "DelimitedNominalData");
            DropColumn("dbo.NbaAtsTreeExamples", "DelimitedOrdinalData");
        }
    }
}
