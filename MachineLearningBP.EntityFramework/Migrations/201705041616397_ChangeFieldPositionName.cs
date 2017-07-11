namespace MachineLearningBP.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeFieldPositionName : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.NflStatLines", "FieldPosition", c => c.Double(nullable: false));
            DropColumn("dbo.NflStatLines", "AverageStartingFieldPosition");
        }
        
        public override void Down()
        {
            AddColumn("dbo.NflStatLines", "AverageStartingFieldPosition", c => c.Double(nullable: false));
            DropColumn("dbo.NflStatLines", "FieldPosition");
        }
    }
}
