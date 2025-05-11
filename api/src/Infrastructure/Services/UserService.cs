using System.Security.Claims;
using Application.Common.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services
{
    public class UserService : IUser
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IApplicationDbContext _dbContext;

        public UserService(IHttpContextAccessor httpContextAccessor, IApplicationDbContext dbContext)
        {
            _httpContextAccessor = httpContextAccessor;
            _dbContext = dbContext;
        }

        public string? ClerkUserId
        {
            get
            {
                return _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value ??
                       _httpContextAccessor.HttpContext?.User?.FindFirst("sub")?.Value;
            }
        }

        public int? Id
        {
            get
            {
                var clerkId = ClerkUserId;
                if (string.IsNullOrEmpty(clerkId)) return null;
                var user = _dbContext.Users.FirstOrDefault(u => u.ClerkUserId == clerkId);
                return user?.Id;
            }
        }

        public async Task<int?> IdAsync()
        {
            var clerkId = ClerkUserId;
            if (string.IsNullOrEmpty(clerkId)) return null;
            var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.ClerkUserId == clerkId);
            return user?.Id;
        }


        public string? Email
        {
            get
            {
                return _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Email)?.Value;
            }
        }

        public bool IsInRole(string roleName)
        {
            return _httpContextAccessor.HttpContext?.User?.IsInRole(roleName) ?? false;
        }
    }
}
