namespace MachineLearningBP.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddMoreExampleDataTypes : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.NbaAtsTreeExamples", "DelimitedNumericData", c => c.String());
            AddColumn("dbo.NbaAtsTreeExamples", "DelimitedAsymmBinaryData", c => c.String());
            AddColumn("dbo.NbaAtsTreeExamples", "DelimitedSymmBinaryData", c => c.String());
            AddColumn("dbo.NbaPointsExamples", "DelimitedNumericData", c => c.String());
            AddColumn("dbo.NbaPointsExamples", "DelimitedAsymmBinaryData", c => c.String());
            AddColumn("dbo.NbaPointsExamples", "DelimitedSymmBinaryData", c => c.String());
            DropColumn("dbo.NbaAtsTreeExamples", "DelimitedBinaryData");
            DropColumn("dbo.NbaPointsExamples", "DelimitedBinaryData");
        }
        
        public override void Down()
        {
            AddColumn("dbo.NbaPointsExamples", "DelimitedBinaryData", c => c.String());
            AddColumn("dbo.NbaAtsTreeExamples", "DelimitedBinaryData", c => c.String());
            DropColumn("dbo.NbaPointsExamples", "DelimitedSymmBinaryData");
            DropColumn("dbo.NbaPointsExamples", "DelimitedAsymmBinaryData");
            DropColumn("dbo.NbaPointsExamples", "DelimitedNumericData");
            DropColumn("dbo.NbaAtsTreeExamples", "DelimitedSymmBinaryData");
            DropColumn("dbo.NbaAtsTreeExamples", "DelimitedAsymmBinaryData");
            DropColumn("dbo.NbaAtsTreeExamples", "DelimitedNumericData");
        }
    }
}
