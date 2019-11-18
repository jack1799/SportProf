namespace SportProf.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CompetitionUpdate : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Competitions", "Status", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Competitions", "Status");
        }
    }
}
