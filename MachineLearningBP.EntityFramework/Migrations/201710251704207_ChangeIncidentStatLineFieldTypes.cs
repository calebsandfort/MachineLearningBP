namespace MachineLearningBP.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeIncidentStatLineFieldTypes : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.IncidentStatLines", "Customer", c => c.String());
            AlterColumn("dbo.IncidentStatLines", "Company", c => c.String());
            AlterColumn("dbo.IncidentStatLines", "Source", c => c.String());
            AlterColumn("dbo.IncidentStatLines", "Template", c => c.String());
            AlterColumn("dbo.IncidentStatLines", "Assignee", c => c.String());
            AlterColumn("dbo.IncidentStatLines", "Status", c => c.String());
            AlterColumn("dbo.IncidentStatLines", "Category1", c => c.String());
            AlterColumn("dbo.IncidentStatLines", "Category2", c => c.String());
            AlterColumn("dbo.IncidentStatLines", "Category3", c => c.String());
            AlterColumn("dbo.IncidentStatLines", "Category4", c => c.String());
            AlterColumn("dbo.IncidentStatLines", "Category5", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.IncidentStatLines", "Category5", c => c.Int(nullable: false));
            AlterColumn("dbo.IncidentStatLines", "Category4", c => c.Int(nullable: false));
            AlterColumn("dbo.IncidentStatLines", "Category3", c => c.Int(nullable: false));
            AlterColumn("dbo.IncidentStatLines", "Category2", c => c.Int(nullable: false));
            AlterColumn("dbo.IncidentStatLines", "Category1", c => c.Int(nullable: false));
            AlterColumn("dbo.IncidentStatLines", "Status", c => c.Int(nullable: false));
            AlterColumn("dbo.IncidentStatLines", "Assignee", c => c.Int(nullable: false));
            AlterColumn("dbo.IncidentStatLines", "Template", c => c.Int(nullable: false));
            AlterColumn("dbo.IncidentStatLines", "Source", c => c.Int(nullable: false));
            AlterColumn("dbo.IncidentStatLines", "Company", c => c.Int(nullable: false));
            AlterColumn("dbo.IncidentStatLines", "Customer", c => c.Int(nullable: false));
        }
    }
}
