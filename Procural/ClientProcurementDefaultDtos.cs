namespace AmsRecords.Procural;

public static class ClientProcurementDefaultDtos
{
    public const string Route = "client-procurement-defaults";
    public sealed record Client(Guid PubId, string Name);
    public sealed record Proposal(Guid PubId, string ClientName, string HubName, DateTime ProposedAtUtc,
        DateTime? DecidedAtUtc, bool Approved, bool IncludeFutureFields, bool FutureActive, bool Expired)
    {
        public string Status => Expired ? "Proposal expired" : DecidedAtUtc is null ? "Awaiting client approval" :
            !Approved ? "Declined" : FutureActive ? "Approved, including future fields" : "Approved";
    }
    public sealed record HubPage(IReadOnlyList<Client> Clients, IReadOnlyList<Proposal> Proposals, bool HubActive);
    public sealed record Impact(Guid PubId, string Name, bool HasOverride);
    public sealed record Review(Proposal Proposal, IReadOnlyList<Impact> Fields, bool FutureAllowed,
        string? Token, string? Problem, string? ReplacedFutureHub);
    public sealed record Propose(Guid OperationId, Guid ClientPubId);
    public sealed record Decide(bool Approve, bool IncludeFutureFields, string? ReviewToken);
}
