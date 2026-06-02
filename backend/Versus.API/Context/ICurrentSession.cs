namespace Versus.API.Context
{
    public interface ICurrentSession
    {
        Guid? SessionId { get; }
    }
}