namespace Versus.API.Context
{
    public class CurrentSession(IHttpContextAccessor accessor) : ICurrentSession
    {
        public Guid? SessionId => accessor.HttpContext?.Items["SessionId"] as Guid?;
    }
}