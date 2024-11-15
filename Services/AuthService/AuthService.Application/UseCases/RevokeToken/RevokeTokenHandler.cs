using LibraryWebApp.AuthService.Domain.Interfaces;
using MediatR;

namespace LibraryWebApp.AuthService.Application.UseCases
{
    public class RevokeTokenHandler : IRequestHandler<RevokeTokenCommand, Unit>
    {
        private readonly IUnitOfWork _unitOfWork;

        public RevokeTokenHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Unit> Handle(RevokeTokenCommand request, CancellationToken cancellationToken)
        {
            var username = request.User.Identity?.Name!;

            var refToken = await _unitOfWork.RefreshTokens.GetAsync(t => t.Token == request.RefreshToken && t.User!.Username == username);

            if (refToken == null)
                throw new ArgumentException("Wrong refreshToken or/and username");

            _unitOfWork.RefreshTokens.Delete(refToken);
            await _unitOfWork.SaveAsync();

            return Unit.Value;
        }
    }
}
