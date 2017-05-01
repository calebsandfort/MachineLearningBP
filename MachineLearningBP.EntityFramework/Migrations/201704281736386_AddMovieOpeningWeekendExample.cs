namespace MachineLearningBP.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddMovieOpeningWeekendExample : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.MovieOpeningWeekendExamples",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        StatLineId = c.Int(nullable: false),
                        Result = c.Double(nullable: false),
                        DelimitedNumericData = c.String(),
                        DelimitedOrdinalData = c.String(),
                        DelimitedNominalData = c.String(),
                        DelimitedAsymmBinaryData = c.String(),
                        DelimitedSymmBinaryData = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.MovieStatLines", t => t.StatLineId, cascadeDelete: true)
                .Index(t => t.StatLineId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.MovieOpeningWeekendExamples", "StatLineId", "dbo.MovieStatLines");
            DropIndex("dbo.MovieOpeningWeekendExamples", new[] { "StatLineId" });
            DropTable("dbo.MovieOpeningWeekendExamples");
        }
    }
}
