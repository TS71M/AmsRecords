using static AmsRecords.Irrigation.IrrigationDigitalTwinDtos;

namespace AmsRecords.Irrigation;

public static class CatchCanCalibrationScopes
{
    public const string Area = "AREA";
    public const string HeadNozzle = "HEAD_NOZZLE";
    public const string Pressure = "PRESSURE";
}

public static class CatchCanCalibrationDtos
{
    public sealed record CatchCanMeasurementSaveDto(
        [property: JsonPropertyName("x")] double X,
        [property: JsonPropertyName("y")] double Y,
        [property: JsonPropertyName("collectedMl")] decimal? CollectedMl,
        [property: JsonPropertyName("containerAreaCm2")] decimal? ContainerAreaCm2,
        [property: JsonPropertyName("appliedMm")] decimal? AppliedMm);

    public sealed record CatchCanTestSaveDto(
        [property: JsonPropertyName("areaPubId")] Guid AreaPubId,
        [property: JsonPropertyName("testDateUtc")] DateTime TestDateUtc,
        [property: JsonPropertyName("runtimeSeconds")] int RuntimeSeconds,
        [property: JsonPropertyName("weatherNotes")][param: MaxLength(2000)] string? WeatherNotes,
        [property: JsonPropertyName("windSpeedMps")] decimal? WindSpeedMps,
        [property: JsonPropertyName("windDirectionDegrees")] decimal? WindDirectionDegrees,
        [property: JsonPropertyName("measurements")] IReadOnlyList<CatchCanMeasurementSaveDto> Measurements);

    public sealed record CatchCanDistributionMetricsDto(
        [property: JsonPropertyName("measurementCount")] int MeasurementCount,
        [property: JsonPropertyName("meanMm")] double MeanMm,
        [property: JsonPropertyName("distributionUniformityLowQuarter")] double? DistributionUniformityLowQuarter,
        [property: JsonPropertyName("christiansenUniformityCoefficient")] double? ChristiansenUniformityCoefficient,
        [property: JsonPropertyName("minimumMm")] double MinimumMm,
        [property: JsonPropertyName("maximumMm")] double MaximumMm,
        [property: JsonPropertyName("coefficientOfVariation")] double? CoefficientOfVariation);

    public sealed record CatchCanModelErrorMetricsDto(
        [property: JsonPropertyName("comparedPointCount")] int ComparedPointCount,
        [property: JsonPropertyName("meanErrorMm")] double MeanErrorMm,
        [property: JsonPropertyName("meanAbsoluteErrorMm")] double MeanAbsoluteErrorMm,
        [property: JsonPropertyName("rootMeanSquareErrorMm")] double RootMeanSquareErrorMm,
        [property: JsonPropertyName("meanAbsolutePercentageError")] double? MeanAbsolutePercentageError);

    public sealed record IrrigationModelCalibrationSummaryDto(
        [property: JsonPropertyName("status")] string Status,
        [property: JsonPropertyName("calibrationTestCount")] int CalibrationTestCount,
        [property: JsonPropertyName("applicationDepthMultiplier")] double ApplicationDepthMultiplier,
        [property: JsonPropertyName("scopeCode")] string ScopeCode);

    public sealed record CatchCanMeasurementComparisonDto(
        [property: JsonPropertyName("pubId")] Guid PubId,
        [property: JsonPropertyName("x")] double X,
        [property: JsonPropertyName("y")] double Y,
        [property: JsonPropertyName("collectedMl")] decimal? CollectedMl,
        [property: JsonPropertyName("containerAreaCm2")] decimal? ContainerAreaCm2,
        [property: JsonPropertyName("measuredMm")] double MeasuredMm,
        [property: JsonPropertyName("baselineSimulatedMm")] double BaselineSimulatedMm,
        [property: JsonPropertyName("simulatedMm")] double SimulatedMm,
        [property: JsonPropertyName("differenceMm")] double DifferenceMm,
        [property: JsonPropertyName("differencePercent")] double? DifferencePercent,
        [property: JsonPropertyName("baselineDifferenceMm")] double BaselineDifferenceMm);

