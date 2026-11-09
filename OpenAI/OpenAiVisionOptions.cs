namespace AmsRecords.OpenAI;

public record OpenAiVisionOptions(
    [property: JsonPropertyName("apiKey")] string? ApiKey,
    string VisionModel,
    string DiagnosticModel)
{
    string _visionModel = OpenAiModelPolicy.ResolveVision(VisionModel);
    string _diagnosticModel = OpenAiModelPolicy.ResolveDiagnostic(DiagnosticModel);
    string _textModel = OpenAiModelPolicy.Luna;

    [JsonPropertyName("visionModel")]
    public string VisionModel { get => _visionModel; init => _visionModel = OpenAiModelPolicy.ResolveVision(value); }
    [JsonPropertyName("diagnosticModel")]
    public string DiagnosticModel { get => _diagnosticModel; init => _diagnosticModel = OpenAiModelPolicy.ResolveDiagnostic(value); }

    [JsonPropertyName("textModel")]
    public string TextModel { get => _textModel; init => _textModel = OpenAiModelPolicy.Resolve(value); }

    [JsonIgnore]
    public bool IsConfigured => !string.IsNullOrWhiteSpace(ApiKey);
}
