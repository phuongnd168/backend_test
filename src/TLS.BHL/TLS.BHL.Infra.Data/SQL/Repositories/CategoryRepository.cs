using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TLS.BHL.Infra.App.Domain.DTO.Category;
using TLS.BHL.Infra.App.Domain.Entities;
using TLS.BHL.Infra.App.Domain.Helper;
using TLS.BHL.Infra.App.Domain.Models;
using TLS.BHL.Infra.App.Repositories;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace TLS.BHL.Infra.Data.SQL.Repositories
{
    public class CategoryRepository : BHLRepositoryBase<CategoryRepository>, ICategoryRepository
    {
        private readonly IBHLDbContext Context;
        public CategoryRepository(IServiceProvider serviceProvider, IBHLDbContext context) : base(serviceProvider)
        {
            Context = context;
        }

        public async Task<ApiResponse> GetListCategory()
        {
            try { 
                  var categories = await Context.Categories.ToListAsync();
                  List<GetListCategoryItemDTO> data = new List<GetListCategoryItemDTO>();
                  foreach (var category in categories){
                      data.Add(new GetListCategoryItemDTO
                      {
                        Id = category.Id,
                        NameEn = category.NameEn,
                        NameVi = category.NameVi
                      });
                  }


                return ResponseHelper.Success("Thành công", data);
            }
            catch (Exception ex)
            {
                return ResponseHelper.Error(500, ex.Message);
            }
          

        }
    }
}
