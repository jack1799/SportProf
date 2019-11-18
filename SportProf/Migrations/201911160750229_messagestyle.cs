namespace SportProf.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class messagestyle : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Messages", "Type", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Messages", "Type");
        }
    }
}
