using TLS.BHL.Web.AdminApi.Models;

namespace TLS.BHL.Web.AdminApi.Helper
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
