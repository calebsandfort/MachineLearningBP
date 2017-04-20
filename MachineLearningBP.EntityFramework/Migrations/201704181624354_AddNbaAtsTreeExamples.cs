namespace MachineLearningBP.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddNbaAtsTreeExamples : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.NbaAtsTreeExamples",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Date = c.DateTime(nullable: false),
                        StatLineId = c.Int(nullable: false),
                        Result = c.String(),
                        DelimitedNumericalData = c.String(),
                        DelimitedCategoricalData = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.NbaStatLines", t => t.StatLineId, cascadeDelete: true)
                .Index(t => t.StatLineId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.NbaAtsTreeExamples", "StatLineId", "dbo.NbaStatLines");
            DropIndex("dbo.NbaAtsTreeExamples", new[] { "StatLineId" });
            DropTable("dbo.NbaAtsTreeExamples");
        }
    }
}
