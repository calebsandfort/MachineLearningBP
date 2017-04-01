namespace MachineLearningBP.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddNbaSeasonAndGame : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.NbaGames",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        SeasonId = c.Int(nullable: false),
                        Date = c.DateTime(nullable: false),
                        EspnIdentifier = c.Int(nullable: false),
                        Completed = c.Boolean(nullable: false),
                        Spread = c.Double(nullable: false),
                        Total = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.NbaSeasons", t => t.SeasonId, cascadeDelete: true)
                .Index(t => t.SeasonId);
            
            CreateTable(
                "dbo.NbaSeasons",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Start = c.DateTime(nullable: false),
                        End = c.DateTime(nullable: false),
                        RollingWindowStart = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.NbaGames", "SeasonId", "dbo.NbaSeasons");
            DropIndex("dbo.NbaGames", new[] { "SeasonId" });
            DropTable("dbo.NbaSeasons");
            DropTable("dbo.NbaGames");
        }
    }
}
