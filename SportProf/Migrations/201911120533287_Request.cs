namespace SportProf.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Request : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Requests",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Description = c.String(),
                        CompetitionId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Competitions", t => t.CompetitionId, cascadeDelete: true)
                .Index(t => t.CompetitionId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Requests", "CompetitionId", "dbo.Competitions");
            DropIndex("dbo.Requests", new[] { "CompetitionId" });
            DropTable("dbo.Requests");
        }
    }
}
