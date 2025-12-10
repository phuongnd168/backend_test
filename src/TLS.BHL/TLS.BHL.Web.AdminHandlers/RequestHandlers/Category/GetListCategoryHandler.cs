using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TLS.BHL.Infra.App.Domain.Models;
using TLS.BHL.Infra.App.Services;
using TLS.BHL.Web.AdminHandlers.RequestHandlers.User;

namespace TLS.BHL.Web.AdminHandlers.RequestHandlers.Category
{
    public class GetListCategoryHandler : WebAdminHandlersBase<GetListCategoryHandler>, IRequestHandler<GetListCategoryInput, ApiResponse>
    {
        
        private ICategoryService CategoryService => GetService<ICategoryService>();
        public GetListCategoryHandler(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }
        public async Task<ApiResponse> Handle(GetListCategoryInput request, CancellationToken cancellationToken)
        {
        

            return await CategoryService.GetListCategory();
        }
    }
    public class GetListCategoryInput : IRequest<ApiResponse>
    {
    }


}




