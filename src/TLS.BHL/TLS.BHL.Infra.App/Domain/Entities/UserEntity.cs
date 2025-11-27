using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TLS.BHL.Infra.App.Domain.Entities
{
    [Table("Users")]
    public class UserEntity : BaseEntity
    {
        
        
        public string UserId { get; set; }

        public required string FullName { get; set; }

        public string Mobile {  get; set; }

        public string Email { get; set; }

        public string Address { get; set; }

        public string Avatar { get; set; }

        public int Type {  get; set; }

        public int Status {  get; set; }

        public ICollection<OrderEntity> Orders { get; set; } = new List<OrderEntity>();
        public IList<UserRoleEntity> UserRoles { get; set; }

    }
}
