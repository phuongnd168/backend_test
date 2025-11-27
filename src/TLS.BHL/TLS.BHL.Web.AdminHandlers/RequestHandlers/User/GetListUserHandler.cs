using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TLS.BHL.Infra.App.Domain.DTO.User;
using TLS.BHL.Infra.App.Services;

namespace TLS.BHL.Web.AdminHandlers.RequestHandlers.User
{
    public class GetListUserHandler : WebAdminHandlersBase<GetListUserHandler>, IRequestHandler<GetListUserInput, GetListUserOutput>
    {
        private IUserService UserService => GetService<IUserService>();
        public GetListUserHandler(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        public async Task<GetListUserOutput> Handle(GetListUserInput input, CancellationToken cancellationToken)
        {
            var users = await UserService.GetListUser(input.Name);

            return new GetListUserOutput
            {
                Data = Mapper.Map<List<GetListUserItem>>(users)
            };
        }
    }
    public class GetListUserInput : IRequest<GetListUserOutput>
    {
        public string? Name { get; set; }
    }
    public class GetListUserOutput
    {
        public IEnumerable<GetListUserItem>? Data { get; set; }
    }
    public class GetListUserItem
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public DateTime? CreatedTime { get; set; }
    }
}
