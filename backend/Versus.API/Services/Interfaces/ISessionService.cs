namespace Versus.API.Services.Interfaces
{
    public interface ISessionService
    {
        Task<Guid> CreateAsync();
    }
}