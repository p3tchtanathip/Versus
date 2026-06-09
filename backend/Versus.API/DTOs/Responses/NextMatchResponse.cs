namespace Versus.API.DTOs.Responses
{
    public class NextMatchResponse
    {
        public MatchPairDto? Match { get; set; }
        public ProgressDto Progress { get; set; } = new();
    }

    public class MatchPairDto
    {
        public ItemResponse ItemA { get; set; } = null!;
        public ItemResponse ItemB { get; set; } = null!;
    }

    public class ProgressDto
    {
        public int Played { get; set; }
        public int Total { get; set; }
    }
}
