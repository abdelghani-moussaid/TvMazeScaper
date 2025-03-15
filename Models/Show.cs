using System.Text.Json.Serialization;
public record Show
{
    [property: JsonPropertyName("id")] 
    public int Id { get; set; }

    [property: JsonPropertyName("name")] 
    public required string Name { get; set; }

    public Show() {}
}