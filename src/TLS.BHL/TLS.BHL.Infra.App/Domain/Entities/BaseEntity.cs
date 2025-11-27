using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TLS.BHL.Infra.App.Domain.Entities
{
    public abstract class BaseEntity
    {
        public int Id { get; set; }

        public string Created_by { get; set; }

        //[DataType(DataType.DateTime)]
        public DateTime? Created_at { get; set; }
       // [DataType(DataType.DateTime)]
        public DateTime? Updated_at { get; set; }

        public string Updated_by { get; set; }
       // [DataType(DataType.DateTime)]
        public DateTime? Deleted_at { get; set; }

        public string Deleted_by { get; set; }

        public bool Deleted { get; set; }
    }
}
