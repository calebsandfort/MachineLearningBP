namespace MachineLearningBP.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddMoreNbaStats : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.NbaStatLines", "FieldGoalsMade", c => c.Double(nullable: false));
            AddColumn("dbo.NbaStatLines", "FieldGoalsAttempted", c => c.Double(nullable: false));
            AddColumn("dbo.NbaStatLines", "ThreePointersMade", c => c.Double(nullable: false));
            AddColumn("dbo.NbaStatLines", "ThreePointersAttempted", c => c.Double(nullable: false));
            AddColumn("dbo.NbaStatLines", "FreeThrowsMade", c => c.Double(nullable: false));
            AddColumn("dbo.NbaStatLines", "FreeThrowsAttempted", c => c.Double(nullable: false));
            AddColumn("dbo.NbaStatLines", "Turnovers", c => c.Double(nullable: false));
            AddColumn("dbo.NbaStatLines", "OffensiveRebounds", c => c.Double(nullable: false));
            AddColumn("dbo.NbaStatLines", "DefensiveRebounds", c => c.Double(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.NbaStatLines", "DefensiveRebounds");
            DropColumn("dbo.NbaStatLines", "OffensiveRebounds");
            DropColumn("dbo.NbaStatLines", "Turnovers");
            DropColumn("dbo.NbaStatLines", "FreeThrowsAttempted");
            DropColumn("dbo.NbaStatLines", "FreeThrowsMade");
            DropColumn("dbo.NbaStatLines", "ThreePointersAttempted");
            DropColumn("dbo.NbaStatLines", "ThreePointersMade");
            DropColumn("dbo.NbaStatLines", "FieldGoalsAttempted");
            DropColumn("dbo.NbaStatLines", "FieldGoalsMade");
        }
    }
}
