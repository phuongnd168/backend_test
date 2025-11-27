using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TLS.BHL.Infra.App.Domain.Entities;
using TLS.BHL.Infra.App.Repositories;

namespace TLS.BHL.Infra.Data.SQL.Repositories
{
    public class CategoryRepository : BHLRepositoryBase<CategoryRepository>, ICategoryRepository
    {
        private readonly IBHLDbContext Context;
        public CategoryRepository(IServiceProvider serviceProvider, IBHLDbContext context) : base(serviceProvider)
        {
            Context = context;
        }

        public async Task<IEnumerable<CategoryEntity>> GetListCategory()
        {

      
            return await Context.Categories.ToListAsync();

        }
    }
}
