using AmsRecords.ContactDetails;
using AmsRecords.Name;
using AmsRecords.Roles;
using static AmsRecords.Users.UserDtos;

namespace AmsRecords.Users;

public static class UserExtensions
{
    public static UserDto ToDto(this User user, DateTime? fallbackLastSeenUtc = null)
        => new(
            PubId: user.PubId,
            Email: user.Email!,
            UserName: user.UserName!,
            Active: user.Active,
            LockedOut: user.LockoutEnabled && user.LockoutEnd.HasValue && user.LockoutEnd.Value > DateTimeOffset.UtcNow,
            LockoutEnd: user.LockoutEnd,
            LastSeenUtc: user.LastSeenUtc ?? fallbackLastSeenUtc,
            IbuPubId: user.Ibu!.PubId,
            Name: user.Name?.ToDto(),
            ImagePubId: user.UserImg?.PubId,
            Tolerance: user.Tolerance,
            WkOfWarn: user.WkOfWarn,
            ConsiderMlsn: user.ConsiderMlsn,
            CheckApps: user.CheckApps,
            CheckOrders: user.CheckOrders,
            CheckEt: user.CheckEt,
            ShowLastDays: user.ShowLastDays,
            ShowProgress: user.ShowProgress,
            ConsiderSkills: user.ConsiderSkills,
            RandomizeAssignments: user.RandomizeAssignments,
            ThisWeek: user.ThisWeek,
            NextWeek: user.NextWeek,
            Irrigation: user.Irrigation,
            Stock: user.Stock,
            Today: user.Today,
            History: user.History,
            PlanningYearPubId: user.Year?.PubId,
            LoadYears: user.LoadYears,
            PreferredCountryPubId: user.PreferredCountry?.PubId,
            PreferredCountryCode: user.PreferredCountry?.CountryCode,
            PreferredCountryName: user.PreferredCountry?.CountryName,
            PreferredLanguagePubId: user.PreferredLanguage?.PubId,
            PreferredLanguageCode: user.PreferredLanguage?.LanguageCode,
            PreferredLanguageName: user.PreferredLanguage?.LanguageName,
            Contact: user.ContactDetail?.ToDto(),
            ManagerUserPubId: user.ManagerUser?.PubId,
            ManagerUserName: ResolveDisplayName(user.ManagerUser),
            UserPositionPubId: user.UserPosition?.PubId,
            UserPositionName: user.UserPosition?.UserPositionName,
            AccessProfilePubId: user.AccessProfile?.PubId,
            AccessProfileName: user.AccessProfile?.ProfileName,
            Roles: [.. user.UserRole.Select(ur => ur.Role.ToDto())],
            VisualizationTempUnitPubId: user.VisualizationTempUnit?.PubId,
            VisualizationRainUnitPubId: user.VisualizationRainUnit?.PubId,
            VisualizationWindUnitPubId: user.VisualizationWindUnit?.PubId
            )
        {
            ManagerUserPubIds = [.. GetReportingManagers(user).Select(x => x.PubId)],
            ManagerUserNames = [.. GetReportingManagers(user).Select(ResolveDisplayName).Where(x => !string.IsNullOrWhiteSpace(x)).Select(x => x!)]
        };

    public static User ToEntity(this UserCreateDto dto, int ibuId, int? countryId)
        => new()
        {
            Email = dto.Email,
            UserName = dto.UserName,
            LockoutEnabled = true,
            LockoutEnd = dto.LockedOut
                ? DateTimeOffset.MaxValue
                : null,
            Active = true,
            IbuId = ibuId,
            Tolerance = dto.Tolerance,
            WkOfWarn = dto.WkOfWarn,
            ConsiderMlsn = dto.ConsiderMlsn,
            CheckApps = dto.CheckApps,
            CheckOrders = dto.CheckOrders,
            CheckEt = dto.CheckEt,
            ShowLastDays = dto.ShowLastDays,
            ShowProgress = dto.ShowProgress,
            ConsiderSkills = dto.ConsiderSkills,
            RandomizeAssignments = dto.RandomizeAssignments,
            ThisWeek = dto.ThisWeek,
            NextWeek = dto.NextWeek,
            Irrigation = dto.Irrigation,
            Stock = dto.Stock,
            Today = dto.Today,
            History = dto.History,
            PreferredCountryId = countryId,
            ManagerUserId = null,
            LoadYears = dto.LoadYears,
            EmailConfirmed = true // Admin-created users are treated as already verified by the creating administrator
        };

