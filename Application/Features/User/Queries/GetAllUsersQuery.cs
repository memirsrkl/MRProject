using Application.DTOs;
using Application.Interfaces;
using Application.Wrappers;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.User.Queries
{
    public class GetAllUsersQuery:IRequest<List<UserListDTO>>
    {
        public class GetAllUsersQueryHandler : IRequestHandler<GetAllUsersQuery, List<UserListDTO>>
        {
            private readonly IApplicationDbContext _context;
            public GetAllUsersQueryHandler(IApplicationDbContext context)
            {
                _context = context;
            }
            public async Task<List<UserListDTO>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
            {
                var userList = _context.Users.Select(t => new UserListDTO
                {
                    Email = t.Email,
                    FullName = t.GetFullName(),
                    IsAdmin = t.IsAdmin,
                    Id = t.Id,
                }).ToList();
                return userList;
            }
        }
    }
}