    public sealed record CatchCanTestDto(
        [property: JsonPropertyName("pubId")] Guid PubId,
        [property: JsonPropertyName("areaPubId")] Guid AreaPubId,
        [property: JsonPropertyName("areaName")] string AreaName,
        [property: JsonPropertyName("testDateUtc")] DateTime TestDateUtc,
        [property: JsonPropertyName("runtimeSeconds")] int RuntimeSeconds,
        [property: JsonPropertyName("weatherNotes")] string WeatherNotes,
        [property: JsonPropertyName("windSpeedMps")] decimal? WindSpeedMps,
        [property: JsonPropertyName("windDirectionDegrees")] decimal? WindDirectionDegrees,
        [property: JsonPropertyName("active")] bool Active,
        [property: JsonPropertyName("measurements")] IReadOnlyList<CatchCanMeasurementComparisonDto> Measurements,
        [property: JsonPropertyName("measuredMetrics")] CatchCanDistributionMetricsDto MeasuredMetrics,
        [property: JsonPropertyName("baselineModelError")] CatchCanModelErrorMetricsDto BaselineModelError,
        [property: JsonPropertyName("calibratedModelError")] CatchCanModelErrorMetricsDto CalibratedModelError,
        [property: JsonPropertyName("calibration")] IrrigationModelCalibrationSummaryDto Calibration,
        [property: JsonPropertyName("generatedCalibrationFactor")] double? GeneratedCalibrationFactor,
        [property: JsonPropertyName("warnings")] IReadOnlyList<string> Warnings);

    public sealed record CatchCanAreaOptionDto(
        [property: JsonPropertyName("pubId")] Guid PubId,
        [property: JsonPropertyName("name")] string Name,
        [property: JsonPropertyName("hasBoundary")] bool HasBoundary);

    public sealed record CatchCanWorkspaceDto(
        [property: JsonPropertyName("fieldPubId")] Guid FieldPubId,
        [property: JsonPropertyName("fieldName")] string FieldName,
        [property: JsonPropertyName("selectedAreaPubId")] Guid SelectedAreaPubId,
        [property: JsonPropertyName("selectedAreaName")] string SelectedAreaName,
        [property: JsonPropertyName("areas")] IReadOnlyList<CatchCanAreaOptionDto> Areas,
        [property: JsonPropertyName("boundary")] IrrigationAreaBoundaryDto Boundary,
        [property: JsonPropertyName("tests")] IReadOnlyList<CatchCanTestDto> Tests,
        [property: JsonPropertyName("calibration")] IrrigationModelCalibrationSummaryDto Calibration);
}

/// <summary>Pure calculations shared by catch-can imports, persistence, API responses, and tests.</summary>
public static class CatchCanCalibration
{
    public const int MaximumMeasurementsPerTest = 500;

    public static decimal CalculateAppliedDepthMm(decimal collectedMl, decimal containerAreaCm2)
    {
        if (collectedMl < 0m)
            throw new ArgumentOutOfRangeException(nameof(collectedMl), "Collected volume cannot be negative.");
        if (containerAreaCm2 <= 0m)
            throw new ArgumentOutOfRangeException(nameof(containerAreaCm2), "Container area must be greater than zero.");

        // SI conversion: m3 / m2 = m, then convert metres to millimetres.
        var volumeM3 = collectedMl / 1_000_000m;
        var areaM2 = containerAreaCm2 / 10_000m;
        return volumeM3 / areaM2 * 1_000m;
    }

    public static CatchCanCalibrationDtos.CatchCanDistributionMetricsDto AnalyzeMeasurements(
        IReadOnlyList<double> measuredMm)
    {
        ValidateDepths(measuredMm, nameof(measuredMm));
        var metrics = IrrigationDistributionAnalytics.AnalyzeEqualAreaDepths(measuredMm);
        return new CatchCanCalibrationDtos.CatchCanDistributionMetricsDto(
            measuredMm.Count,
            metrics.MeanMm,
            metrics.DistributionUniformityLowQuarter,
            metrics.ChristiansenUniformityCoefficient,
            metrics.MinimumMm,
            metrics.MaximumMm,
            metrics.CoefficientOfVariation);
    }

