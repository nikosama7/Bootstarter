namespace Bootstarter.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class creditcard : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "CreditCard_CardNumber", c => c.String(nullable: false));
            AddColumn("dbo.AspNetUsers", "CreditCard_CardType", c => c.Int(nullable: false));
            AddColumn("dbo.AspNetUsers", "CreditCard_CVV", c => c.Int(nullable: false));
            AddColumn("dbo.AspNetUsers", "CreditCard_CardOwner", c => c.String(nullable: false));
            AddColumn("dbo.AspNetUsers", "CreditCard_Month", c => c.Int(nullable: false));
            AddColumn("dbo.AspNetUsers", "CreditCard_Year", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "CreditCard_Year");
            DropColumn("dbo.AspNetUsers", "CreditCard_Month");
            DropColumn("dbo.AspNetUsers", "CreditCard_CardOwner");
            DropColumn("dbo.AspNetUsers", "CreditCard_CVV");
            DropColumn("dbo.AspNetUsers", "CreditCard_CardType");
            DropColumn("dbo.AspNetUsers", "CreditCard_CardNumber");
        }
    }
}
