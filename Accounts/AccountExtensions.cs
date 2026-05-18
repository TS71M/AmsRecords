using static AmsRecords.Accounts.AccountDtos;

namespace AmsRecords.Accounts;

public static class AccountExtensions
{
    public static AccountDto ToDto(this Account account)
           => new(
               account.PubId,
               account.AccNum,
               account.AccDes,
               account.AccountGroup?.PubId,
               account.AccountGroup?.AccountGroupName,
               account.Image
               );
}