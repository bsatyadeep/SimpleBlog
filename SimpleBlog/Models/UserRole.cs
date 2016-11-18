using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;

namespace SimpleBlog.Models
{
    public class UserRole
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int RoleId { get; set; }
    }

    public class UserRoleMap : ClassMapping<UserRole>
    {
        public UserRoleMap()
        {
            Table("role_users");
            Id(x => x.Id, x => x.Generator(Generators.Identity));
            Property(x => x.RoleId, x => x.NotNullable(true));
            Property(x => x.UserId, x => x.NotNullable(true));
        }
    }
}