using MediatR;
using Microsoft.Extensions.Logging.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TLS.BHL.Infra.App.Domain.DTO.Product;
using TLS.BHL.Infra.App.Domain.Entities;
using TLS.BHL.Infra.App.Domain.Models;
using TLS.BHL.Infra.App.Services;


namespace TLS.BHL.Web.AdminHandlers.RequestHandlers.Product
{
    public record GetProductByIdInput(int id) : IRequest<ApiResponse>
    {
    }
    public class GetProductByIdHandler
     : WebAdminHandlersBase<GetProductByIdHandler>,
       IRequestHandler<GetProductByIdInput, ApiResponse>
    {
        private IProductService ProductService => GetService<IProductService>();

        public GetProductByIdHandler(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        public async Task<ApiResponse> Handle(GetProductByIdInput request, CancellationToken cancellationToken)
        {
            return await ProductService.GetProductById(request.id);
         
        }

    
    }

}
