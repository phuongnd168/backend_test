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
using TLS.BHL.Infra.App.Services;

namespace TLS.BHL.Web.AdminHandlers.RequestHandlers.Product
{
    public class UpdateProductCountHandler : WebAdminHandlersBase<UpdateProductCountHandler>, IRequestHandler<UpdateProductCountInput, UpdateProductCountOutput>
    {
     

        private IProductService ProductService => GetService<IProductService>();
        public UpdateProductCountHandler(IServiceProvider serviceProvider) : base(serviceProvider)
        {
           

        }

        public async Task<UpdateProductCountOutput> Handle(UpdateProductCountInput input, CancellationToken cancellationToken)
        {
         
            var res = await ProductService.UpdateProductCount(input.Carts, cancellationToken);
            return  new UpdateProductCountOutput
            {
                Data = res
            };
        }
    }
    public class UpdateProductCountInput : IRequest<UpdateProductCountOutput>
    {
        public IList<UpdateProductDTO> Carts { get; set; }
        

    }
  
    public class UpdateProductCountOutput
    {
        public string Data { get; set; }
    }



}
