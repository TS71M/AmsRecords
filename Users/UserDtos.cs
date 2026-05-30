using static AmsRecords.AppImages.AppImageDtos;
using static AmsRecords.Authentication.AuthenticationDtos;
using static AmsRecords.ContactDetails.ContactDetailDtos;
using static AmsRecords.Name.NameDtos;
using static AmsRecords.Roles.RoleDtos;

namespace AmsRecords.Users;

public static class UserDtos
{
    public interface IUserPreferenceUpdateDto
    {
        Guid PubId { get; }
        Guid? PreferredCountryPubId { get; }
        Guid? PreferredLanguagePubId { get; }
        Guid? VisualizationTempUnitPubId { get; }
        Guid? VisualizationRainUnitPubId { get; }
        Guid? VisualizationWindUnitPubId { get; }
        Guid? ImagePubId { get; }
        AppImageCreateDto? AppImageCreateDto { get; }
    }

    public record UserDto(
        [property: JsonPropertyName("publicId")] Guid PubId,
        [property: JsonPropertyName("email")] string Email,
        [property: JsonPropertyName("username")] string UserName,
        [property: JsonPropertyName("active")] bool Active,
        [property: JsonPropertyName("lockedOut")] bool LockedOut,
        [property: JsonPropertyName("lockoutEnd")] DateTimeOffset? LockoutEnd,
        [property: JsonPropertyName("lastSeenUtc")] DateTime? LastSeenUtc,
        [property: JsonPropertyName("ibuPubId")][Required] Guid IbuPubId,
        [property: JsonPropertyName("name")] NameDto? Name,
        [property: JsonPropertyName("imagePubId")] Guid? ImagePubId,
        [property: JsonPropertyName("tolerance")] int Tolerance,
        [property: JsonPropertyName("wkOfWarn")] int WkOfWarn,
        [property: JsonPropertyName("considerMlsn")] bool ConsiderMlsn,
        [property: JsonPropertyName("checkApps")] bool CheckApps,
        [property: JsonPropertyName("checkOrders")] bool CheckOrders,
        [property: JsonPropertyName("checkEt")] bool CheckEt,
        [property: JsonPropertyName("showLastDays")] bool ShowLastDays,
        [property: JsonPropertyName("showProgress")] bool ShowProgress,
        [property: JsonPropertyName("considerSkills")] bool ConsiderSkills,
        [property: JsonPropertyName("randomizeAssignments")] bool RandomizeAssignments,
        [property: JsonPropertyName("thisWeek")] bool ThisWeek,
        [property: JsonPropertyName("nextWeek")] bool NextWeek,
        [property: JsonPropertyName("irrigation")] bool Irrigation,
        [property: JsonPropertyName("stock")] bool Stock,
        [property: JsonPropertyName("today")] bool Today,
        [property: JsonPropertyName("history")] bool History,
        [property: JsonPropertyName("planningYearPubId")] Guid? PlanningYearPubId,
        [property: JsonPropertyName("loadYears")] int LoadYears,
        [property: JsonPropertyName("preferredCountryPubId")] Guid? PreferredCountryPubId,
        [property: JsonPropertyName("preferredCountryCode")] string? PreferredCountryCode,
        [property: JsonPropertyName("preferredCountryName")] string? PreferredCountryName,
        [property: JsonPropertyName("preferredLanguagePubId")] Guid? PreferredLanguagePubId,
        [property: JsonPropertyName("preferredLanguageCode")] string? PreferredLanguageCode,
        [property: JsonPropertyName("preferredLanguageName")] string? PreferredLanguageName,
        [property: JsonPropertyName("contact")] ContactDetailDto? Contact,
        [property: JsonPropertyName("managerUserPubId")] Guid? ManagerUserPubId,
        [property: JsonPropertyName("managerUserName")] string? ManagerUserName,
        [property: JsonPropertyName("userPositionPubId")] Guid? UserPositionPubId,
        [property: JsonPropertyName("userPositionName")] string? UserPositionName,
        [property: JsonPropertyName("accessProfilePubId")] Guid? AccessProfilePubId,
        [property: JsonPropertyName("accessProfileName")] string? AccessProfileName,
        [property: JsonPropertyName("role")] List<RoleDto> Roles,
        [property: JsonPropertyName("passwordResetEmailJob")] PasswordResetEmailJobStatusDto? PasswordResetEmailJob = null,
        [property: JsonPropertyName("visualizationTempUnitPubId")] Guid? VisualizationTempUnitPubId = default,
        [property: JsonPropertyName("visualizationRainUnitPubId")] Guid? VisualizationRainUnitPubId = default,
        [property: JsonPropertyName("visualizationWindUnitPubId")] Guid? VisualizationWindUnitPubId = default
        )
    {
        public string? ImageThumbDataUrl { get; set; }
        [JsonPropertyName("managerUserPubIds")]
        public List<Guid> ManagerUserPubIds { get; init; } = [];
        [JsonPropertyName("managerUserNames")]
        public List<string> ManagerUserNames { get; init; } = [];
    };

