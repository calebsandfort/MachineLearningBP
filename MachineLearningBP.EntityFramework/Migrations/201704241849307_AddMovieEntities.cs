namespace MachineLearningBP.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddMovieEntities : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Movies",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                        WideRelease = c.DateTime(nullable: false),
                        TimeGroupingId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.MovieYears", t => t.TimeGroupingId, cascadeDelete: true)
                .Index(t => t.TimeGroupingId);
            
            CreateTable(
                "dbo.MovieStatLines",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        RunTime = c.Double(nullable: false),
                        Budget = c.Double(nullable: false),
                        TheaterCount = c.Double(nullable: false),
                        TheaterAverage = c.Double(nullable: false),
                        WeekendDuration = c.Double(nullable: false),
                        Opening = c.Double(nullable: false),
                        SampleId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Movies", t => t.SampleId, cascadeDelete: true)
                .Index(t => t.SampleId);
            
            CreateTable(
                "dbo.MovieYears",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Start = c.DateTime(nullable: false),
                        End = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Movies", "TimeGroupingId", "dbo.MovieYears");
            DropForeignKey("dbo.MovieStatLines", "SampleId", "dbo.Movies");
            DropIndex("dbo.MovieStatLines", new[] { "SampleId" });
            DropIndex("dbo.Movies", new[] { "TimeGroupingId" });
            DropTable("dbo.MovieYears");
            DropTable("dbo.MovieStatLines");
            DropTable("dbo.Movies");
        }
    }
}
