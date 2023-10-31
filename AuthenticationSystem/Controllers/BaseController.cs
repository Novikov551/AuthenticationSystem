using AuthenticationSystem.Filters;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AuthenticationSystem.Controllers
{
    [ApiController]
    [ErrorHandling]
    public class BaseController : ControllerBase
    {
        private ISender _mediator = null!;

        protected ISender Mediator => _mediator ??= HttpContext.RequestServices.GetRequiredService<ISender>();
    }
}
