using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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

        public async Task<ApiResponse> CreateCategory(CreateCategoryDTO createCategory, CancellationToken cancellation)
        {
            try
            {
                var category = new CategoryEntity
                {
                    NameEn = createCategory.NameEn,
                    NameVi = createCategory.NameVi,
                    Created_at = DateTime.Now
                };
            await Context.Categories.AddAsync(category);
            var result = await Context.SaveChangesAsync(cancellation);
            if (result > 0)
            {
                return ResponseHelper.Created("Thêm danh mục thành công");
            }
            return ResponseHelper.Error(400, "Thêm danh mục thất bại");
            }
            catch (Exception ex)
            {
                return ResponseHelper.Error(500, ex.Message);
            }
        }

        public async Task<ApiResponse> DeleteCategory(int id, CancellationToken cancellationToken)
        {
            try
            {
                var category = await Context.Categories.FindAsync(id);
                if(category == null)
                {
                    return ResponseHelper.Error(404, "Danh mục không tồn tại");
                }
                category.Deleted = true;
                var result = await Context.SaveChangesAsync(cancellationToken);
                if (result > 0)
                {
                    return ResponseHelper.Success("Xóa danh mục thành công");
                }
                return ResponseHelper.Error(400, "Xóa danh mục thất bại");
            }
            catch (Exception ex)
            {
                return ResponseHelper.Error(500, ex.Message);
            }
        }

        public async Task<ApiResponse> GetCategoryById(int id)
        {
            try
            {
                var cate = await Context.Categories.FindAsync(id);
                if (cate == null)
                {
                    return ResponseHelper.Error(404, "Danh mục không tồn tại");
                }
                var category = new GetListCategoryItemDTO
                {
                    Id = cate.Id,
                    NameEn = cate.NameEn,
                    NameVi = cate.NameVi
                };
                return ResponseHelper.Success("Thành công", category);
           
            }
            catch (Exception ex)
            {
                return ResponseHelper.Error(500, ex.Message);
            }
        }

        public async Task<ApiResponse> GetListCategory()
        {
            try { 
                  var categories = await Context.Categories.Where(c => !c.Deleted).ToListAsync();
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

        public async Task<ApiResponse> UpdateCategory(int id, UpdateCategoryDTO updateCategory, CancellationToken cancellationToken)
        {
            try
            {
                var cate = await Context.Categories.FindAsync(id);
                if (cate == null)
                {
                    return ResponseHelper.Error(404, "Danh mục không tồn tại");
                }
                cate.NameEn = updateCategory.NameEn;
                cate.NameVi = updateCategory.NameVi;
                cate.Updated_at = DateTime.Now;
               
                var result = await Context.SaveChangesAsync(cancellationToken);
                if (result > 0)
                {
                    return ResponseHelper.Updated("Sửa danh mục thành công");
                }
                return ResponseHelper.Error(400, "Sửa danh mục thất bại");
           
            }
            catch (Exception ex)
            {
                return ResponseHelper.Error(500, ex.Message);
            }

        }
    }
}
