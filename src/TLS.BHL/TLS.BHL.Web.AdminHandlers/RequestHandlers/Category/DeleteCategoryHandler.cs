using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TLS.BHL.Infra.App.Domain.Models;
using TLS.BHL.Infra.App.Services;
using static TLS.BHL.Web.AdminHandlers.RequestHandlers.Category.DeleteCategoryHandler;

namespace TLS.BHL.Web.AdminHandlers.RequestHandlers.Category
{
    public class DeleteCategoryHandler : WebAdminHandlersBase<DeleteCategoryHandler>, IRequestHandler<DeleteCategoryInput, ApiResponse>
    {
        private ICategoryService CategoryService => GetService<ICategoryService>();
        public DeleteCategoryHandler(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }
        public async Task<ApiResponse> Handle(DeleteCategoryInput request, CancellationToken cancellationToken)
        {

            return await CategoryService.DeleteCategory(request.id, cancellationToken);

        }
        public record DeleteCategoryInput(int id) : IRequest<ApiResponse>;
    }
}
