namespace MachineLearningBP.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeIncidentCategoryFields : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.IncidentStatLines", "Category", c => c.String());
            DropColumn("dbo.IncidentStatLines", "Category1");
            DropColumn("dbo.IncidentStatLines", "Category2");
            DropColumn("dbo.IncidentStatLines", "Category3");
            DropColumn("dbo.IncidentStatLines", "Category4");
            DropColumn("dbo.IncidentStatLines", "Category5");
        }
        
        public override void Down()
        {
            AddColumn("dbo.IncidentStatLines", "Category5", c => c.String());
            AddColumn("dbo.IncidentStatLines", "Category4", c => c.String());
            AddColumn("dbo.IncidentStatLines", "Category3", c => c.String());
            AddColumn("dbo.IncidentStatLines", "Category2", c => c.String());
            AddColumn("dbo.IncidentStatLines", "Category1", c => c.String());
            DropColumn("dbo.IncidentStatLines", "Category");
        }
    }
}
