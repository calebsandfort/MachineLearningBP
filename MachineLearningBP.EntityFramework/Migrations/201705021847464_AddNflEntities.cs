namespace MachineLearningBP.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddNflEntities : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.NflGames",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TimeGroupingId = c.Int(nullable: false),
                        Date = c.DateTime(nullable: false),
                        EspnIdentifier = c.Int(nullable: false),
                        Completed = c.Boolean(nullable: false),
                        Total = c.Double(nullable: false),
                        Spread = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.NflSeasons", t => t.TimeGroupingId, cascadeDelete: true)
                .Index(t => t.TimeGroupingId);
            
            CreateTable(
                "dbo.NflStatLines",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        SampleId = c.Int(nullable: false),
                        ParticipantId = c.Int(nullable: false),
                        Points = c.Double(nullable: false),
                        KnnPoints = c.Double(nullable: false),
                        TreePoints = c.Double(nullable: false),
                        Home = c.Boolean(nullable: false),
                        TwoInTwoDays = c.Boolean(nullable: false),
                        ThreeInFourDays = c.Boolean(nullable: false),
                        FourInFiveDays = c.Boolean(nullable: false),
                        FourInSixDays = c.Boolean(nullable: false),
                        FiveInSevenDays = c.Boolean(nullable: false),
                        FieldGoalsMade = c.Double(nullable: false),
                        FieldGoalsAttempted = c.Double(nullable: false),
                        ThreePointersMade = c.Double(nullable: false),
                        ThreePointersAttempted = c.Double(nullable: false),
                        FreeThrowsMade = c.Double(nullable: false),
                        FreeThrowsAttempted = c.Double(nullable: false),
                        Turnovers = c.Double(nullable: false),
                        OffensiveRebounds = c.Double(nullable: false),
                        DefensiveRebounds = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.NflTeams", t => t.ParticipantId, cascadeDelete: true)
                .ForeignKey("dbo.NflGames", t => t.SampleId, cascadeDelete: true)
                .Index(t => t.SampleId)
                .Index(t => t.ParticipantId);
            
            CreateTable(
                "dbo.NflTeams",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        PfrId = c.String(),
                        SavId = c.String(),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.NflSeasons",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        RollingWindowStart = c.DateTime(),
                        Start = c.DateTime(nullable: false),
                        End = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.NflPointsExamples",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Date = c.DateTime(nullable: false),
                        StatLineId = c.Int(nullable: false),
                        Result = c.Double(nullable: false),
                        DelimitedNumericData = c.String(),
                        DelimitedOrdinalData = c.String(),
                        DelimitedNominalData = c.String(),
                        DelimitedAsymmBinaryData = c.String(),
                        DelimitedSymmBinaryData = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.NflStatLines", t => t.StatLineId, cascadeDelete: true)
                .Index(t => t.StatLineId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.NflPointsExamples", "StatLineId", "dbo.NflStatLines");
            DropForeignKey("dbo.NflGames", "TimeGroupingId", "dbo.NflSeasons");
            DropForeignKey("dbo.NflStatLines", "SampleId", "dbo.NflGames");
            DropForeignKey("dbo.NflStatLines", "ParticipantId", "dbo.NflTeams");
            DropIndex("dbo.NflPointsExamples", new[] { "StatLineId" });
            DropIndex("dbo.NflStatLines", new[] { "ParticipantId" });
            DropIndex("dbo.NflStatLines", new[] { "SampleId" });
            DropIndex("dbo.NflGames", new[] { "TimeGroupingId" });
            DropTable("dbo.NflPointsExamples");
            DropTable("dbo.NflSeasons");
            DropTable("dbo.NflTeams");
            DropTable("dbo.NflStatLines");
            DropTable("dbo.NflGames");
        }
    }
}
