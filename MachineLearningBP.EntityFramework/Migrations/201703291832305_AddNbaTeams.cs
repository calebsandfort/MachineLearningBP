namespace MachineLearningBP.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddNbaTeams : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.NbaTeams",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        BasketballReferenceAbbr = c.String(),
                        EspnName = c.String(),
                        CoversName = c.String(),
                        City = c.String(),
                        Mascot = c.String(),
                        DraftKingsIdentifier = c.Int(nullable: false),
                        DraftKingsAbbr = c.String(),
                        EspnAbbr = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.NbaTeams");
        }
    }
}
