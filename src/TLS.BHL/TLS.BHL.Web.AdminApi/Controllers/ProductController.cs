using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TLS.BHL.Infra.App.Domain.DTO.Product;
using TLS.BHL.Infra.App.Domain.Entities;
using TLS.BHL.Infra.App.Domain.Models;
using TLS.BHL.Web.AdminHandlers.RequestHandlers;
using TLS.BHL.Web.AdminHandlers.RequestHandlers.Category;
using TLS.BHL.Web.AdminHandlers.RequestHandlers.Product;


namespace TLS.BHL.Web.AdminApi.Controllers
{
    [Route("api/products")]
    [ApiController]
    public class ProductController : WebAdminControllersBase<ProductController>
    {

        [HttpGet]
        public async Task<ApiResponse> List([FromQuery] GetListProductInput search)
        {
            try
            {
                return await Mediator.Send(search, HttpContext.RequestAborted);
            }
            catch (Exception ex)
            {
                throw LogError(ex);
            }
        }

        [HttpGet("all")]
        public async Task<ApiResponse> GetAllProduct()
        {
             try
             {
                 return await Mediator.Send(new GetAllProductInput(), HttpContext.RequestAborted);
             }
             catch (Exception ex)
             {
                 throw LogError(ex);
             }
        }

        [HttpPatch]
        public async Task<ApiResponse> UpdateProductCount([FromBody] List<UpdateProductCountDTO> input)
        {
            try
            {
                return await Mediator.Send(new UpdateProductCountInput { Carts = input}, HttpContext.RequestAborted);
            }
            catch (Exception ex)
            {
                throw LogError(input, ex);
            }
        }
        [HttpDelete("{id}")]
        public async Task<ApiResponse> DeleteProduct([FromRoute] int id)
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
        public async Task<ApiResponse> CreateProduct([FromBody] CreateProductDTO input)
        {
            try
            {
                return await Mediator.Send(new CreateProductInput { createProduct = input}, HttpContext.RequestAborted);
            }
            catch (Exception ex)
            {
                throw LogError(input, ex);
            }
        }
        [HttpPut("{id}")]
        public async Task<ApiResponse> UpdateProduct([FromRoute] int id, [FromBody] UpdateProductDTO input)
        {
            try
            {
                return await Mediator.Send(new UpdateProductInput { ProductId =id, UpdateProduct=input}, HttpContext.RequestAborted);
            }
            catch (Exception ex)
            {
                throw LogError(input, ex);
            }
        }

        [HttpGet("{id}")]
        public async Task<ApiResponse> GetProductById([FromRoute] int id)
        {
            try
            {
                return await Mediator.Send(new GetProductByIdInput(id), HttpContext.RequestAborted);
            }
            catch (Exception ex)
            {
                throw LogError(id, ex);
            }
        }

      
    }
}
