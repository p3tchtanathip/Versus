namespace Versus.API.Models
{
    public record EloResult(
        int WinnerRatingBefore,
        int WinnerRatingAfter,
        int WinnerDelta,
        int LoserRatingBefore,
        int LoserRatingAfter,
        int LoserDelta);
}
