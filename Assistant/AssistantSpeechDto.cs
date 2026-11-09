namespace AmsRecords.Assistant;

public sealed record AssistantSpeechRequestDto(string Text, string? Culture = null);
public sealed record AssistantSpeechDto(string Base64, string ContentType);

public static class AssistantSpeechPolicy
{
    public const int MaximumCharacters = 2000;
    public const int MaximumAudioBytes = 8 * 1024 * 1024;
    public const int MaximumRequestBytes = 32768;

    public static bool TryDecode(AssistantSpeechDto? input, out byte[] wave)
    {
        wave = [];
        if (input?.ContentType != "audio/wav" || string.IsNullOrWhiteSpace(input.Base64)
            || input.Base64.Length > ((MaximumAudioBytes + 46) / 3) * 4) return false;
        try { wave = Convert.FromBase64String(input.Base64); }
        catch (FormatException) { return false; }
        if (wave.Length is < 46 or > MaximumAudioBytes + 44) return false;
        var bytes = wave.AsSpan();
        static int Int(ReadOnlySpan<byte> bytes, int at) => System.Buffers.Binary.BinaryPrimitives.ReadInt32LittleEndian(bytes[at..]);
        static short Short(ReadOnlySpan<byte> bytes, int at) => System.Buffers.Binary.BinaryPrimitives.ReadInt16LittleEndian(bytes[at..]);
        return bytes[..4].SequenceEqual("RIFF"u8) && bytes.Slice(8, 8).SequenceEqual("WAVEfmt "u8)
            && Int(bytes, 4) == bytes.Length - 8 && Int(bytes, 16) == 16 && Short(bytes, 20) == 1 && Short(bytes, 22) == 1
            && Int(bytes, 24) == 24000 && Int(bytes, 28) == 48000 && Short(bytes, 32) == 2 && Short(bytes, 34) == 16
            && bytes.Slice(36, 4).SequenceEqual("data"u8) && Int(bytes, 40) == bytes.Length - 44 && (bytes.Length - 44) % 2 == 0;
    }
}
