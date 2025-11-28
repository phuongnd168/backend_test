using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TLS.BHL.Infra.App.Services;

namespace TLS.BHL.Web.AdminHandlers.RequestHandlers.Product
{
    public class DeleteProductHandler : WebAdminHandlersBase<DeleteProductHandler>, IRequestHandler<DeleteProductInput, DeleteProductOutput>
    {
        private IProductService ProductService => GetService<IProductService>();
        public DeleteProductHandler(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }
        public async Task<DeleteProductOutput> Handle(DeleteProductInput request, CancellationToken cancellationToken)
        {

            var result = await ProductService.DeleteProduct(request.id, cancellationToken);

            return new DeleteProductOutput
            {
                Data = result
            };
        }
    }

}
public record DeleteProductInput(int id) : IRequest<DeleteProductOutput>;

public class DeleteProductOutput
{
    public string Data { get; set; }
}

