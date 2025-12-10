using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TLS.BHL.Infra.App.Domain.Models;
using TLS.BHL.Infra.App.Services;

namespace TLS.BHL.Web.AdminHandlers.RequestHandlers.Category
{
    public record GetCategoryByIdInput(int id) : IRequest<ApiResponse>
    {
    }
    public class GetCategoryByIdHandler
     : WebAdminHandlersBase<GetCategoryByIdHandler>,
       IRequestHandler<GetCategoryByIdInput, ApiResponse>
    {
        private ICategoryService CategoryService => GetService<ICategoryService>();

        public GetCategoryByIdHandler(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        public async Task<ApiResponse> Handle(GetCategoryByIdInput request, CancellationToken cancellationToken)
        {
            return await CategoryService.GetCategoryById(request.id);

        }


    }
}
