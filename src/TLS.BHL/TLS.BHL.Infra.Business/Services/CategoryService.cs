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

        public async Task<ApiResponse> CreateCategory(CreateCategoryDTO createCategory, CancellationToken cancellation)
        {
            return await CategoryRepo.CreateCategory(createCategory, cancellation);
        }

        public async Task<ApiResponse> DeleteCategory(int id, CancellationToken cancellationToken)
        {
            return await CategoryRepo.DeleteCategory(id, cancellationToken);
        }

        public async Task<ApiResponse> GetCategoryById(int id)
        {
            return await CategoryRepo.GetCategoryById(id);
        }

        public async Task<ApiResponse> GetListCategory()
        {
            return await CategoryRepo.GetListCategory();
        }

        public async Task<ApiResponse> UpdateCategory(int id, UpdateCategoryDTO updateCategory, CancellationToken cancellationToken)
        {
            return await CategoryRepo.UpdateCategory(id, updateCategory, cancellationToken);
        }
    }
}
