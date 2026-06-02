using Versus.API.Repositories.Interfaces;

namespace Versus.API.Middleware
{
    public class SessionMiddleware(RequestDelegate next)
    {
        public async Task InvokeAsync(HttpContext context, ISessionRepository sessionRepo)
        {
            var sessionIdHeader = context.Request.Headers["X-Session-Id"].ToString();

            if (Guid.TryParse(sessionIdHeader, out var sessionId))
            {
                var exists = await sessionRepo.TouchAsync(sessionId);

                if (exists)
                {
                    context.Items["SessionId"] = sessionId;
                }
            }

            await next(context);
        }
    }
}