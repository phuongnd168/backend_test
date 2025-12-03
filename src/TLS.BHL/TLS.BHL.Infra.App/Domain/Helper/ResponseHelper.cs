using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TLS.BHL.Infra.App.Domain.Models;
using TLS.BHL.Web.AdminApi.Models;

namespace TLS.BHL.Infra.App.Domain.Helper
{
    public static class ResponseHelper
    {
        public static ApiResponse Success(string message, object? data = null)
        {
            return new ApiResponse(200, message, data);
        }

        public static ApiResponse Error(int statusCode, string message)
        {
            return new ApiResponse(statusCode, message);
        }
        public static ApiResponse Created(string message, object? data = null)
        {
            return new ApiResponse(201, message, data);
        }
        public static ApiResponse Updated(string message, object? data = null)
        {
            return new ApiResponse(200, message, data);
        }
    }
}
