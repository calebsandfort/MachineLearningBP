namespace MachineLearningBP.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeIncidentExampleResultFromDoubleToInt : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.IncidentExamples", "Result", c => c.Double(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.IncidentExamples", "Result", c => c.Int(nullable: false));
        }
    }
}