    public static CatchCanCalibrationDtos.CatchCanModelErrorMetricsDto AnalyzeModelError(
        IReadOnlyList<double> measuredMm,
        IReadOnlyList<double> simulatedMm)
    {
        ValidateDepths(measuredMm, nameof(measuredMm));
        ValidateDepths(simulatedMm, nameof(simulatedMm));
        if (measuredMm.Count != simulatedMm.Count)
            throw new ArgumentException("Measured and simulated point counts must match.", nameof(simulatedMm));

        var errors = measuredMm.Select((measured, index) => simulatedMm[index] - measured).ToArray();
        var percentageErrors = measuredMm
            .Select((measured, index) => measured > 0d
                ? Math.Abs(simulatedMm[index] - measured) / measured * 100d
                : (double?)null)
            .Where(value => value.HasValue)
            .Select(value => value!.Value)
            .ToArray();
        return new CatchCanCalibrationDtos.CatchCanModelErrorMetricsDto(
            measuredMm.Count,
            errors.Average(),
            errors.Average(Math.Abs),
            Math.Sqrt(errors.Average(Square)),
            percentageErrors.Length == 0 ? null : percentageErrors.Average());
    }

    /// <summary>Fits a zero-intercept measured = factor × modeled correction by least squares.</summary>
    public static double? FitApplicationDepthMultiplier(
        IReadOnlyList<double> measuredMm,
        IReadOnlyList<double> baselineSimulatedMm)
    {
        ValidateDepths(measuredMm, nameof(measuredMm));
        ValidateDepths(baselineSimulatedMm, nameof(baselineSimulatedMm));
        if (measuredMm.Count != baselineSimulatedMm.Count)
            throw new ArgumentException("Measured and simulated point counts must match.", nameof(baselineSimulatedMm));

        var denominator = baselineSimulatedMm.Sum(Square);
        if (denominator <= 1e-12d || measuredMm.All(value => value <= 0d))
            return null;
        var factor = measuredMm.Select((measured, index) => measured * baselineSimulatedMm[index]).Sum() / denominator;
        return double.IsFinite(factor) && factor is >= 0.05d and <= 20d ? factor : null;
    }

    public static string CalibrationStatus(int testCount)
        => testCount switch
        {
            <= 0 => "Uncalibrated model",
            1 => "Calibrated from 1 test",
            _ => $"Calibrated from {testCount} tests"
        };

    static void ValidateDepths(IReadOnlyList<double> values, string parameterName)
    {
        ArgumentNullException.ThrowIfNull(values);
        if (values.Count == 0)
            throw new ArgumentException("At least one catch-can depth is required.", parameterName);
        if (values.Any(value => !double.IsFinite(value) || value < 0d))
            throw new ArgumentOutOfRangeException(parameterName, "Catch-can depths must be finite and non-negative.");
    }

    static double Square(double value) => value * value;
}

public sealed record CatchCanCsvRow(
    double X,
    double Y,
    decimal? AppliedMm,
    decimal? CollectedMl,
    decimal? ContainerAreaCm2);

