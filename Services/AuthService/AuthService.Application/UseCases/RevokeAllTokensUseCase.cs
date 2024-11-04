using LibraryWebApp.AuthService.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace LibraryWebApp.AuthService.Application.UseCases
{
    public class RevokeAllTokensUseCase
    {
        private readonly IUnitOfWork _unitOfWork;

        public RevokeAllTokensUseCase(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public void Execute(ClaimsPrincipal user)
        {
            var username = user.Identity?.Name!;

            var refreshTokens = _unitOfWork.RefreshTokens.GetAll()
                .Include(t => t.User)
                .Where(t => t.User != null && t.User.Username == username)
                .ToList();

            if (!refreshTokens.Any())
                throw new ArgumentException("Wrong username");

            foreach (var refreshToken in refreshTokens)
                _unitOfWork.RefreshTokens.Delete(refreshToken);

            _unitOfWork.Save();
        }
    }
}
