using System.Text.Json.Serialization;

public record Person 
{
    [property: JsonPropertyName("id")] 
    public int Id { get; set; }

    [property: JsonPropertyName("name")] 
    public required string Name { get; set; }

    [property: JsonPropertyName("birthday")] 
    public DateTime? Birthday { get; set; }

    public Person() { }

}