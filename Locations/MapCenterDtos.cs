namespace AmsRecords.Locations;

public static class MapCenterDtos
{
    public sealed record MapCenterDto(double Latitude, double Longitude, string Source);

    public static class Sources
    {
        public const string Field = "Field";
        public const string Ibu = "Ibu";
        public const string CompanyOrConsultant = "CompanyOrConsultant";
        public const string EmergencyDefault = "EmergencyDefault";
    }

    public static class Defaults
    {
        public const double EmergencyLatitude = 47d;
        public const double EmergencyLongitude = 19d;
    }
}
