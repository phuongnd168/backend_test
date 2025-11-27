using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TLS.BHL.Infra.App.Domain.Entities
{
    [Table("User_Roles")]
    public class UserRoleEntity
    {
        public int Id { get; set; }
        public int RoleId {  get; set; }

        public RoleEntity Role { get; set; }


        public int UserId {  get; set; }

        public UserEntity User { get; set; }
        //public IEnumerable<RoleEntity> Roles { get; set; }

        //public IEnumerable<UserEntity> Users { get; set; }
    }
}
