namespace MachineLearningBP.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddStatLineToExample : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.NbaPointsExamples", "StatLineId", c => c.Int(nullable: false));
            CreateIndex("dbo.NbaPointsExamples", "StatLineId");
            AddForeignKey("dbo.NbaPointsExamples", "StatLineId", "dbo.NbaStatLines", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.NbaPointsExamples", "StatLineId", "dbo.NbaStatLines");
            DropIndex("dbo.NbaPointsExamples", new[] { "StatLineId" });
            DropColumn("dbo.NbaPointsExamples", "StatLineId");
        }
    }
}
