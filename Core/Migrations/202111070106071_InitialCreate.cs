namespace Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ItemsTbls",
                c => new
                    {
                        ID = c.Long(nullable: false, identity: true),
                        Image = c.String(),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Quantity = c.Int(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Items_LocTbl",
                c => new
                    {
                        ID = c.Long(nullable: false, identity: true),
                        ItemID = c.Long(nullable: false),
                        Name = c.String(),
                        Description = c.String(),
                        LanguageID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.ItemsTbls", t => t.ItemID, cascadeDelete: true)
                .Index(t => t.ItemID);
            
            CreateTable(
                "dbo.OrderInvoicesTbls",
                c => new
                    {
                        ID = c.Long(nullable: false, identity: true),
                        ItemID = c.Long(nullable: false),
                        OrderID = c.Long(nullable: false),
                        Quantity = c.Int(nullable: false),
                        ItemPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        TotalPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.OrdersTbls", t => t.OrderID, cascadeDelete: true)
                .ForeignKey("dbo.ItemsTbls", t => t.ItemID, cascadeDelete: true)
                .Index(t => t.ItemID)
                .Index(t => t.OrderID);
            
            CreateTable(
                "dbo.OrdersTbls",
                c => new
                    {
                        ID = c.Long(nullable: false, identity: true),
                        UserID = c.Long(nullable: false),
                        StatusID = c.Int(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.UsersTbls", t => t.UserID, cascadeDelete: true)
                .Index(t => t.UserID);
            
            CreateTable(
                "dbo.UsersTbls",
                c => new
                    {
                        ID = c.Long(nullable: false, identity: true),
                        Name = c.String(),
                        Email = c.String(),
                        Password = c.String(),
                        DefaultLanguageID = c.Int(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.UserDevicesTbls",
                c => new
                    {
                        ID = c.Long(nullable: false, identity: true),
                        UserID = c.Long(nullable: false),
                        ApiToken = c.String(),
                        DeviceToken = c.String(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.UsersTbls", t => t.UserID, cascadeDelete: true)
                .Index(t => t.UserID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserDevicesTbls", "UserID", "dbo.UsersTbls");
            DropForeignKey("dbo.OrderInvoicesTbls", "ItemID", "dbo.ItemsTbls");
            DropForeignKey("dbo.OrderInvoicesTbls", "OrderID", "dbo.OrdersTbls");
            DropForeignKey("dbo.OrdersTbls", "UserID", "dbo.UsersTbls");
            DropForeignKey("dbo.Items_LocTbl", "ItemID", "dbo.ItemsTbls");
            DropIndex("dbo.UserDevicesTbls", new[] { "UserID" });
            DropIndex("dbo.OrdersTbls", new[] { "UserID" });
            DropIndex("dbo.OrderInvoicesTbls", new[] { "OrderID" });
            DropIndex("dbo.OrderInvoicesTbls", new[] { "ItemID" });
            DropIndex("dbo.Items_LocTbl", new[] { "ItemID" });
            DropTable("dbo.UserDevicesTbls");
            DropTable("dbo.UsersTbls");
            DropTable("dbo.OrdersTbls");
            DropTable("dbo.OrderInvoicesTbls");
            DropTable("dbo.Items_LocTbl");
            DropTable("dbo.ItemsTbls");
        }
    }
}
