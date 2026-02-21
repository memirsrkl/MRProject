using Application.DTOs;
using Application.Interfaces;
using Application.Wrappers;
using Domain.Common.Attributes;
using MediatR;
using Persistence.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.User.Queries
{
    public class LoginUserQuery:IRequest<Response<UserLoginDto>>
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public class LoginUserQueryHandler : IRequestHandler<LoginUserQuery, Response<UserLoginDto>>
        {
            private readonly IApplicationDbContext _context;
            public LoginUserQueryHandler(IApplicationDbContext context)
            {
                _context = context;
            }
            public async Task<Response<UserLoginDto>> Handle(LoginUserQuery request, CancellationToken cancellationToken)
            {
                var hashedPassword = EncryptionHelper.Encrypt(request.Password);
                var user = _context.Users.Where(t => t.UserName == request.UserName && t.Password== hashedPassword).Select(t=> new UserLoginDto
                {
                    UserName=t.UserName,
                    IsAdmin=t.IsAdmin
                }).FirstOrDefault();
                if (user!=null)
                {
                    return new Response<UserLoginDto>(user);
                }
                else
                {
                    return new Response<UserLoginDto>(null);
                }
            }
        }
    }
}
