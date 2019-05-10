namespace Bootstarter.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class first : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Ideas",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 200),
                        Description = c.String(nullable: false),
                        Image = c.String(),
                        Goal = c.Decimal(nullable: false, precision: 18, scale: 2),
                        MoneyGathered = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Cancelled = c.Boolean(nullable: false),
                        SubmissionDate = c.DateTime(nullable: false),
                        StartDate = c.DateTime(nullable: false),
                        EndDate = c.DateTime(nullable: false),
                        CancellationDate = c.DateTime(),
                        FounderId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.FounderId, cascadeDelete: true)
                .Index(t => t.FounderId);
            
            CreateTable(
                "dbo.Interests",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        IdeaId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Ideas", t => t.IdeaId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .Index(t => t.UserId)
                .Index(t => t.IdeaId);
            
            CreateTable(
                "dbo.Transactions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        IdeaId = c.Int(nullable: false),
                        DonatorId = c.String(nullable: false, maxLength: 128),
                        DonationMoney = c.Decimal(nullable: false, precision: 18, scale: 2),
                        DonationDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.DonatorId, cascadeDelete: true)
                .ForeignKey("dbo.Ideas", t => t.IdeaId)
                .Index(t => t.IdeaId)
                .Index(t => t.DonatorId);
            
            AddColumn("dbo.AspNetUsers", "CompanyName", c => c.String());
            AddColumn("dbo.AspNetUsers", "FirstName", c => c.String());
            AddColumn("dbo.AspNetUsers", "LastName", c => c.String());
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Ideas", "FounderId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Transactions", "IdeaId", "dbo.Ideas");
            DropForeignKey("dbo.Transactions", "DonatorId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Interests", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Interests", "IdeaId", "dbo.Ideas");
            DropIndex("dbo.Transactions", new[] { "DonatorId" });
            DropIndex("dbo.Transactions", new[] { "IdeaId" });
            DropIndex("dbo.Interests", new[] { "IdeaId" });
            DropIndex("dbo.Interests", new[] { "UserId" });
            DropIndex("dbo.Ideas", new[] { "FounderId" });
            DropColumn("dbo.AspNetUsers", "LastName");
            DropColumn("dbo.AspNetUsers", "FirstName");
            DropColumn("dbo.AspNetUsers", "CompanyName");
            DropTable("dbo.Transactions");
            DropTable("dbo.Interests");
            DropTable("dbo.Ideas");
        }
    }
}