    public record UserCreateDto(
        [property: JsonPropertyName("email")][Required, EmailAddress] string Email,
        [property: JsonPropertyName("userName")][Required, MinLength(4), MaxLength(100)] string UserName,
        [property: JsonPropertyName("lockedOut")] bool LockedOut,
        [property: JsonPropertyName("ibuPubId")][Required] Guid IbuPubId,
        [property: JsonPropertyName("tolerance")] int Tolerance,
        [property: JsonPropertyName("wkOfWarn")] int WkOfWarn,
        [property: JsonPropertyName("considerMlsn")] bool ConsiderMlsn,
        [property: JsonPropertyName("checkApps")] bool CheckApps,
        [property: JsonPropertyName("checkOrders")] bool CheckOrders,
        [property: JsonPropertyName("checkEt")] bool CheckEt,
        [property: JsonPropertyName("showLastDays")] bool ShowLastDays,
        [property: JsonPropertyName("showProgress")] bool ShowProgress,
        [property: JsonPropertyName("considerSkills")] bool ConsiderSkills,
        [property: JsonPropertyName("randomizeAssignments")] bool RandomizeAssignments,
        [property: JsonPropertyName("thisWeek")] bool ThisWeek,
        [property: JsonPropertyName("nextWeek")] bool NextWeek,
        [property: JsonPropertyName("irrigation")] bool Irrigation,
        [property: JsonPropertyName("stock")] bool Stock,
        [property: JsonPropertyName("today")] bool Today,
        [property: JsonPropertyName("history")] bool History,
        [property: JsonPropertyName("planningYearPubId")] Guid? PlanningYearPubId,
        [property: JsonPropertyName("loadYears")] int LoadYears,
        [property: JsonPropertyName("preferredCountryPubId")] Guid? PreferredCountryPubId,
        [property: JsonPropertyName("preferredLanguagePubId")] Guid? PreferredLanguagePubId,
        [property: JsonPropertyName("appImageCreateDto")] AppImageCreateDto? AppImageCreateDto,
        [property: JsonPropertyName("managerUserPubId")] Guid? ManagerUserPubId,
        [property: JsonPropertyName("userPositionPubId")] Guid? UserPositionPubId,
        [property: JsonPropertyName("accessProfilePubId")] Guid? AccessProfilePubId,
        [property: JsonPropertyName("visualizationTempUnitPubId")] Guid? VisualizationTempUnitPubId = default,
        [property: JsonPropertyName("visualizationRainUnitPubId")] Guid? VisualizationRainUnitPubId = default,
        [property: JsonPropertyName("visualizationWindUnitPubId")] Guid? VisualizationWindUnitPubId = default
        )
    {
        public UserCreateDto() : this(
            Email: string.Empty,
            UserName: string.Empty,
            LockedOut: false,
            IbuPubId: Guid.Empty,
            Tolerance: 0,
            WkOfWarn: 4,
            ConsiderMlsn: false,
            CheckApps: false,
            CheckOrders: false,
            CheckEt: false,
            ShowLastDays: false,
            ShowProgress: false,
            ConsiderSkills: false,
            RandomizeAssignments: false,
            ThisWeek: false,
            NextWeek: false,
            Irrigation: false,
            Stock: false,
            Today: false,
            History: false,
            PlanningYearPubId: null,
            LoadYears: 3,
            PreferredCountryPubId: null,
            PreferredLanguagePubId: null,
            AppImageCreateDto: null,
            ManagerUserPubId: null,
            UserPositionPubId: null,
            AccessProfilePubId: null,
            VisualizationTempUnitPubId: null,
            VisualizationRainUnitPubId: null,
            VisualizationWindUnitPubId: null)
        { }

        [JsonPropertyName("roleIds")]
        public List<int> RoleIds { get; set; } = [];

        [JsonPropertyName("managerUserPubIds")]
        public List<Guid> ManagerUserPubIds { get; set; } = [];
    }

