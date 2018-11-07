namespace SOPD.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ForeignKeys : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Theses", new[] { "Promoter_Id" });
            DropIndex("dbo.Theses", new[] { "Reviewer_Id" });
            DropColumn("dbo.Theses", "PromoterID");
            DropColumn("dbo.Theses", "ReviewerID");
            RenameColumn(table: "dbo.Theses", name: "Promoter_Id", newName: "PromoterID");
            RenameColumn(table: "dbo.Theses", name: "Reviewer_Id", newName: "ReviewerID");
            AlterColumn("dbo.Theses", "PromoterID", c => c.String(maxLength: 128));
            AlterColumn("dbo.Theses", "AuthorID", c => c.String(maxLength: 128));
            AlterColumn("dbo.Theses", "ReviewerID", c => c.String(maxLength: 128));
            CreateIndex("dbo.Theses", "PromoterID");
            CreateIndex("dbo.Theses", "AuthorID");
            CreateIndex("dbo.Theses", "ReviewerID");
            AddForeignKey("dbo.Theses", "AuthorID", "dbo.AspNetUsers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Theses", "AuthorID", "dbo.AspNetUsers");
            DropIndex("dbo.Theses", new[] { "ReviewerID" });
            DropIndex("dbo.Theses", new[] { "AuthorID" });
            DropIndex("dbo.Theses", new[] { "PromoterID" });
            AlterColumn("dbo.Theses", "ReviewerID", c => c.Int());
            AlterColumn("dbo.Theses", "AuthorID", c => c.Int(nullable: false));
            AlterColumn("dbo.Theses", "PromoterID", c => c.Int(nullable: false));
            RenameColumn(table: "dbo.Theses", name: "ReviewerID", newName: "Reviewer_Id");
            RenameColumn(table: "dbo.Theses", name: "PromoterID", newName: "Promoter_Id");
            AddColumn("dbo.Theses", "ReviewerID", c => c.Int());
            AddColumn("dbo.Theses", "PromoterID", c => c.Int(nullable: false));
            CreateIndex("dbo.Theses", "Reviewer_Id");
            CreateIndex("dbo.Theses", "Promoter_Id");
        }
    }
}
