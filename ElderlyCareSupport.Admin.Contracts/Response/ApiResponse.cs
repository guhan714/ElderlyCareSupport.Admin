using System.Net;

namespace ElderlyCareSupport.Admin.Contracts.Response;

public record ApiResponse<T>
(
    bool Success,
    HttpStatusCode StatusCode,
    T Data,
    IEnumerable<Error> Errors
    );