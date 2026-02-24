using Application.Interfaces;
using Application.Wrappers;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.User.Commands
{
    public class ChangeUserStatusCommand:IRequest<Response<bool>>
    {
        public Guid id { get; set; }
        public ChangeUserStatusCommand(Guid id)
        {
            this.id = id;
        }
        public class ChangeUserStatusCommandHandler : IRequestHandler<ChangeUserStatusCommand, Response<bool>>
        {
            private readonly IApplicationDbContext _context;
            public ChangeUserStatusCommandHandler(IApplicationDbContext context)
            {
                _context = context;
            }
            public async Task<Response<bool>> Handle(ChangeUserStatusCommand request, CancellationToken cancellationToken)
            {
                var user = _context.Users.FirstOrDefault(t => t.Id == request.id);
                if (user != null)
                {
                    user.IsAdmin = !user.IsAdmin;
                }
                await _context.SaveChangesAsync(cancellationToken);
                return new Response<bool>(true);
            }
        }
    }
}
