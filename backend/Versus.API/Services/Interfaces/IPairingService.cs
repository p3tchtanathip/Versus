using Versus.API.Models;

namespace Versus.API.Services.Interfaces
{
    public interface IPairingService
    {
        (Item ItemA, Item ItemB) SelectPair(
            IReadOnlyList<Item> items,
            IReadOnlySet<(Guid, Guid)> sessionPairHistory);
    }
}