    public static UserUpdateDto ToUpdateDto(this UserDto dto)
        => new(
            PubId: dto.PubId,
            IbuPubId: dto.IbuPubId,
            Email: dto.Email,
            UserName: dto.UserName,
            LockedOut: dto.LockedOut,
            Name: dto.Name,
            ImagePubId: dto.ImagePubId,
            Tolerance: dto.Tolerance,
            WkOfWarn: dto.WkOfWarn,
            ConsiderMlsn: dto.ConsiderMlsn,
            CheckApps: dto.CheckApps,
            CheckOrders: dto.CheckOrders,
            CheckEt: dto.CheckEt,
            ShowLastDays: dto.ShowLastDays,
            ShowProgress: dto.ShowProgress,
            ConsiderSkills: dto.ConsiderSkills,
            RandomizeAssignments: dto.RandomizeAssignments,
            ThisWeek: dto.ThisWeek,
            NextWeek: dto.NextWeek,
            Irrigation: dto.Irrigation,
            Stock: dto.Stock,
            Today: dto.Today,
            History: dto.History,
            PlanningYearPubId: dto.PlanningYearPubId,
            LoadYears: dto.LoadYears,
            PreferredCountryPubId: dto.PreferredCountryPubId,
            PreferredLanguagePubId: dto.PreferredLanguagePubId,
            Contact: dto.Contact,
            ManagerUserPubId: dto.ManagerUserPubId,
            UserPositionPubId: dto.UserPositionPubId,
            AccessProfilePubId: dto.AccessProfilePubId,
            VisualizationTempUnitPubId: dto.VisualizationTempUnitPubId,
            VisualizationRainUnitPubId: dto.VisualizationRainUnitPubId,
            VisualizationWindUnitPubId: dto.VisualizationWindUnitPubId
            )
        {
            RoleIds = [.. dto.Roles.Select(r => r.Id)],
            ManagerUserPubIds = dto.ManagerUserPubIds.Count > 0
                ? [.. dto.ManagerUserPubIds]
                : dto.ManagerUserPubId is Guid managerPubId && managerPubId != Guid.Empty
                    ? [managerPubId]
                    : []
        };

    public static UserProfileUpdateDto ToProfileUpdateDto(this UserDto dto)
        => new(
            PubId: dto.PubId,
            Email: dto.Email,
            UserName: dto.UserName,
            Name: dto.Name,
            ImagePubId: dto.ImagePubId,
            PreferredCountryPubId: dto.PreferredCountryPubId,
            PreferredLanguagePubId: dto.PreferredLanguagePubId,
            Contact: dto.Contact,
            VisualizationTempUnitPubId: dto.VisualizationTempUnitPubId,
            VisualizationRainUnitPubId: dto.VisualizationRainUnitPubId,
            VisualizationWindUnitPubId: dto.VisualizationWindUnitPubId
            );

    public static UserTemporaryReplacementDto ToDto(this UserTemporaryReplacement entity)
        => new(
            PubId: entity.PubId,
            IbuPubId: entity.Ibu.PubId,
            AbsentUserPubId: entity.AbsentUser.PubId,
            AbsentUserName: ResolveDisplayName(entity.AbsentUser),
            ReplacementUserPubId: entity.ReplacementUser.PubId,
            ReplacementUserName: ResolveDisplayName(entity.ReplacementUser),
            StartDate: entity.StartDate,
            EndDate: entity.EndDate,
            Reason: entity.Reason,
            Active: entity.Active);

