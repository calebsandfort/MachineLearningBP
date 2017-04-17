namespace MachineLearningBP.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddKnnPoints : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.NbaStatLines", "KnnPoints", c => c.Double(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.NbaStatLines", "KnnPoints");
        }
    }
}
