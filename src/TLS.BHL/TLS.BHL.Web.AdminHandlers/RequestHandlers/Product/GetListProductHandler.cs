using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TLS.BHL.Infra.App.Services;


namespace TLS.BHL.Web.AdminHandlers.RequestHandlers.Product
{
    public class GetListProductHandler : WebAdminHandlersBase<GetListProductHandler>, IRequestHandler<GetListProductInput, GetListProductOutput>
    {
        private IProductService ProductService => GetService<IProductService>();
        public GetListProductHandler(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }
        public async Task<GetListProductOutput> Handle(GetListProductInput request, CancellationToken cancellationToken)
        {

            var products = await ProductService.GetListProduct();
            
            return new GetListProductOutput
            {
                Data = Mapper.Map<IEnumerable<GetListProductItem>>(products)
            };
        }
    }

}
public class GetListProductInput : IRequest<GetListProductOutput>
{
}
public class GetListProductOutput
{
    public IEnumerable<GetListProductItem>? Data { get; set; }
}
public class GetListCategoryId
{
    public int Id { get; set; }
}
public class GetListProductItem
{
    public int Id { get; set; }
    public string NameVi { get; set; }
    public string NameEn { get; set; }
    public string Img { get; set; }
    public double Price { get; set; }
    public int Quantity { get; set; }
    public IEnumerable<GetListCategoryId>? Categories { get; set; } 
}
