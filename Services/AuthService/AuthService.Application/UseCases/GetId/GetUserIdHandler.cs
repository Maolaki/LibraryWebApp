using LibraryWebApp.AuthService.Application.Exceptions;
using LibraryWebApp.AuthService.Domain.Interfaces;
using MediatR;

namespace LibraryWebApp.AuthService.Application.UseCases
{
    public class GetUserIdHandler : IRequestHandler<GetUserIdQuery, long>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetUserIdHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<long> Handle(GetUserIdQuery request, CancellationToken cancellationToken)
        {
            var username = request.ClaimsPrincipalIdentity.Identity?.Name;
            if (username == null)
            {
                throw new NotFoundException("Wrong username");
            }

            var existingUser = await _unitOfWork.Users.GetAsync(u => u.Username == username);
            if (existingUser == null)
            {
                throw new NotFoundException("Wrong username");
            }

            return existingUser.Id;
        }
    }
}
