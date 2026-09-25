using SFA.DAS.Apim.Shared.Interfaces;

namespace SFA.DAS.EmployerFinance.InnerApi.Requests.Commitments;

public sealed record GetCommittedLearnersCostByAccountIdRequest(
    long AccountId,
    int PageNumber = 1,
    int PageItemCount = 1000,
    long? TransferSenderId = null) : IGetApiRequest
{
    public string GetUrl => TransferSenderId.HasValue
        ? $"api/apprenticeships?accountId={AccountId}&transferSenderId={TransferSenderId.Value}&pageNumber={PageNumber}&pageItemCount={PageItemCount}"
        : $"api/apprenticeships?accountId={AccountId}&pageNumber={PageNumber}&pageItemCount={PageItemCount}";
}