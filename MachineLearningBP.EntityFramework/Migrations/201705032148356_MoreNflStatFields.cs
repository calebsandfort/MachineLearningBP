namespace MachineLearningBP.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MoreNflStatFields : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.NflStatLines", "TotalPlays", c => c.Double(nullable: false));
            AddColumn("dbo.NflStatLines", "TotalYards", c => c.Double(nullable: false));
            AddColumn("dbo.NflStatLines", "TotalDrives", c => c.Double(nullable: false));
            AddColumn("dbo.NflStatLines", "AverageStartingFieldPosition", c => c.Double(nullable: false));
            AddColumn("dbo.NflStatLines", "DrivesInside40", c => c.Double(nullable: false));
            AddColumn("dbo.NflStatLines", "PointsDrivesInside40", c => c.Double(nullable: false));
            AddColumn("dbo.NflStatLines", "PointsPerDriveInside40", c => c.Double(nullable: false));
            AddColumn("dbo.NflStatLines", "Turnovers", c => c.Double(nullable: false));
            AddColumn("dbo.NflStatLines", "TurnoversForced", c => c.Double(nullable: false));
            AddColumn("dbo.NflStatLines", "TurnoverMargin", c => c.Double(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.NflStatLines", "TurnoverMargin");
            DropColumn("dbo.NflStatLines", "TurnoversForced");
            DropColumn("dbo.NflStatLines", "Turnovers");
            DropColumn("dbo.NflStatLines", "PointsPerDriveInside40");
            DropColumn("dbo.NflStatLines", "PointsDrivesInside40");
            DropColumn("dbo.NflStatLines", "DrivesInside40");
            DropColumn("dbo.NflStatLines", "AverageStartingFieldPosition");
            DropColumn("dbo.NflStatLines", "TotalDrives");
            DropColumn("dbo.NflStatLines", "TotalYards");
            DropColumn("dbo.NflStatLines", "TotalPlays");
        }
    }
}
