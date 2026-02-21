using Application.Features.MeetingRoom.Features;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Reflection.Metadata.Ecma335;

namespace MRP.Web.Controllers
{
    [Authorize(Roles ="Admin")]
    public class AdminController:Controller
    {
        private IMediator _mediator;
        protected IMediator Mediator => _mediator ??= HttpContext.RequestServices.GetService<IMediator>();
        [HttpGet]
        public IActionResult AddMeetingRoom()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddMeetingRoom(CreateMeetingRoomCommand command)
        {
            var result = await Mediator.Send(command);

            return Json(result);
        }
    }
}
