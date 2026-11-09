using System.ComponentModel.DataAnnotations;

namespace AmsRecords.Accounts;

public static class IbuAccountDtos
{
    public sealed record AccountRow(Guid PubId, int Number, string Name, Guid GroupPubId,
        string GroupName, string SharedGroupName, bool IsActive, bool IsCustom,
        Guid? SharedAccountPubId, long Version);
    public sealed record GroupRow(Guid PubId, string Name, string SharedName, long Version);
    public sealed record Chart(Guid IbuPubId, string IbuName, bool CanEdit,
        IReadOnlyList<AccountRow> Accounts, IReadOnlyList<GroupRow> Groups);

    public sealed class SaveAccount
    {
        [Range(1, int.MaxValue)] public int Number { get; set; }
        [Required, MaxLength(250)] public string Name { get; set; } = "";
        public Guid GroupPubId { get; set; }
        public bool IsActive { get; set; } = true;
        [Range(0, long.MaxValue)] public long ExpectedVersion { get; set; }
    }

    public sealed class RenameGroup
    {
        [Required, MaxLength(250)] public string Name { get; set; } = "";
        [Range(0, long.MaxValue)] public long ExpectedVersion { get; set; }
    }
}