    public static UserCreateDto ToCreateDto(this UserUpdateDto dto, Guid ibuPubId)
        => new(
            Email: dto.Email,
            UserName: dto.UserName,
            LockedOut: dto.LockedOut,
            IbuPubId: dto.IbuPubId != Guid.Empty ? dto.IbuPubId : ibuPubId,
            Tolerance: dto.Tolerance,
            WkOfWarn: dto.WkOfWarn,
            ConsiderMlsn: dto.ConsiderMlsn,
            CheckApps: dto.CheckApps,
            CheckOrders: dto.CheckOrders,
            CheckEt: dto.CheckEt,
            ShowLastDays: dto.ShowLastDays,
            ShowProgress: dto.ShowProgress,
            ConsiderSkills: dto.ConsiderSkills,
            RandomizeAssignments: dto.RandomizeAssignments,
            ThisWeek: dto.ThisWeek,
            NextWeek: dto.NextWeek,
            Irrigation: dto.Irrigation,
            Stock: dto.Stock,
            Today: dto.Today,
            History: dto.History,
            PlanningYearPubId: dto.PlanningYearPubId,
            LoadYears: dto.LoadYears,
            PreferredCountryPubId: dto.PreferredCountryPubId,
            PreferredLanguagePubId: dto.PreferredLanguagePubId,
            AppImageCreateDto: dto.AppImageCreateDto,
            ManagerUserPubId: dto.ManagerUserPubId,
            UserPositionPubId: dto.UserPositionPubId,
            AccessProfilePubId: dto.AccessProfilePubId,
            VisualizationTempUnitPubId: dto.VisualizationTempUnitPubId,
            VisualizationRainUnitPubId: dto.VisualizationRainUnitPubId,
            VisualizationWindUnitPubId: dto.VisualizationWindUnitPubId
        )
        {
            RoleIds = dto.RoleIds,
            ManagerUserPubIds = [.. dto.ManagerUserPubIds]
        };

    public static void UpdateEntity(this User user, UserUpdateDto dto, Ibu ibu, Year? year = null)
    {
        user.Email = dto.Email;
        user.UserName = dto.UserName;
        user.LockoutEnabled = true;
        user.LockoutEnd = dto.LockedOut
            ? DateTimeOffset.MaxValue
            : null;
        if (!dto.LockedOut)
            user.AccessFailedCount = 0;

        // ✅ FK only (avoid nav assignment)
        user.IbuId = ibu.IbuId;

        user.Tolerance = dto.Tolerance;
        user.WkOfWarn = dto.WkOfWarn;
        user.ConsiderMlsn = dto.ConsiderMlsn;
        user.CheckApps = dto.CheckApps;
        user.CheckOrders = dto.CheckOrders;
        user.CheckEt = dto.CheckEt;
        user.ShowLastDays = dto.ShowLastDays;
        user.ShowProgress = dto.ShowProgress;
        user.ConsiderSkills = dto.ConsiderSkills;
        user.RandomizeAssignments = dto.RandomizeAssignments;
        user.ThisWeek = dto.ThisWeek;
        user.NextWeek = dto.NextWeek;
        user.Irrigation = dto.Irrigation;
        user.Stock = dto.Stock;
        user.Today = dto.Today;
        user.History = dto.History;
        user.LoadYears = dto.LoadYears;
        user.ManagerUserId = null;

        user.PlanningYearId = year?.YearId;
    }

    static string? ResolveDisplayName(User? user)
    {
        if (user is null)
            return null;

        var fullName = user.Name?.FullName?.Trim();
        var userName = user.UserName?.Trim();

        if (!string.IsNullOrWhiteSpace(fullName) && !string.IsNullOrWhiteSpace(userName) && !string.Equals(fullName, userName, StringComparison.OrdinalIgnoreCase))
            return $"{fullName} ({userName})";

        if (!string.IsNullOrWhiteSpace(fullName))
            return fullName;

        return !string.IsNullOrWhiteSpace(userName)
            ? userName
            : user.Email;
    }

    static List<User> GetReportingManagers(User user)
    {
        var managers = user.ReportingLines
            .Select(x => x.ManagerUser)
            .Where(x => x is not null)
            .Cast<User>()
            .Where(x => x.Active)
            .GroupBy(x => x.Id)
            .Select(x => x.First())
            .OrderBy(ResolveDisplayName)
            .ToList();

        if (managers.Count == 0 && user.ManagerUser is not null && user.ManagerUser.Active)
            managers.Add(user.ManagerUser);

        return managers;
    }
}
