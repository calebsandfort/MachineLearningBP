namespace MachineLearningBP.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddNbaStatLines : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.NbaStatLines",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TwoInTwoDays = c.Boolean(nullable: false),
                        ThreeInFourDays = c.Boolean(nullable: false),
                        FourInFiveDays = c.Boolean(nullable: false),
                        FourInSixDays = c.Boolean(nullable: false),
                        FiveInSevenDays = c.Boolean(nullable: false),
                        TeamId = c.Int(nullable: false),
                        GameId = c.Int(nullable: false),
                        Points = c.Double(nullable: false),
                        Home = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.NbaGames", t => t.GameId, cascadeDelete: true)
                .ForeignKey("dbo.NbaTeams", t => t.TeamId, cascadeDelete: true)
                .Index(t => t.TeamId)
                .Index(t => t.GameId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.NbaStatLines", "TeamId", "dbo.NbaTeams");
            DropForeignKey("dbo.NbaStatLines", "GameId", "dbo.NbaGames");
            DropIndex("dbo.NbaStatLines", new[] { "GameId" });
            DropIndex("dbo.NbaStatLines", new[] { "TeamId" });
            DropTable("dbo.NbaStatLines");
        }
    }
}
