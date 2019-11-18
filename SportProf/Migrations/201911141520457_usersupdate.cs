namespace SportProf.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class usersupdate : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Competitions", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.Requests", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.UserInCompetitions", new[] { "User_Id" });
            DropColumn("dbo.Competitions", "ApplicationUserId");
            DropColumn("dbo.Requests", "ApplicationUserId");
            DropColumn("dbo.UserInCompetitions", "UserId");
            RenameColumn(table: "dbo.Competitions", name: "ApplicationUser_Id", newName: "ApplicationUserId");
            RenameColumn(table: "dbo.Requests", name: "ApplicationUser_Id", newName: "ApplicationUserId");
            RenameColumn(table: "dbo.UserInCompetitions", name: "User_Id", newName: "UserId");
            AlterColumn("dbo.Competitions", "ApplicationUserId", c => c.String(maxLength: 128));
            AlterColumn("dbo.Requests", "ApplicationUserId", c => c.String(maxLength: 128));
            AlterColumn("dbo.UserInCompetitions", "UserId", c => c.String(maxLength: 128));
            CreateIndex("dbo.Competitions", "ApplicationUserId");
            CreateIndex("dbo.Requests", "ApplicationUserId");
            CreateIndex("dbo.UserInCompetitions", "UserId");
        }
        
        public override void Down()
        {
            DropIndex("dbo.UserInCompetitions", new[] { "UserId" });
            DropIndex("dbo.Requests", new[] { "ApplicationUserId" });
            DropIndex("dbo.Competitions", new[] { "ApplicationUserId" });
            AlterColumn("dbo.UserInCompetitions", "UserId", c => c.Int());
            AlterColumn("dbo.Requests", "ApplicationUserId", c => c.Int(nullable: false));
            AlterColumn("dbo.Competitions", "ApplicationUserId", c => c.Int());
            RenameColumn(table: "dbo.UserInCompetitions", name: "UserId", newName: "User_Id");
            RenameColumn(table: "dbo.Requests", name: "ApplicationUserId", newName: "ApplicationUser_Id");
            RenameColumn(table: "dbo.Competitions", name: "ApplicationUserId", newName: "ApplicationUser_Id");
            AddColumn("dbo.UserInCompetitions", "UserId", c => c.Int());
            AddColumn("dbo.Requests", "ApplicationUserId", c => c.Int(nullable: false));
            AddColumn("dbo.Competitions", "ApplicationUserId", c => c.Int());
            CreateIndex("dbo.UserInCompetitions", "User_Id");
            CreateIndex("dbo.Requests", "ApplicationUser_Id");
            CreateIndex("dbo.Competitions", "ApplicationUser_Id");
        }
    }
}
