using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TLS.BHL.Infra.App.Services;
using TLS.BHL.Web.AdminHandlers.RequestHandlers.User;

namespace TLS.BHL.Web.AdminHandlers.RequestHandlers.Category
{
    public class GetListCategoryHandler : WebAdminHandlersBase<GetListCategoryHandler>, IRequestHandler<GetListCategoryInput, GetListCategoryOutput>
    {
        private ICategoryService CategoryService => GetService<ICategoryService>();
        public GetListCategoryHandler(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }
        public async Task<GetListCategoryOutput> Handle(GetListCategoryInput request, CancellationToken cancellationToken)
        {
         
            var categories = await CategoryService.GetListCategory();

            return new GetListCategoryOutput
            {
                Data = Mapper.Map<IEnumerable<GetListCategoryItem>>(categories)
            };
        }
    }

    }
    public class GetListCategoryInput : IRequest<GetListCategoryOutput>
    {
    }
    public class GetListCategoryOutput
    {
        public IEnumerable<GetListCategoryItem>? Data { get; set; }
    }
    public class GetListCategoryItem
    {
        public int Id { get; set; }
        public string NameVi { get; set; }
        public string NameEn { get; set; }

    }




