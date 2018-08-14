namespace Cyber_Kitchen.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddUserName1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Ratings", "UserName", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Ratings", "UserName");
        }
    }
}
