using Versus.API.Models;
using Versus.API.Repositories.Interfaces;
using Versus.API.Services.Interfaces;

namespace Versus.API.Services
{
    public class SessionService(ISessionRepository repo) : ISessionService
    {
        public Task<Guid> CreateAsync()
        {
            var session = new Session
            {
                Id = Guid.NewGuid(),
                CreatedAt = DateTime.UtcNow,
                LastSeenAt = DateTime.UtcNow,
            };

            return repo.CreateAsync(session);
        }
    }
}