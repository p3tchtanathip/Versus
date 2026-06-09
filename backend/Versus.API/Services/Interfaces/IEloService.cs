using Versus.API.Models;

namespace Versus.API.Services.Interfaces
{
    public interface IEloService
    {
        EloResult Calculate(float winnerRating, float loserRating);
    }
}