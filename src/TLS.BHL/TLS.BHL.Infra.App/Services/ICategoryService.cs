using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TLS.BHL.Infra.App.Domain.DTO.Category;
using TLS.BHL.Infra.App.Domain.Entities;
using TLS.BHL.Infra.App.Domain.Helper;
using TLS.BHL.Infra.App.Domain.Models;
using TLS.Core.Service;

namespace TLS.BHL.Infra.App.Services
{
    public interface ICategoryService : IService
    {
        Task<ApiResponse> GetListCategory();
        Task<ApiResponse> CreateCategory(CreateCategoryDTO createCategory, CancellationToken cancellation);
        Task<ApiResponse> DeleteCategory(int id, CancellationToken cancellationToken);
        Task<ApiResponse> UpdateCategory(int id, UpdateCategoryDTO updateCategory, CancellationToken cancellationToken);
        Task<ApiResponse> GetCategoryById(int id);
    }
}
