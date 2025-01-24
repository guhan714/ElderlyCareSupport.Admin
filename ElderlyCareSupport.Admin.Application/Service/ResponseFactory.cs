using System.Net;
using ElderlyCareSupport.Admin.Application.IService;
using ElderlyCareSupport.Admin.Contracts.Response;

namespace ElderlyCareSupport.Admin.Application.Service;

public class ResponseFactory : IResponseCreator
{
    public ApiResponse<T> CreateResponse<T>(bool isSuccess, HttpStatusCode statusCode, T data,
        IEnumerable<Error>? errors = null) where T: class
    {
        return new ApiResponse<T>
        (
            Success: isSuccess,
            StatusCode: statusCode,
            Data: data,
            Errors: errors ?? Enumerable.Empty<Error>()
        );
    }
}