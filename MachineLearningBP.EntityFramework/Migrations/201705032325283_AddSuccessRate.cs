namespace MachineLearningBP.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddSuccessRate : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.NflStatLines", "TotalSuccessRatePlays", c => c.Double(nullable: false));
            AddColumn("dbo.NflStatLines", "TotalSuccessfulSuccessRatePlays", c => c.Double(nullable: false));
            AddColumn("dbo.NflStatLines", "SuccessRate", c => c.Double(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.NflStatLines", "SuccessRate");
            DropColumn("dbo.NflStatLines", "TotalSuccessfulSuccessRatePlays");
            DropColumn("dbo.NflStatLines", "TotalSuccessRatePlays");
        }
    }
}
