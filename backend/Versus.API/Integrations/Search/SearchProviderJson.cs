using System.Text.Json;
using System.Text.Json.Serialization;

namespace Versus.API.Integrations.Search
{
    internal static class SearchProviderJson
    {
        public static readonly JsonSerializerOptions Options = new(JsonSerializerDefaults.Web)
        {
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
        };

        public static string SerializeMetadata(object metadata)
        {
            return JsonSerializer.Serialize(metadata, Options);
        }
    }
}
