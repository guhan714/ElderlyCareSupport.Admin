using System.Net;
using ElderlyCareSupport.Admin.Contracts.Response;

namespace ElderlyCareSupport.Admin.Application.IService;

public interface IResponseCreator
{
    ApiResponse<T> CreateResponse<T>(bool isSuccess, HttpStatusCode statusCode, T data, IEnumerable<Error> errors) where T: class;
}