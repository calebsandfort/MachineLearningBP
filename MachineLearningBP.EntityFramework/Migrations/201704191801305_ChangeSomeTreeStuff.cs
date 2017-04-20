namespace MachineLearningBP.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeSomeTreeStuff : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.NbaStatLines", "TreePoints", c => c.Double(nullable: false));
            AlterColumn("dbo.NbaAtsTreeExamples", "Result", c => c.Double(nullable: false));
            DropColumn("dbo.NbaStatLines", "AtsTree");
        }
        
        public override void Down()
        {
            AddColumn("dbo.NbaStatLines", "AtsTree", c => c.String());
            AlterColumn("dbo.NbaAtsTreeExamples", "Result", c => c.String());
            DropColumn("dbo.NbaStatLines", "TreePoints");
        }
    }
}
