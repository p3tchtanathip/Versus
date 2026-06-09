using Versus.API.Models;
using Versus.API.Services.Interfaces;

namespace Versus.API.Services
{
    public class PairingService : IPairingService
    {
        private const float RatingDiffWeight = 1.0f;
        private const float BattleCountWeight = 0.1f;
        private const float RandomWeight = 0.05f;

        public (Item ItemA, Item ItemB) SelectPair(
            IReadOnlyList<Item> items,
            IReadOnlySet<(Guid, Guid)> sessionPairHistory)
        {
            var unseenPairs = GetCandidatePairs(items, sessionPairHistory, unseenOnly: true);
            var candidates = unseenPairs.Count > 0
                ? unseenPairs
                : GetCandidatePairs(items, sessionPairHistory, unseenOnly: false);

            var best = candidates.MinBy(c => c.Score);
            return (best!.ItemA, best!.ItemB);
        }

        private static List<PairCandidate> GetCandidatePairs(
            IReadOnlyList<Item> items,
            IReadOnlySet<(Guid, Guid)> sessionPairHistory,
            bool unseenOnly)
        {
            var candidates = new List<PairCandidate>();

            for (var i = 0; i < items.Count; i++)
            {
                for (var j = i + 1; j < items.Count; j++)
                {
                    var itemA = items[i];
                    var itemB = items[j];
                    var pairKey = NormalizePairKey(itemA.Id, itemB.Id);

                    if (unseenOnly && sessionPairHistory.Contains(pairKey))
                    {
                        continue;
                    }

                    candidates.Add(new PairCandidate(
                        itemA,
                        itemB,
                        CalculateScore(itemA, itemB, sessionPairHistory.Contains(pairKey))));
                }
            }

            return candidates;
        }

        private static float CalculateScore(Item itemA, Item itemB, bool alreadyPlayed)
        {
            var ratingDiff = RatingDiff(itemA, itemB);
            var battleCount = BattleCount(itemA, itemB);
            var pairHistoryPenalty = PairHistory(alreadyPlayed);
            var randomWeight = RandomWeightFactor();

            return RatingDiffWeight * ratingDiff
                + BattleCountWeight * battleCount
                + pairHistoryPenalty
                + RandomWeight * randomWeight;
        }

        private static float RatingDiff(Item itemA, Item itemB)
        {
            return MathF.Abs(itemA.EloRating - itemB.EloRating);
        }

        private static float BattleCount(Item itemA, Item itemB)
        {
            return Math.Max(itemA.MatchCount, itemB.MatchCount);
        }

        private static float PairHistory(bool alreadyPlayed)
        {
            return alreadyPlayed ? 1000f : 0f;
        }

        private static float RandomWeightFactor()
        {
            return Random.Shared.NextSingle();
        }

        private static (Guid, Guid) NormalizePairKey(Guid idA, Guid idB)
        {
            return idA.CompareTo(idB) < 0 ? (idA, idB) : (idB, idA);
        }

        private sealed record PairCandidate(Item ItemA, Item ItemB, float Score);
    }
}
