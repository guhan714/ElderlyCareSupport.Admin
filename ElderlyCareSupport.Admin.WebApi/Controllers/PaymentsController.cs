using Asp.Versioning;
using ElderlyCareSupport.Admin.WebApi.Abstractions;
using ElderlyCareSupport.Admin.WebApi.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ElderlyCareSupport.Admin.WebApi.Controllers;

[ApiController]
[ApiVersion(1)]
[Route("admin/v{v:apiVersion}/[controller]")]
[Produces("application/json")]
[Authorize(Roles = "Admin")]
[MacFilter]
public class PaymentsController : BaseController
{
    
}