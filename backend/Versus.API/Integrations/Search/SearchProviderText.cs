namespace Versus.API.Integrations.Search
{
    internal static class SearchProviderText
    {
        public static string? FirstNotEmpty(params string?[] values)
        {
            return values.FirstOrDefault(value => !string.IsNullOrWhiteSpace(value));
        }
    }
}
