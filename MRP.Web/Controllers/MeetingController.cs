using Application.DTOs;
using Application.Features.MeetingRoom.Querie;
using Application.Features.Meetings.Commands;
using Application.Features.Meetings.Queries;
using Application.Features.Reservations.Command;
using Application.Features.Reservations.Query;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography.X509Certificates;

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
        public async Task<IActionResult> GetAllMeetingUser(Guid? id,DateTime start, DateTime end)
        {
            var users = await Mediator.Send(
                new GetAllMeetingUsersQuery(id,start.Date, start, end));

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
        [HttpPost]
        public async Task<IActionResult> AddParticipant([FromBody] MettingAddParticipant model)
        {
            var response = await Mediator.Send(new AddParticipantCommand(model));
            return Ok(response);
        }
        [HttpPost]
        public async Task<IActionResult> RemoveParticipant([FromBody] MettingAddParticipant model)
        {
            var currentUser = User.Identity.Name;
            var response = await Mediator.Send(new RemoveParticipantCommand(model, currentUser));
            return Ok(response);
        }
    }
}
