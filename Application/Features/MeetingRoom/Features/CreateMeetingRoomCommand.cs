using Application.Interfaces;
using Application.Wrappers;
using Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.MeetingRoom.Features
{
    public class CreateMeetingRoomCommand : IRequest<Response<bool>>
    {
        public string Description { get; set; }
        public string Location { get; set; }
        public int Capacity { get; set; }
        public class CreateMeetingRoomCommandHandler : IRequestHandler<CreateMeetingRoomCommand, Response<bool>>
        {
            private readonly IApplicationDbContext _context;
            public CreateMeetingRoomCommandHandler(IApplicationDbContext context)
            {
                _context = context;
            }
            public Task<Response<bool>> Handle(CreateMeetingRoomCommand request, CancellationToken cancellationToken)
            {
                MeetingRooms meetingRoom = new MeetingRooms
                {
                    Description = request.Description,
                    Location = request.Location,
                    Capacity = request.Capacity
                };
                _context.MeetingRooms.Add(meetingRoom);
                _context.SaveChangesAsync(cancellationToken);
                return Task.FromResult(new Response<bool>(true));
            }
        }
    }
}
