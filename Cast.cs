using System.Text.Json.Serialization;

public record class Cast(
    [property: JsonPropertyName("person")] Person Person
);