using System.Text.Json.Serialization;
public record class Show(
    [property: JsonPropertyName("id")] int Id,
    [property: JsonPropertyName("name")] string Name
);