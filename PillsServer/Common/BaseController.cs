using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace PillsServer.Common
{
    public class BaseController : ControllerBase
    {
        private IMediator mediator;
#pragma warning disable CS8603 // Possible null reference return.
        protected IMediator Mediator => mediator ??= (IMediator)HttpContext.RequestServices.GetService(typeof(IMediator));
#pragma warning restore CS8603 // Possible null reference return.
    }
}
