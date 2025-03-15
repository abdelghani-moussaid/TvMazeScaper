using System.Text.Json.Serialization;

public record Cast {
    
    public int Id { get; set; }

    // Foreign key property for Show
    public int ShowId { get; set; }

    // Navigation property for Show
    [JsonIgnore]
    public Show Show { get; set; } = null!;
    
    // Add explicit foreign key property
    public int PersonId { get; set; }

    [property: JsonPropertyName("person")] 
    public required Person Person { get; set; }

    public Cast() { }
}