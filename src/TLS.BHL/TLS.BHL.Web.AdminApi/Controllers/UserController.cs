using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TLS.BHL.Web.AdminApi.Models;
using TLS.BHL.Web.AdminHandlers.RequestHandlers;
using TLS.BHL.Web.AdminHandlers.RequestHandlers.User;
using TLS.Core.Web.Validation;

namespace TLS.BHL.Web.AdminApi.Controllers
{
    public class filter
    {
        public string? sortField { get; set; }
        public string? sortOrder { get; set; }

        public int? page { get; set; } = 1;

        public int total { get; set; } = 0;

        public int? perPage { get; set; } = 10;

        public int? first {  get; set; } = 0;
        public Dictionary<string, Dictionary<string, string?>>? filters { get; set; }


    }
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : WebAdminControllersBase<UserController>
    {
        [AllowAnonymous]
        [HttpGet]
        public async Task<GetListUserOutput> List([FromQuery] filter test)
        {
            //Request 
            var name = string.Empty;
            try
            {
                return await Mediator.Send(new GetListUserInput { Name = name }, HttpContext.RequestAborted);
            }
            catch (Exception ex)
            {
                throw LogError(new { name }, ex);
            }
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("SearchUser")]
        public async Task<SearchUserOutput> SearchUser([FromQuery] FilterModel model)
        {
            //Request 
            var name = string.Empty;
            if(model != null && model.Filters != null && model.Filters.Any())
            {
                foreach (var filter in model.Filters)
                {
                    if(filter.Value != null && filter.Value.Any())
                    {
                        foreach(var value in filter.Value)
                        {
                            Console.WriteLine($"value's Key: {value.Key},value's Value : {value.Value}");
                        }
                    }
                    Console.WriteLine($"Key: {filter.Key}, Value: {filter.Value}");
                }
            }

            try
            {
                return await Mediator.Send(new SearchUserInput { SortField = model.SortField, Filters = model.Filters, First = model.First, Page = model.Page, PerPage = model.PerPage, SortOrder = model.SortOrder, Total = model.Total }, HttpContext.RequestAborted);
            }
            catch (Exception ex)
            {
                throw LogError(new { SortField = model.SortField, Filters = model.Filters, First = model.First, Page = model.Page, PerPage = model.PerPage, SortOrder = model.SortOrder, Total = model.Total }, ex);
            }
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<UpdateUserOutput> Update(UpdateUserInput input)
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
