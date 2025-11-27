using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TLS.BHL.Web.AdminHandlers.RequestHandlers.Order;

namespace TLS.BHL.Web.AdminApi.Controllers
{
    [Route("api/orderProduct")]
    [ApiController]
    public class OrderProductController : WebAdminControllersBase<OrderProductController>
    {
        [AllowAnonymous]
        [HttpGet]
        public async Task<GetListOrderOutput> List()
        {
            try
            {

                return await Mediator.Send(new GetListOrderInput(), HttpContext.RequestAborted);
            }
            catch (Exception ex)
            {
                throw LogError(ex);
            }
        }
        [AllowAnonymous]
        [HttpPost]
        public async Task<CreateOrderProductOutput> CreateOrderProduct([FromBody] CreateOrderProductInput input)
        {
            try
            {
                return await Mediator.Send(input, HttpContext.RequestAborted);
            }
            catch (Exception ex)
            {
                throw LogError(ex);
            }
        }
    }
}
