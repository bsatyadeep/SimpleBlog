using FluentMigrator;
using System.Data;

namespace SimpleBlog.Migratons
{
    [Migration(2)]
    public class _002_PostsAndTags : Migration
    {
        public override void Down()
        {
            Delete.Table("post_tag");
            Delete.Table("posts");
            Delete.Table("tags");
        }

        public override void Up()
        {
            Create.Table("posts")
                .WithColumn("id").AsInt32().PrimaryKey().Identity()
                .WithColumn("user_id").AsInt32().ForeignKey("users", "id")
                .WithColumn("title").AsString(128).NotNullable()
                .WithColumn("slug").AsString(128).NotNullable()
                .WithColumn("content").AsString().NotNullable()
                .WithColumn("created_at").AsDateTime().NotNullable()
                .WithColumn("updated_at").AsDateTime().Nullable()
                .WithColumn("deleted_at").AsDateTime().Nullable();

            Create.Table("tags")
                .WithColumn("id").AsInt32().PrimaryKey().Identity()
                .WithColumn("slug").AsString(128).NotNullable()
                .WithColumn("name").AsString(128).NotNullable();

            Create.Table("post_tags")
                .WithColumn("post_id").AsInt32().ForeignKey("posts", "id").OnDelete(Rule.Cascade)
                .WithColumn("tag_id").AsInt32().ForeignKey("tags", "id").OnDelete(Rule.Cascade);

        }
    }
}