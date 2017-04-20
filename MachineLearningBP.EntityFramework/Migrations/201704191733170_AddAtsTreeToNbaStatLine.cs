namespace MachineLearningBP.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddAtsTreeToNbaStatLine : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.NbaStatLines", "AtsTree", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.NbaStatLines", "AtsTree");
        }
    }
}
