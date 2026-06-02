using Microsoft.EntityFrameworkCore;
using Versus.API.Data;
using Versus.API.Models;
using Versus.API.Repositories.Interfaces;

namespace Versus.API.Repositories
{
    public class SessionRepository(AppDbContext db) : ISessionRepository
    {
        public async Task<Guid> CreateAsync(Session session)
        {
            db.Sessions.Add(session);
            await db.SaveChangesAsync();
            return session.Id;
        }

        public async Task<bool> TouchAsync(Guid sessionId)
        {
            var affectedRows = await db.Sessions
                .Where(x => x.Id == sessionId)
                .ExecuteUpdateAsync(setters =>
                    setters.SetProperty(
                        x => x.LastSeenAt,
                        DateTime.UtcNow));

            return affectedRows > 0;
        }
    }
}