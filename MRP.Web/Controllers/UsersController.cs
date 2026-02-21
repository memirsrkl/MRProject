using Application.Features.User.Commands;
using Application.Features.User.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace MRP.Web.Controllers
{
    public class UsersController:Controller
    {
        private IMediator _mediator;
        protected IMediator Mediator => _mediator ??= HttpContext.RequestServices.GetService<IMediator>();
        public async Task<IActionResult> Index()
        {
            var allUser = await Mediator.Send(new GetAllUsersQuery());
            return View(allUser);
        }
        [HttpPost]
        public async Task<IActionResult> ChangeStatuse(Guid id)
        {
            var changeStatus = await Mediator.Send(new ChangeUserStatusCommand(id));
            return Ok(changeStatus);
        }
    }
}
