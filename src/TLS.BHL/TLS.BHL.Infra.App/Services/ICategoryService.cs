using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TLS.BHL.Infra.App.Domain.Entities;
using TLS.Core.Service;

namespace TLS.BHL.Infra.App.Services
{
    public interface ICategoryService : IService
    {
        Task<IEnumerable<CategoryEntity>> GetListCategory();
    }
}
