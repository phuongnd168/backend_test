using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TLS.BHL.Infra.App.Domain.DTO.Product;
using TLS.BHL.Infra.App.Domain.DTO.User;
using TLS.BHL.Infra.App.Domain.Entities;
using TLS.BHL.Infra.App.Repositories;
using TLS.BHL.Infra.App.Services;
using TLS.Core.Service;

namespace TLS.BHL.Infra.Business.Services
{
    public class ProductServie : ServiceBase, IProductService
    {
        private IProductRepository ProductRepo;
        public ProductServie(IServiceProvider serviceProvider, IProductRepository productRepo) : base(serviceProvider)
        {
            ProductRepo = productRepo;
        }

        public async Task<IEnumerable<ProductEntity>> GetListProduct()
        {
            return await ProductRepo.GetListProduct();
        }
        public async Task<string> UpdateProductCount(IList<UpdateProductDTO> products, CancellationToken cancellationToken)
        {
            
       
                return await ProductRepo.UpdateProductCount(products, cancellationToken);

              
      
            

            
        }

       
    }
}

