using static AmsRecords.Accounts.AccountGroupDtos;

namespace AmsRecords.Accounts;

public static class AccountGroupExtensions
{
    public static AccountGroupDto ToDto(this AccountGroup accountGroup)
            => new(
                accountGroup.PubId,
                accountGroup.OrderNumber,
                accountGroup.AccountGroupName,
                accountGroup.Image
                );
}