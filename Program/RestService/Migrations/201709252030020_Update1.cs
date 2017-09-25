namespace RestService.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Update1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Signatures", "lengthO", c => c.Double(nullable: false));
            AddColumn("dbo.Signatures", "lengthM", c => c.Double(nullable: false));
            AddColumn("dbo.Signatures", "height", c => c.Double(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Signatures", "height");
            DropColumn("dbo.Signatures", "lengthM");
            DropColumn("dbo.Signatures", "lengthO");
        }
    }
}
