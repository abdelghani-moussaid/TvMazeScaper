using System.Text.Json.Serialization;

public record class Person(
    [property: JsonPropertyName("id")] int Id,
    [property: JsonPropertyName("name")] string Name,
    [property: JsonPropertyName("birthday")] DateTime? Birthday
);