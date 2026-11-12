using System.Text.Json.Serialization;

namespace ReadingSummary.Api.Models
{
    // Represents a title item in the reading summary application.
    // Require every field in the HTTP response so missing values are not silently replaced with defaults.
    public record TitleItem(
        [property: JsonRequired] Guid Id,
        [property: JsonRequired] string Title,
        [property: JsonRequired] bool IsRead);
}
