using Azure.Core;
using Microsoft.AspNetCore.Http;
using System.Net;

namespace OrderServiceAPI.Models.response
{
    public class ApiResponse<T>
    {
        public bool Success { get; set; }
        public HttpStatusCode StatusCode { get; set; }
        public T Data { get; set; }

        public ApiResponse(bool success, HttpStatusCode  statusCode, T data)
        {
            Success = success;
            StatusCode = statusCode;
            Data = data;
        }
    }
}