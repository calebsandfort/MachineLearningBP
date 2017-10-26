namespace MachineLearningBP.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddIncidentExamples : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.IncidentExamples",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        StatLineId = c.Int(nullable: false),
                        Result = c.Int(nullable: false),
                        DelimitedNumericData = c.String(),
                        DelimitedOrdinalData = c.String(),
                        DelimitedNominalData = c.String(),
                        DelimitedAsymmBinaryData = c.String(),
                        DelimitedSymmBinaryData = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.IncidentStatLines", t => t.StatLineId, cascadeDelete: true)
                .Index(t => t.StatLineId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.IncidentExamples", "StatLineId", "dbo.IncidentStatLines");
            DropIndex("dbo.IncidentExamples", new[] { "StatLineId" });
            DropTable("dbo.IncidentExamples");
        }
    }
}
