using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TLS.BHL.Web.AdminHandlers.RequestHandlers.Category;
using TLS.BHL.Web.AdminHandlers.RequestHandlers.User;

namespace TLS.BHL.Web.AdminApi.Controllers
{
    [Route("api/categories")]
    [ApiController]
    public class CategoryController : WebAdminControllersBase<CategoryController>
    {
        [AllowAnonymous]
        [HttpGet]
        public async Task<GetListCategoryOutput> List()
        {
            try
            {
             
                return await Mediator.Send(new GetListCategoryInput(), HttpContext.RequestAborted);
            }
            catch (Exception ex)
            {
                throw LogError(ex);
            }
        }

    }
}