    public record UserUpdateDto(
        [property: JsonPropertyName("publicId")][Required] Guid PubId,
        [property: JsonPropertyName("ibuPubId")][Required] Guid IbuPubId,
        [property: JsonPropertyName("email")][Required, EmailAddress] string Email,
        [property: JsonPropertyName("userName")][Required, MinLength(4), MaxLength(100)] string UserName,
        [property: JsonPropertyName("lockedOut")] bool LockedOut,
        [property: JsonPropertyName("name")] NameDto? Name,
        [property: JsonPropertyName("imagePubId")] Guid? ImagePubId,
        [property: JsonPropertyName("tolerance")] int Tolerance,
        [property: JsonPropertyName("wkOfWarn")] int WkOfWarn,
        [property: JsonPropertyName("considerMlsn")] bool ConsiderMlsn,
        [property: JsonPropertyName("checkApps")] bool CheckApps,
        [property: JsonPropertyName("checkOrders")] bool CheckOrders,
        [property: JsonPropertyName("checkEt")] bool CheckEt,
        [property: JsonPropertyName("showLastDays")] bool ShowLastDays,
        [property: JsonPropertyName("showProgress")] bool ShowProgress,
        [property: JsonPropertyName("considerSkills")] bool ConsiderSkills,
        [property: JsonPropertyName("randomizeAssignments")] bool RandomizeAssignments,
        [property: JsonPropertyName("thisWeek")] bool ThisWeek,
        [property: JsonPropertyName("nextWeek")] bool NextWeek,
        [property: JsonPropertyName("irrigation")] bool Irrigation,
        [property: JsonPropertyName("stock")] bool Stock,
        [property: JsonPropertyName("today")] bool Today,
        [property: JsonPropertyName("history")] bool History,
        [property: JsonPropertyName("planningYearPubId")] Guid? PlanningYearPubId,
        [property: JsonPropertyName("loadYears")] int LoadYears,
        [property: JsonPropertyName("preferredCountryPubId")] Guid? PreferredCountryPubId,
        [property: JsonPropertyName("preferredLanguagePubId")] Guid? PreferredLanguagePubId,
        [property: JsonPropertyName("contact")] ContactDetailDto? Contact,
        [property: JsonPropertyName("managerUserPubId")] Guid? ManagerUserPubId,
        [property: JsonPropertyName("userPositionPubId")] Guid? UserPositionPubId,
        [property: JsonPropertyName("accessProfilePubId")] Guid? AccessProfilePubId,
        [property: JsonPropertyName("visualizationTempUnitPubId")] Guid? VisualizationTempUnitPubId = default,
        [property: JsonPropertyName("visualizationRainUnitPubId")] Guid? VisualizationRainUnitPubId = default,
        [property: JsonPropertyName("visualizationWindUnitPubId")] Guid? VisualizationWindUnitPubId = default
        ) : IUserPreferenceUpdateDto
    {
        public UserUpdateDto() : this(
            PubId: Guid.Empty,
            IbuPubId: Guid.Empty,
            Email: string.Empty,
            UserName: string.Empty,
            LockedOut: false,
            Name: new NameDto(),
            ImagePubId: null,
            Tolerance: 0,
            WkOfWarn: 4,
            ConsiderMlsn: false,
            CheckApps: false,
            CheckOrders: false,
            CheckEt: false,
            ShowLastDays: false,
            ShowProgress: false,
            ConsiderSkills: false,
            RandomizeAssignments: false,
            ThisWeek: false,
            NextWeek: false,
            Irrigation: false,
            Stock: false,
            Today: false,
            History: false,
            PlanningYearPubId: null,
            LoadYears: 3,
            PreferredCountryPubId: null,
            PreferredLanguagePubId: null,
            Contact: null,
            ManagerUserPubId: null,
            UserPositionPubId: null,
            AccessProfilePubId: null,
            VisualizationTempUnitPubId: null,
            VisualizationRainUnitPubId: null,
            VisualizationWindUnitPubId: null)
        { }
        public string? ImageThumbDataUrl { get; set; }
        public AppImageCreateDto? AppImageCreateDto { get; set; }

        [JsonPropertyName("roleIds")]
        public List<int> RoleIds { get; set; } = [];

        [JsonPropertyName("managerUserPubIds")]
        public List<Guid> ManagerUserPubIds { get; set; } = [];
    }

