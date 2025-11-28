using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TLS.BHL.Infra.App.Domain.DTO.Product;
using TLS.BHL.Web.AdminHandlers.RequestHandlers;
using TLS.BHL.Web.AdminHandlers.RequestHandlers.Product;

namespace TLS.BHL.Web.AdminApi.Controllers
{
    [Route("api/products")]
    [ApiController]
    public class ProductController : WebAdminControllersBase<ProductController>
    {
       
        [HttpGet]
        public async Task<List<GetListProductItemDto>> List()
        {
            try
            {
                 return await Mediator.Send(new GetListProductInput(), HttpContext.RequestAborted);
            }
            catch (Exception ex)
            {
                throw LogError(ex);
            }
        }

        [HttpPatch]
       

        public async Task<UpdateProductCountOutput> UpdateProductCount([FromBody] UpdateProductCountInput input)
        {
            try
            {
                
                return await Mediator.Send(input, HttpContext.RequestAborted);
            }
            catch (Exception ex)
            {
                throw LogError(input, ex);
            }
        }
        [HttpDelete("{id}")]

        public async Task<DeleteProductOutput> DeleteProduct([FromRoute] int id)
        {
            try
            {

                return await Mediator.Send(new DeleteProductInput(id), HttpContext.RequestAborted);
            }
            catch (Exception ex)
            {
                throw LogError(id, ex);
            }
        }

        [HttpPost]
       

        public async Task<CreateProductOutput> CreateProduct([FromBody] CreateProductInput input)
        {
            try
            {

                return await Mediator.Send(input, HttpContext.RequestAborted);
            }
            catch (Exception ex)
            {
                throw LogError(input, ex);
            }
        }
    }
}
