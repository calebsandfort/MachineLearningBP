namespace MachineLearningBP.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.MlbGames",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TimeGroupingId = c.Int(nullable: false),
                        Date = c.DateTime(nullable: false),
                        EspnIdentifier = c.Int(nullable: false),
                        Completed = c.Boolean(nullable: false),
                        Total = c.Double(nullable: false),
                        CoversId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.MlbSeasons", t => t.TimeGroupingId, cascadeDelete: true)
                .Index(t => t.TimeGroupingId);
            
            CreateTable(
                "dbo.MlbStatLines",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        SampleId = c.Int(nullable: false),
                        ParticipantId = c.Int(nullable: false),
                        Points = c.Double(nullable: false),
                        Home = c.Boolean(nullable: false),
                        Moneyline = c.Double(nullable: false),
                        InningsPitched = c.Double(nullable: false),
                        AtBats = c.Double(nullable: false),
                        Walks = c.Double(nullable: false),
                        Hits = c.Double(nullable: false),
                        HitByPitch = c.Double(nullable: false),
                        SacrificeFlies = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.MlbTeams", t => t.ParticipantId, cascadeDelete: true)
                .ForeignKey("dbo.MlbGames", t => t.SampleId, cascadeDelete: true)
                .Index(t => t.SampleId)
                .Index(t => t.ParticipantId);
            
            CreateTable(
                "dbo.MlbTeams",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.MlbSeasons",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        RollingWindowStart = c.DateTime(),
                        Start = c.DateTime(nullable: false),
                        End = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.NbaGames",
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
                .ForeignKey("dbo.NbaSeasons", t => t.TimeGroupingId, cascadeDelete: true)
                .Index(t => t.TimeGroupingId);
            
            CreateTable(
                "dbo.NbaStatLines",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        SampleId = c.Int(nullable: false),
                        ParticipantId = c.Int(nullable: false),
                        Points = c.Double(nullable: false),
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
                .ForeignKey("dbo.NbaTeams", t => t.ParticipantId, cascadeDelete: true)
                .ForeignKey("dbo.NbaGames", t => t.SampleId, cascadeDelete: true)
                .Index(t => t.SampleId)
                .Index(t => t.ParticipantId);
            
            CreateTable(
                "dbo.NbaTeams",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.NbaSeasons",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        RollingWindowStart = c.DateTime(),
                        Start = c.DateTime(nullable: false),
                        End = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.NbaPointsExamples",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Result = c.Double(nullable: false),
                        DelimitedPoints = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.NbaGames", "TimeGroupingId", "dbo.NbaSeasons");
            DropForeignKey("dbo.NbaStatLines", "SampleId", "dbo.NbaGames");
            DropForeignKey("dbo.NbaStatLines", "ParticipantId", "dbo.NbaTeams");
            DropForeignKey("dbo.MlbGames", "TimeGroupingId", "dbo.MlbSeasons");
            DropForeignKey("dbo.MlbStatLines", "SampleId", "dbo.MlbGames");
            DropForeignKey("dbo.MlbStatLines", "ParticipantId", "dbo.MlbTeams");
            DropIndex("dbo.NbaStatLines", new[] { "ParticipantId" });
            DropIndex("dbo.NbaStatLines", new[] { "SampleId" });
            DropIndex("dbo.NbaGames", new[] { "TimeGroupingId" });
            DropIndex("dbo.MlbStatLines", new[] { "ParticipantId" });
            DropIndex("dbo.MlbStatLines", new[] { "SampleId" });
            DropIndex("dbo.MlbGames", new[] { "TimeGroupingId" });
            DropTable("dbo.NbaPointsExamples");
            DropTable("dbo.NbaSeasons");
            DropTable("dbo.NbaTeams");
            DropTable("dbo.NbaStatLines");
            DropTable("dbo.NbaGames");
            DropTable("dbo.MlbSeasons");
            DropTable("dbo.MlbTeams");
            DropTable("dbo.MlbStatLines");
            DropTable("dbo.MlbGames");
        }
    }
}
