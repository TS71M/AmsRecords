namespace AmsRecords.OpenAI;

public record OpenAiVisionOptions(
    [property: JsonPropertyName("apiKey")] string? ApiKey,
    [property: JsonPropertyName("visionModel")] string VisionModel,
    [property: JsonPropertyName("diagnosticModel")] string DiagnosticModel)
{
    [JsonIgnore]
    public bool IsConfigured => !string.IsNullOrWhiteSpace(ApiKey);
}
