using Application.Interfaces;
using Application.Wrappers;
using MediatR;
using Persistence.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.User.Commands
{
    public class AddUserCommand : IRequest<Response<bool>>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string RePassword { get; set; }
        public class AddUserCommandHandler : IRequestHandler<AddUserCommand, Response<bool>>
        {
            private readonly IApplicationDbContext _context;
            public AddUserCommandHandler(IApplicationDbContext context)
            {
                _context = context;
            }
            public async Task<Response<bool>> Handle(AddUserCommand request, CancellationToken cancellationToken)
            {
                if (request.Password != request.RePassword)
                    return new Response<bool>( "Şifreler uyuşmuyor");
                var userMailAny = _context.Users.Any(t => t.Email == request.Email);
                if(userMailAny)
                    return new Response<bool>( "Bu mail başka bir kullanıcı tarafından kullanılıyor.");
                var userNameAny = _context.Users.Any(t => t.UserName == request.UserName);
                if(userNameAny)
                    return new Response<bool>( "Bu kullanıcı adı başka bir kullanıcı kullanıyor");
                var user = new Domain.Entities.Users
                {
                    FirstName = request.FirstName,
                    LastName = request.LastName,
                    UserName = request.UserName,
                    Email = request.Email,
                    Password = EncryptionHelper.Encrypt(request.Password),
                    IsAdmin = false
                };
                _context.Users.Add(user);
                await _context.SaveChangesAsync(cancellationToken);
                return new Response<bool>(true);
            }
        }
    }
}