    public sealed record UserProfileUpdateDto(
        [property: JsonPropertyName("publicId")] Guid PubId,
        [property: JsonPropertyName("email")][param: Required, EmailAddress] string Email,
        [property: JsonPropertyName("userName")][param: Required, MinLength(4), MaxLength(100)] string UserName,
        [property: JsonPropertyName("name")] NameDto? Name,
        [property: JsonPropertyName("imagePubId")] Guid? ImagePubId,
        [property: JsonPropertyName("preferredCountryPubId")] Guid? PreferredCountryPubId,
        [property: JsonPropertyName("preferredLanguagePubId")] Guid? PreferredLanguagePubId,
        [property: JsonPropertyName("contact")] ContactDetailDto? Contact,
        [property: JsonPropertyName("visualizationTempUnitPubId")] Guid? VisualizationTempUnitPubId = default,
        [property: JsonPropertyName("visualizationRainUnitPubId")] Guid? VisualizationRainUnitPubId = default,
        [property: JsonPropertyName("visualizationWindUnitPubId")] Guid? VisualizationWindUnitPubId = default
    ) : IUserPreferenceUpdateDto
    {
        public string? ImageThumbDataUrl { get; set; }
        public AppImageCreateDto? AppImageCreateDto { get; set; }
    }

    public sealed record UserTemporaryReplacementDto(
        [property: JsonPropertyName("publicId")] Guid PubId,
        [property: JsonPropertyName("ibuPubId")] Guid IbuPubId,
        [property: JsonPropertyName("absentUserPubId")] Guid AbsentUserPubId,
        [property: JsonPropertyName("absentUserName")] string? AbsentUserName,
        [property: JsonPropertyName("replacementUserPubId")] Guid ReplacementUserPubId,
        [property: JsonPropertyName("replacementUserName")] string? ReplacementUserName,
        [property: JsonPropertyName("startDate")] DateOnly StartDate,
        [property: JsonPropertyName("endDate")] DateOnly EndDate,
        [property: JsonPropertyName("reason")] string? Reason,
        [property: JsonPropertyName("active")] bool Active);

    public sealed record UserTemporaryReplacementSaveDto(
        [property: JsonPropertyName("publicId")] Guid PubId,
        [property: JsonPropertyName("ibuPubId")][Required] Guid IbuPubId,
        [property: JsonPropertyName("absentUserPubId")][Required] Guid AbsentUserPubId,
        [property: JsonPropertyName("replacementUserPubId")][Required] Guid ReplacementUserPubId,
        [property: JsonPropertyName("startDate")][Required] DateOnly StartDate,
        [property: JsonPropertyName("endDate")][Required] DateOnly EndDate,
        [property: JsonPropertyName("reason")] string? Reason,
        [property: JsonPropertyName("active")] bool Active)
    {
        public UserTemporaryReplacementSaveDto() : this(
            PubId: Guid.Empty,
            IbuPubId: Guid.Empty,
            AbsentUserPubId: Guid.Empty,
            ReplacementUserPubId: Guid.Empty,
            StartDate: DateOnly.FromDateTime(DateTime.Today),
            EndDate: DateOnly.FromDateTime(DateTime.Today),
            Reason: null,
            Active: true)
        { }
    }

    
}
