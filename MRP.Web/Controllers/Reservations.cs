using Application.Features.Meetings.Commands;
using Application.Features.Reservations.Command;
using Application.Features.Reservations.Query;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace MRP.Web.Controllers
{
    public class Reservations : Controller
    {
        private IMediator _mediator;
        protected IMediator Mediator => _mediator ??= HttpContext.RequestServices.GetService<IMediator>();

        public Reservations(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var reservations = await Mediator.Send(new GetAllReservationQuery(User.Identity.Name));
            return View(reservations);
        }
        [HttpPost]
        public async Task<IActionResult> UpdateStatus([FromBody] UpdateReservationStatusCommand command)
        {
            var result = await Mediator.Send(command);

            return Json(new
            {
                succeeded = result.Succeeded,
                message = result.Message
            });
        }
        [HttpPost]
        public async Task<IActionResult> DeleteReservation(Guid id)
        {
                var currentUserName = User.Identity.Name;
            var result = await Mediator.Send(new RemoveMeetingCommand(id,currentUserName));

            return Json(new
            {
                succeeded = result.Succeeded,
                message = result.Message
            });
        }
    }
}
