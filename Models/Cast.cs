using System.Text.Json.Serialization;

public record Cast {
    
    public int Id { get; set; }

    // Add explicit foreign key property
    public int PersonId { get; set; }

    [property: JsonPropertyName("person")] 
    public required Person Person { get; set; }

    public Cast() { }
}