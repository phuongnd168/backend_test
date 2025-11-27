using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TLS.BHL.Web.AdminHandlers.RequestHandlers;
using TLS.BHL.Web.AdminHandlers.RequestHandlers.Product;

namespace TLS.BHL.Web.AdminApi.Controllers
{
    [Route("api/products")]
    [ApiController]
    public class ProductController : WebAdminControllersBase<ProductController>
    {
        [AllowAnonymous]
        [HttpGet]
        public async Task<GetListProductOutput> List()
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
        [AllowAnonymous]

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
    }
}
