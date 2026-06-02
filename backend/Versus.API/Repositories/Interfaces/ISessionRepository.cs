using Versus.API.Models;

namespace Versus.API.Repositories.Interfaces
{
    public interface ISessionRepository
    {
        Task<Guid> CreateAsync(Session session);
        Task<bool> TouchAsync(Guid sessionId);
    }
}