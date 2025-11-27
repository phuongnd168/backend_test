using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TLS.BHL.Infra.App.Services;
using TLS.Core.Data;

namespace TLS.BHL.Web.AdminHandlers.RequestHandlers.User
{
    public class SearchUserHandler : WebAdminHandlersBase<SearchUserHandler>, IRequestHandler<SearchUserInput, SearchUserOutput>
    {
        private IUserService UserService => GetService<IUserService>();
        public SearchUserHandler(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            
        }
        public async Task<SearchUserOutput> Handle(SearchUserInput input, CancellationToken cancellationToken)
        {
            var result = await UserService.SearchUserAsync(input.Filters, input.SortField, input.SortOrder, input.Page.HasValue ? input.Page.Value : 0, input.PerPage.HasValue ? input.PerPage.Value : int.MaxValue);

            return new SearchUserOutput
            {
                Data = Mapper.Map<List<UserItem>>(result.Items),
                PageNumber = result.PageNumber,
                TotalCount = result.TotalCount,
                TotalPages = result.TotalPages,
            };
        }
    }
    public class SearchUserInput : IRequest<SearchUserOutput>
    {
        public string? SortField { get; set; }
        public string? SortOrder { get; set; }

        public int? Page { get; set; } = 1;

        public int Total { get; set; } = 0;

        public int? PerPage { get; set; } = 10;

        public int? First { get; set; } = 0;
        public Dictionary<string, Dictionary<string, string?>>? Filters { get; set; }
    }

    public class SearchUserOutput
    {
        public List<UserItem>? Data { get; set; }

        public int PageNumber { get; set; }
        public int TotalPages { get; set; }
        public int TotalCount { get; set; }
    }
}
