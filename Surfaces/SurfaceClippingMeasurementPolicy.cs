namespace AmsRecords.Surfaces;

public static class SurfaceClippingMeasurementPolicy
{
    public static bool CanConfigureSurface(bool areaClippingsRemoved)
        => areaClippingsRemoved;

    public static bool DefaultSurfaceSelection(bool areaClippingsRemoved)
        => areaClippingsRemoved;

    public static bool IsEnabled(bool areaClippingsRemoved, bool surfaceSelected)
        => areaClippingsRemoved && surfaceSelected;
}
