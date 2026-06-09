namespace Versus.API.Models
{
    public record EloResult(
        float WinnerRatingBefore,
        float WinnerRatingAfter,
        float WinnerDelta,
        float LoserRatingBefore,
        float LoserRatingAfter,
        float LoserDelta);
}