public static class CatchCanCsvParser
{
    public static IReadOnlyList<CatchCanCsvRow> Parse(string csv)
    {
        if (string.IsNullOrWhiteSpace(csv))
            throw new ArgumentException("The CSV file is empty.", nameof(csv));

        var records = ReadRecords(csv);
        if (records.Count < 2)
            throw new ArgumentException("The CSV file needs a header and at least one data row.", nameof(csv));
        var headers = records[0]
            .Select((value, index) => (Name: NormalizeHeader(value), Index: index))
            .Where(item => item.Name.Length > 0)
            .GroupBy(item => item.Name)
            .ToDictionary(group => group.Key, group => group.First().Index, StringComparer.OrdinalIgnoreCase);
        var xIndex = RequiredHeader(headers, "x");
        var yIndex = RequiredHeader(headers, "y");
        var appliedIndex = OptionalHeader(headers, "appliedmm");
        var volumeIndex = OptionalHeader(headers, "collectedml");
        var areaIndex = OptionalHeader(headers, "containerareacm2");
        if (!appliedIndex.HasValue && (!volumeIndex.HasValue || !areaIndex.HasValue))
            throw new ArgumentException("CSV needs AppliedMm, or both CollectedMl and ContainerAreaCm2 columns.", nameof(csv));

        var result = new List<CatchCanCsvRow>();
        for (var recordIndex = 1; recordIndex < records.Count; recordIndex++)
        {
            var record = records[recordIndex];
            if (record.All(string.IsNullOrWhiteSpace))
                continue;
            var rowNumber = recordIndex + 1;
            var x = RequiredDouble(record, xIndex, "X", rowNumber);
            var y = RequiredDouble(record, yIndex, "Y", rowNumber);
            var applied = OptionalDecimal(record, appliedIndex, "AppliedMm", rowNumber);
            var volume = OptionalDecimal(record, volumeIndex, "CollectedMl", rowNumber);
            var area = OptionalDecimal(record, areaIndex, "ContainerAreaCm2", rowNumber);
            if (!applied.HasValue && (!volume.HasValue || !area.HasValue))
                throw new ArgumentException($"CSV row {rowNumber} needs AppliedMm, or both CollectedMl and ContainerAreaCm2.", nameof(csv));
            result.Add(new CatchCanCsvRow(x, y, applied, volume, area));
        }

        if (result.Count == 0)
            throw new ArgumentException("The CSV file contains no measurement rows.", nameof(csv));
        if (result.Count > CatchCanCalibration.MaximumMeasurementsPerTest)
            throw new ArgumentException($"A catch-can test may contain at most {CatchCanCalibration.MaximumMeasurementsPerTest} measurements.", nameof(csv));
        return result;
    }

    static List<List<string>> ReadRecords(string csv)
    {
        var records = new List<List<string>>();
        var record = new List<string>();
        var field = new System.Text.StringBuilder();
        var quoted = false;
        for (var index = 0; index < csv.Length; index++)
        {
            var character = csv[index];
            if (quoted)
            {
                if (character == '"' && index + 1 < csv.Length && csv[index + 1] == '"')
                {
                    field.Append('"');
                    index++;
                }
                else if (character == '"')
                    quoted = false;
                else
                    field.Append(character);
                continue;
            }

            if (character == '"' && field.Length == 0)
                quoted = true;
            else if (character == ',')
            {
                record.Add(field.ToString().Trim());
                field.Clear();
            }
            else if (character is '\r' or '\n')
            {
                if (character == '\r' && index + 1 < csv.Length && csv[index + 1] == '\n')
                    index++;
                record.Add(field.ToString().Trim());
                field.Clear();
                records.Add(record);
                record = [];
            }
            else
                field.Append(character);
        }

        if (quoted)
            throw new ArgumentException("The CSV file contains an unterminated quoted field.", nameof(csv));
        if (field.Length > 0 || record.Count > 0)
        {
            record.Add(field.ToString().Trim());
            records.Add(record);
        }
        return records;
    }

    static int RequiredHeader(IReadOnlyDictionary<string, int> headers, string name)
        => headers.TryGetValue(name, out var index)
            ? index
            : throw new ArgumentException($"CSV column '{name}' is required.");

    static int? OptionalHeader(IReadOnlyDictionary<string, int> headers, string name)
        => headers.TryGetValue(name, out var index) ? index : null;

    static string NormalizeHeader(string value)
        => new(value.Trim().Where(char.IsLetterOrDigit).Select(char.ToLowerInvariant).ToArray());

    static double RequiredDouble(IReadOnlyList<string> record, int index, string label, int row)
    {
        var value = Read(record, index);
        if (double.TryParse(value, System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture, out var parsed) &&
            double.IsFinite(parsed))
            return parsed;
        throw new ArgumentException($"CSV row {row} has an invalid {label} value.");
    }

    static decimal? OptionalDecimal(IReadOnlyList<string> record, int? index, string label, int row)
    {
        if (!index.HasValue)
            return null;
        var value = Read(record, index.Value);
        if (string.IsNullOrWhiteSpace(value))
            return null;
        if (decimal.TryParse(value, System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture, out var parsed))
            return parsed;
        throw new ArgumentException($"CSV row {row} has an invalid {label} value.");
    }

    static string Read(IReadOnlyList<string> record, int index)
        => index < record.Count ? record[index] : "";
}
