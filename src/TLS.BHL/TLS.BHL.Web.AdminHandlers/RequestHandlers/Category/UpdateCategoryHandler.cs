using Azure.Core;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TLS.BHL.Infra.App.Domain.DTO.Category;
using TLS.BHL.Infra.App.Domain.DTO.Product;
using TLS.BHL.Infra.App.Domain.Helper;
using TLS.BHL.Infra.App.Domain.Models;
using TLS.BHL.Infra.App.Services;

namespace TLS.BHL.Web.AdminHandlers.RequestHandlers.Category
{
    public class UpdateCategoryHandler : WebAdminHandlersBase<UpdateCategoryHandler>, IRequestHandler<UpdateCategoryInput, ApiResponse>
    {


        private ICategoryService CategoryService => GetService<ICategoryService>();
        public UpdateCategoryHandler(IServiceProvider serviceProvider) : base(serviceProvider)
        {

        }

        public async Task<ApiResponse> Handle(UpdateCategoryInput input, CancellationToken cancellationToken)
        {

            if (string.IsNullOrWhiteSpace(input.category.NameVi))
                return ResponseHelper.Error(400, "Tên tiếng Việt không được để trống");
            if (string.IsNullOrWhiteSpace(input.category.NameEn))
                return ResponseHelper.Error(400, "Tên tiếng Anh không được để trống");
            return await CategoryService.UpdateCategory(input.CategoryId, input.category, cancellationToken);

        }
    }
    public class UpdateCategoryInput : IRequest<ApiResponse>
    {
        public int CategoryId { get; set; }
        public UpdateCategoryDTO category { get; set; }


    }

}
