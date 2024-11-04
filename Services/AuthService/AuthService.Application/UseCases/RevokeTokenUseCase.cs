using LibraryWebApp.AuthService.Domain.Interfaces;
using System.Security.Claims;

namespace LibraryWebApp.AuthService.Application.UseCases
{
    public class RevokeTokenUseCase
    {
        private readonly IUnitOfWork _unitOfWork;

        public RevokeTokenUseCase(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public void Execute(string refreshToken, ClaimsPrincipal user)
        {
            var username = user.Identity?.Name!;

            var refToken = _unitOfWork.RefreshTokens.Get(t => t.Token == refreshToken && t.User!.Username == username);

            if (refToken == null)
                throw new ArgumentException("Wrong refreshToken or/and username");

            _unitOfWork.RefreshTokens.Delete(refToken);
            _unitOfWork.Save();
        }
    }
}
