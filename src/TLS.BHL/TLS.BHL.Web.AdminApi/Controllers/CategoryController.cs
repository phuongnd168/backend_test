using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TLS.BHL.Infra.App.Domain.DTO.Category;
using TLS.BHL.Infra.App.Domain.Models;
using TLS.BHL.Web.AdminHandlers.RequestHandlers.Category;
using TLS.BHL.Web.AdminHandlers.RequestHandlers.User;
using static TLS.BHL.Web.AdminHandlers.RequestHandlers.Category.DeleteCategoryHandler;

namespace TLS.BHL.Web.AdminApi.Controllers
{
    [Route("api/categories")]
    [ApiController]
    public class CategoryController : WebAdminControllersBase<CategoryController>
    {

        [HttpGet]
        public async Task<ApiResponse> List()
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


        [HttpPost]
        public async Task<ApiResponse> CreateCategory([FromBody] CreateCategoryDTO input)
        {
            try
            {
                return await Mediator.Send(new CreateCategoryInput { createCategory = input }, HttpContext.RequestAborted);
            }
            catch (Exception ex)
            {
                throw LogError(ex);
            }
        }

        [HttpDelete("{id}")]
        public async Task<ApiResponse> DeleteCategory([FromRoute] int id)
        {
            try
            {
                return await Mediator.Send(new DeleteCategoryInput(id), HttpContext.RequestAborted);
            }
            catch (Exception ex)
            {
                throw LogError(ex);
            }
        }
        [HttpPut("{id}")]
        public async Task<ApiResponse> UpdateCategory([FromRoute] int id, [FromBody] UpdateCategoryDTO input)
        {
            try
            {
                return await Mediator.Send(new UpdateCategoryInput { category= input, CategoryId =id}, HttpContext.RequestAborted);
            }
            catch (Exception ex)
            {
                throw LogError(ex);
            }
        }
        [HttpGet("{id}")]
        public async Task<ApiResponse> GetCategoryById([FromRoute] int id)
        {
            try
            {
                return await Mediator.Send(new GetCategoryByIdInput(id), HttpContext.RequestAborted);
            }
            catch (Exception ex)
            {
                throw LogError(ex);
            }
        }
    }
}
