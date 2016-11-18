using System.Data;
using FluentMigrator;

namespace SimpleBlog.Migratons
{
    [Migration(1)]
    public class _001_UsersAndRoles : Migration
    {
        public override void Down()
        {
            Delete.Table("role_users");
            Delete.Table("users");
            Delete.Table("roles");
        }
        public override void Up()
        {
            //Create.Table("users")
            //    .WithColumn("id").AsInt32().Identity().PrimaryKey()
            //    .WithColumn("username").AsString(128)
            //    .WithColumn("email").AsString(250)
            //    .WithColumn("password_has").AsString(128);
            Create.Table("users")
                .WithColumn("id").AsInt32().Identity().PrimaryKey()
                .WithColumn("username").AsCustom("VARCHAR(128)")
                .WithColumn("email").AsCustom("VARCHAR(256)")
                .WithColumn("password_hash").AsCustom("VARCHAR(128)");

            Create.Table("roles")
                .WithColumn("id").AsInt32().Identity().PrimaryKey()
                .WithColumn("name").AsCustom("VARCHAR(128)");

            Create.Table("role_users")
                .WithColumn("id").AsInt32().Identity().PrimaryKey()
                .WithColumn("user_id").AsInt32().ForeignKey("users", "id").OnDelete(Rule.Cascade)
                .WithColumn("role_id").AsInt32().ForeignKey("roles", "id").OnDelete(Rule.Cascade);
        }
    }
}