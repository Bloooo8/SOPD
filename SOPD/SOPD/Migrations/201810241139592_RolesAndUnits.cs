namespace SOPD.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RolesAndUnits : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.OrganizationalUnits",
                c => new
                    {
                        OrganizationalUnitID = c.Int(nullable: false, identity: true),
                        UnitName = c.String(),
                    })
                .PrimaryKey(t => t.OrganizationalUnitID);
            
            AddColumn("dbo.Reviews", "ThesisID", c => c.Int(nullable: false));
            AddColumn("dbo.Theses", "EnglishTitle", c => c.String());
            AddColumn("dbo.Theses", "Abstract", c => c.String());
            AddColumn("dbo.Theses", "EnglishAbstract", c => c.String());
            AddColumn("dbo.Theses", "ApprovalDate", c => c.DateTime());
            AddColumn("dbo.Theses", "KeyWords", c => c.String());
            AddColumn("dbo.Theses", "EnglishKeyWords", c => c.String());
            AddColumn("dbo.Theses", "OrganizationalUnitID", c => c.Int(nullable: false));
            AddColumn("dbo.Theses", "Promoter_Id", c => c.String(maxLength: 128));
            AddColumn("dbo.Theses", "Reviewer_Id", c => c.String(maxLength: 128));
            AddColumn("dbo.AspNetUsers", "OrganizationalUnitID", c => c.Int());
            AlterColumn("dbo.Theses", "ReviewerID", c => c.Int());
            CreateIndex("dbo.Theses", "OrganizationalUnitID");
            CreateIndex("dbo.Theses", "Promoter_Id");
            CreateIndex("dbo.Theses", "Reviewer_Id");
            CreateIndex("dbo.AspNetUsers", "OrganizationalUnitID");
            CreateIndex("dbo.Reviews", "ThesisID");
            AddForeignKey("dbo.Theses", "OrganizationalUnitID", "dbo.OrganizationalUnits", "OrganizationalUnitID", cascadeDelete: true);
            AddForeignKey("dbo.AspNetUsers", "OrganizationalUnitID", "dbo.OrganizationalUnits", "OrganizationalUnitID");
            AddForeignKey("dbo.Theses", "Promoter_Id", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.Theses", "Reviewer_Id", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.Reviews", "ThesisID", "dbo.Theses", "ThesisID", cascadeDelete: true);
            DropColumn("dbo.Theses", "Descritpion");
            DropColumn("dbo.Theses", "ReviewID");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Theses", "ReviewID", c => c.Int(nullable: false));
            AddColumn("dbo.Theses", "Descritpion", c => c.String());
            DropForeignKey("dbo.Reviews", "ThesisID", "dbo.Theses");
            DropForeignKey("dbo.Theses", "Reviewer_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Theses", "Promoter_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUsers", "OrganizationalUnitID", "dbo.OrganizationalUnits");
            DropForeignKey("dbo.Theses", "OrganizationalUnitID", "dbo.OrganizationalUnits");
            DropIndex("dbo.Reviews", new[] { "ThesisID" });
            DropIndex("dbo.AspNetUsers", new[] { "OrganizationalUnitID" });
            DropIndex("dbo.Theses", new[] { "Reviewer_Id" });
            DropIndex("dbo.Theses", new[] { "Promoter_Id" });
            DropIndex("dbo.Theses", new[] { "OrganizationalUnitID" });
            AlterColumn("dbo.Theses", "ReviewerID", c => c.Int(nullable: false));
            DropColumn("dbo.AspNetUsers", "OrganizationalUnitID");
            DropColumn("dbo.Theses", "Reviewer_Id");
            DropColumn("dbo.Theses", "Promoter_Id");
            DropColumn("dbo.Theses", "OrganizationalUnitID");
            DropColumn("dbo.Theses", "EnglishKeyWords");
            DropColumn("dbo.Theses", "KeyWords");
            DropColumn("dbo.Theses", "ApprovalDate");
            DropColumn("dbo.Theses", "EnglishAbstract");
            DropColumn("dbo.Theses", "Abstract");
            DropColumn("dbo.Theses", "EnglishTitle");
            DropColumn("dbo.Reviews", "ThesisID");
            DropTable("dbo.OrganizationalUnits");
        }
    }
}
