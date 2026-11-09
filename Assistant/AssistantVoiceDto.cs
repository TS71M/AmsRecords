namespace AmsRecords.Assistant;

// Transcription is input only. It never executes an assistant instruction.
public sealed record AssistantAudioDto(string Base64, string ContentType);
public sealed record AssistantTranscriptDto(string Text);

public static class AssistantVoicePolicy
{
    public const int MaximumBytes = 4 * 1024 * 1024;
    public const int MaximumJsonRequestBytes = ((MaximumBytes + 2) / 3) * 4 + 8192;
    public const int MaximumCaptureSeconds = 90;
    public const int MaximumTranscriptCharacters = 2000;

    public static bool TryDecode(AssistantAudioDto? input, out byte[] audio, out string extension, out string contentType)
    {
        audio = []; extension = ""; contentType = "";
        if (input is null || string.IsNullOrWhiteSpace(input.Base64)
            || input.Base64.Length > ((MaximumBytes + 2) / 3) * 4) return false;
        contentType = input.ContentType?.Split(';')[0].Trim().ToLowerInvariant() ?? "";
        if (contentType is not ("audio/webm" or "audio/mp4" or "audio/wav" or "audio/x-wav")) return false;
        try { audio = Convert.FromBase64String(input.Base64); }
        catch (FormatException) { return false; }
        if (audio.Length is < 16 or > MaximumBytes) return false;
        extension = contentType switch {
            "audio/webm" when audio.AsSpan(0, 4).SequenceEqual(new byte[] { 0x1a, 0x45, 0xdf, 0xa3 }) => "webm",
            "audio/mp4" when audio.AsSpan(4, 4).SequenceEqual("ftyp"u8) => "m4a",
            "audio/wav" or "audio/x-wav" when audio.Length >= 44 && audio.AsSpan(0, 4).SequenceEqual("RIFF"u8)
                && audio.AsSpan(8, 4).SequenceEqual("WAVE"u8) => "wav",
            _ => ""
        };
        return extension.Length > 0;
    }
}
