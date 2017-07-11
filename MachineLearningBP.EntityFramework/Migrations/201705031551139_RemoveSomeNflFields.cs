namespace MachineLearningBP.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveSomeNflFields : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.NflStatLines", "YardsPerPlay", c => c.Double(nullable: false));
            DropColumn("dbo.NflStatLines", "TreePoints");
            DropColumn("dbo.NflStatLines", "TwoInTwoDays");
            DropColumn("dbo.NflStatLines", "ThreeInFourDays");
            DropColumn("dbo.NflStatLines", "FourInFiveDays");
            DropColumn("dbo.NflStatLines", "FourInSixDays");
            DropColumn("dbo.NflStatLines", "FiveInSevenDays");
            DropColumn("dbo.NflStatLines", "FieldGoalsMade");
            DropColumn("dbo.NflStatLines", "FieldGoalsAttempted");
            DropColumn("dbo.NflStatLines", "ThreePointersMade");
            DropColumn("dbo.NflStatLines", "ThreePointersAttempted");
            DropColumn("dbo.NflStatLines", "FreeThrowsMade");
            DropColumn("dbo.NflStatLines", "FreeThrowsAttempted");
            DropColumn("dbo.NflStatLines", "Turnovers");
            DropColumn("dbo.NflStatLines", "OffensiveRebounds");
            DropColumn("dbo.NflStatLines", "DefensiveRebounds");
        }
        
        public override void Down()
        {
            AddColumn("dbo.NflStatLines", "DefensiveRebounds", c => c.Double(nullable: false));
            AddColumn("dbo.NflStatLines", "OffensiveRebounds", c => c.Double(nullable: false));
            AddColumn("dbo.NflStatLines", "Turnovers", c => c.Double(nullable: false));
            AddColumn("dbo.NflStatLines", "FreeThrowsAttempted", c => c.Double(nullable: false));
            AddColumn("dbo.NflStatLines", "FreeThrowsMade", c => c.Double(nullable: false));
            AddColumn("dbo.NflStatLines", "ThreePointersAttempted", c => c.Double(nullable: false));
            AddColumn("dbo.NflStatLines", "ThreePointersMade", c => c.Double(nullable: false));
            AddColumn("dbo.NflStatLines", "FieldGoalsAttempted", c => c.Double(nullable: false));
            AddColumn("dbo.NflStatLines", "FieldGoalsMade", c => c.Double(nullable: false));
            AddColumn("dbo.NflStatLines", "FiveInSevenDays", c => c.Boolean(nullable: false));
            AddColumn("dbo.NflStatLines", "FourInSixDays", c => c.Boolean(nullable: false));
            AddColumn("dbo.NflStatLines", "FourInFiveDays", c => c.Boolean(nullable: false));
            AddColumn("dbo.NflStatLines", "ThreeInFourDays", c => c.Boolean(nullable: false));
            AddColumn("dbo.NflStatLines", "TwoInTwoDays", c => c.Boolean(nullable: false));
            AddColumn("dbo.NflStatLines", "TreePoints", c => c.Double(nullable: false));
            DropColumn("dbo.NflStatLines", "YardsPerPlay");
        }
    }
}
