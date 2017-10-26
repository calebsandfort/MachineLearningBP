namespace MachineLearningBP.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddIncidentTables1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Incidents",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        iSupportId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.IncidentStatLines",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Customer = c.Int(nullable: false),
                        Company = c.Int(nullable: false),
                        Source = c.Int(nullable: false),
                        Template = c.Int(nullable: false),
                        Month = c.Int(nullable: false),
                        DayOfWeek = c.Int(nullable: false),
                        Priority = c.Int(nullable: false),
                        Assignee = c.Int(nullable: false),
                        Status = c.Int(nullable: false),
                        Category1 = c.Int(nullable: false),
                        Category2 = c.Int(nullable: false),
                        Category3 = c.Int(nullable: false),
                        Category4 = c.Int(nullable: false),
                        Category5 = c.Int(nullable: false),
                        TotalTimeWorked = c.Int(nullable: false),
                        SampleId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Incidents", t => t.SampleId, cascadeDelete: true)
                .Index(t => t.SampleId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.IncidentStatLines", "SampleId", "dbo.Incidents");
            DropIndex("dbo.IncidentStatLines", new[] { "SampleId" });
            DropTable("dbo.IncidentStatLines");
            DropTable("dbo.Incidents");
        }
    }
}
