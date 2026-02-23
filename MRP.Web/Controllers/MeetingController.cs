using Application.DTOs;
using Application.Features.MeetingRoom.Querie;
using Application.Features.Meetings.Queries;
using Application.Features.Reservations.Command;
using Application.Features.Reservations.Query;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MRP.Web.Controllers
{
    [Authorize]
    public class MeetingController:Controller
    {
        private IMediator _mediator;
        protected IMediator Mediator => _mediator ??= HttpContext.RequestServices.GetService<IMediator>();

        public MeetingController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpGet]
        public async Task<IActionResult> CreateMeeting()
        {
            var model = await Mediator.Send(new GetCreateMeetingRoomQuery(User.Identity.Name));
            return View(model);
        }
        [HttpGet]
        public async Task<IActionResult> GetAllMeetingUser(DateTime start, DateTime end)
        {
            var users = await Mediator.Send(
                new GetAllMeetingUsersQuery(start.Date, start, end));

            return Json(users);
        }
        [HttpGet]
        public async Task<IActionResult> GetRoomReservations(Guid roomId)
        {
            var events = await Mediator.Send(new GetRoomReservationQuery(roomId));
            return Json(events);
        }
        [HttpPost]
        public async Task<IActionResult> CreateReservation([FromBody] CreateReservationDto reservation)
        {
            var response = await Mediator.Send(new CreateReservationCommand(reservation, User.Identity.Name));
            return Ok(response);
        }
        [HttpGet]
        public async Task<IActionResult> PersonMeetingList()
        {
            
            var list = await Mediator.Send(new GetUserPersonMeetingListQuery(User.Identity.Name));
            return View(list);
        }
        [HttpGet]
        public async Task<IActionResult> CreatedMeetingList()
        {
            var list = await Mediator.Send(new GetUserCreaterPersonMeetingListQuery(User.Identity.Name));
            return View(list);
        }
    }
}
