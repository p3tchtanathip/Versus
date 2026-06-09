using Versus.API.Models;
using Versus.API.Services.Interfaces;

namespace Versus.API.Services
{
    public class EloService : IEloService
    {
        private const float KFactor = 32f;
        private const float DefaultRating = 1000f;

        public EloResult Calculate(float winnerRating, float loserRating)
        {
            var winnerRatingBefore = NormalizeRating(winnerRating);
            var loserRatingBefore = NormalizeRating(loserRating);

            var winnerExpected = ExpectedScore(winnerRatingBefore, loserRatingBefore);
            var loserExpected = ExpectedScore(loserRatingBefore, winnerRatingBefore);

            var winnerDelta = KFactor * (1f - winnerExpected);
            var loserDelta = KFactor * (0f - loserExpected);

            return new EloResult(
                winnerRatingBefore,
                winnerRatingBefore + winnerDelta,
                winnerDelta,
                loserRatingBefore,
                loserRatingBefore + loserDelta,
                loserDelta);
        }

        private static float ExpectedScore(float ratingA, float ratingB)
        {
            return 1f / (1f + MathF.Pow(10f, (ratingB - ratingA) / 400f));
        }

        private static float NormalizeRating(float rating)
        {
            return rating <= 0 ? DefaultRating : rating;
        }
    }
}
