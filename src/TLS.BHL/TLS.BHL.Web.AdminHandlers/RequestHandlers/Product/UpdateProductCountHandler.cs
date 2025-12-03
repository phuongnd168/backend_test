using AutoMapper;
using Azure.Core;
using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TLS.BHL.Infra.App.Domain.DTO.Product;
using TLS.BHL.Infra.App.Domain.DTO.User;
using TLS.BHL.Infra.App.Domain.Entities;
using TLS.BHL.Infra.App.Domain.Models;
using TLS.BHL.Infra.App.Services;

namespace TLS.BHL.Web.AdminHandlers.RequestHandlers.Product
{
    public class UpdateProductCountHandler : WebAdminHandlersBase<UpdateProductCountHandler>, IRequestHandler<UpdateProductCountInput, ApiResponse>
    {
     

        private IProductService ProductService => GetService<IProductService>();
        public UpdateProductCountHandler(IServiceProvider serviceProvider) : base(serviceProvider)
        {

        }

        public async Task<ApiResponse> Handle(UpdateProductCountInput input, CancellationToken cancellationToken)
        {
         
            return await ProductService.UpdateProductCount(input.Carts, cancellationToken);

        }
    }
    public class UpdateProductCountInput : IRequest<ApiResponse>
    {
        public List<UpdateProductCountDTO> Carts { get; set; }
        

    }
  
 



}
