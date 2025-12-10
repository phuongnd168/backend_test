using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TLS.BHL.Infra.App.Domain.DTO.Category;
using TLS.BHL.Infra.App.Domain.Helper;
using TLS.BHL.Infra.App.Domain.Models;
using TLS.BHL.Infra.App.Services;

namespace TLS.BHL.Web.AdminHandlers.RequestHandlers.Category
{
    public class CreateCategoryHandler : WebAdminHandlersBase<CreateCategoryHandler>, IRequestHandler<CreateCategoryInput, ApiResponse>
    {

        private ICategoryService CategoryService => GetService<ICategoryService>();
        public CreateCategoryHandler(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }
        public async Task<ApiResponse> Handle(CreateCategoryInput request, CancellationToken cancellationToken)
        {
          
                if (string.IsNullOrWhiteSpace(request.createCategory.NameVi))
                    return ResponseHelper.Error(400, "Tên tiếng Việt không được để trống");
                if (string.IsNullOrWhiteSpace(request.createCategory.NameEn))
                    return ResponseHelper.Error(400, "Tên tiếng Anh không được để trống");
                return await CategoryService.CreateCategory(request.createCategory, cancellationToken);
        
        }
    }
    public class CreateCategoryInput : IRequest<ApiResponse>
    {
        public CreateCategoryDTO createCategory { get; set; }
    }
}
