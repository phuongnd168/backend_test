using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TLS.BHL.Infra.App.Domain.DTO.Order;
using TLS.BHL.Infra.App.Domain.Models;
using TLS.BHL.Web.AdminHandlers.RequestHandlers.Order;

namespace TLS.BHL.Web.AdminApi.Controllers
{
    [Route("api/orderProduct")]
    [ApiController]
    public class OrderProductController : WebAdminControllersBase<OrderProductController>
    {
 
        [HttpGet]
        public async Task<ApiResponse> List()
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

        [HttpPost]
        public async Task<ApiResponse> CreateOrderProduct([FromBody] CreateOrderProductDTO input)
        {
            try
            {
                return await Mediator.Send(new CreateOrderProductInput { createOrder = input}, HttpContext.RequestAborted);
            }
            catch (Exception ex)
            {
                throw LogError(ex);
            }
        }
    }
}
