using FluentMigrator;

namespace SimpleBlog.Migratons
{
    [Migration(3)]
    public class _003_AddContentToPost : Migration
    {
        public override void Down()
        {
            Delete.Column("content").FromTable("posts");
        }

        public override void Up()
        {
            //Create.Column("content").OnTable("posts").AsCustom("TEXT");
            Alter.Column("content").OnTable("posts").AsCustom("TEXT");
        }
    }
}