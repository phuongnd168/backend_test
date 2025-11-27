using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TLS.BHL.Infra.App.Domain.DTO.User;
using TLS.BHL.Infra.App.Services;

namespace TLS.BHL.Web.AdminHandlers.RequestHandlers
{
    public class UpdateUserHandler : WebAdminHandlersBase<UpdateUserHandler>, IRequestHandler<UpdateUserInput, UpdateUserOutput>
    {
        private IUserService UserService => GetService<IUserService>();
        public UpdateUserHandler(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        public async Task<UpdateUserOutput> Handle(UpdateUserInput input, CancellationToken cancellationToken)
        {
            var user = Mapper.Map<UpdateUserDTO>(input);
            var res = await UserService.UpdateUser(user, cancellationToken);
            return new UpdateUserOutput
            {
                UserId = res
            };
        }
    }
    public class UpdateUserInput : IRequest<UpdateUserOutput>
    {
        public int? Id { get; set; }
        public string? Name { get; set; }
    }
    public class UpdateUserOutput
    {
        public int? UserId { get; set; }
    }
    public class UpdateUserValidator : AbstractValidator<UpdateUserInput>
    {
        public UpdateUserValidator()
        {
            RuleFor(v => v.Name)
                .MinimumLength(5)
                .MaximumLength(200)
                .NotEmpty();
        }
    }
}
