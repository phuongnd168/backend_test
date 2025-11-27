using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TLS.BHL.Infra.App.Domain.Entities;
using TLS.BHL.Infra.App.Repositories;
using TLS.BHL.Infra.App.Services;
using TLS.Core.Service;

namespace TLS.BHL.Infra.Business.Services
{
    public class CategoryService : ServiceBase, ICategoryService
    {
        private readonly ICategoryRepository CategoryRepo;
        public CategoryService(IServiceProvider serviceProvider, ICategoryRepository categoryRepo) : base(serviceProvider)
        {
            CategoryRepo = categoryRepo;
        }

        public async Task<IEnumerable<CategoryEntity>> GetListCategory()
        {
            return await CategoryRepo.GetListCategory();
        }
    }
}
