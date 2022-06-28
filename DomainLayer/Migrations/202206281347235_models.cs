namespace DomainLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class models : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Bookings",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ServiceID = c.Int(nullable: false),
                        UserID = c.String(nullable: false),
                        AppointmentDate = c.DateTime(nullable: false),
                        AppointmentTime = c.String(nullable: false),
                        Address = c.String(maxLength: 100),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Services", t => t.ServiceID, cascadeDelete: true)
                .Index(t => t.ServiceID);
            
            CreateTable(
                "dbo.Services",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(maxLength: 50),
                        ShopID = c.Int(nullable: false),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Image1 = c.String(nullable: false),
                        Image2 = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Shops", t => t.ShopID, cascadeDelete: true)
                .Index(t => t.ShopID);
            
            CreateTable(
                "dbo.Shops",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 50),
                        PermanantAddress = c.String(nullable: false, maxLength: 100),
                        Contact1 = c.String(nullable: false, maxLength: 11),
                        Contact2 = c.String(nullable: false, maxLength: 11),
                        Image1 = c.String(nullable: false),
                        Image2 = c.String(),
                        UserID = c.String(nullable: false),
                        BookingDays = c.String(nullable: false),
                        OpeningHour = c.String(nullable: false),
                        ClosingHour = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Categories",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 50),
                        Description = c.String(maxLength: 100),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Products",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 50),
                        CategoryID = c.Int(nullable: false),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Description = c.String(maxLength: 100),
                        Image1 = c.String(nullable: false),
                        Image2 = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Categories", t => t.CategoryID, cascadeDelete: true)
                .Index(t => t.CategoryID);
            
            CreateTable(
                "dbo.Orders",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        UserID = c.Int(nullable: false),
                        ProductID = c.Int(nullable: false),
                        OrderDate = c.DateTime(nullable: false),
                        Quantity = c.Int(nullable: false),
                        ShippingAddress = c.String(nullable: false, maxLength: 100),
                        Amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Products", t => t.ProductID, cascadeDelete: true)
                .Index(t => t.ProductID);
            
            DropTable("dbo.Employees");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.Employees",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            DropForeignKey("dbo.Orders", "ProductID", "dbo.Products");
            DropForeignKey("dbo.Products", "CategoryID", "dbo.Categories");
            DropForeignKey("dbo.Bookings", "ServiceID", "dbo.Services");
            DropForeignKey("dbo.Services", "ShopID", "dbo.Shops");
            DropIndex("dbo.Orders", new[] { "ProductID" });
            DropIndex("dbo.Products", new[] { "CategoryID" });
            DropIndex("dbo.Services", new[] { "ShopID" });
            DropIndex("dbo.Bookings", new[] { "ServiceID" });
            DropTable("dbo.Orders");
            DropTable("dbo.Products");
            DropTable("dbo.Categories");
            DropTable("dbo.Shops");
            DropTable("dbo.Services");
            DropTable("dbo.Bookings");
        }
    }
}
