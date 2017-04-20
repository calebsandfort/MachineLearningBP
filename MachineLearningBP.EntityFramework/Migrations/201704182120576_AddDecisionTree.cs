namespace MachineLearningBP.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddDecisionTree : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.DecisionTrees",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Target = c.Int(nullable: false),
                        RootJson = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.DecisionTrees");
        }
    }
}
