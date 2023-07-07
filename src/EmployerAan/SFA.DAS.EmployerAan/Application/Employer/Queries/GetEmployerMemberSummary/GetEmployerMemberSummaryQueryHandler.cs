using MediatR;
using SFA.DAS.EmployerAan.Infrastructure;

namespace SFA.DAS.EmployerAan.Application.Employer.Queries.GetEmployerMemberSummary
{
    public class GetEmployerMemberSummaryQueryHandler : IRequestHandler<GetEmployerMemberSummaryQuery, GetEmployerMemberSummaryQueryResult?>
    {
        private readonly ICommitmentsV2ApiClient _commitmentsV2ApiClient;

        public GetEmployerMemberSummaryQueryHandler(ICommitmentsV2ApiClient commitmentsV2ApiClient)
        {
            _commitmentsV2ApiClient = commitmentsV2ApiClient;
        }

        public async Task<GetEmployerMemberSummaryQueryResult?> Handle(GetEmployerMemberSummaryQuery request, CancellationToken cancellationToken)
        {
            var responseEmployerAccount = await _commitmentsV2ApiClient.GetEmployerAccounts(request.EmployerAccountId, cancellationToken);
            var responseEmployerSummary = await _commitmentsV2ApiClient.GetApprenticeshipsSummaryForEmployer(request.EmployerAccountId, cancellationToken);

            GetEmployerMemberSummaryQueryResult result = new();

            if (responseEmployerAccount.ResponseMessage.StatusCode == System.Net.HttpStatusCode.OK)
            {
                result.ActiveCount = responseEmployerAccount.GetContent()!.ApprenticeshipStatusSummaryResponse!.FirstOrDefault()!.ActiveCount;
            }

            if (responseEmployerSummary.ResponseMessage.StatusCode == System.Net.HttpStatusCode.OK)
            {
                result.StartDate = responseEmployerSummary.GetContent()!.StartDates!.Min(x => x.Date);
                result.Sectors = responseEmployerSummary.GetContent()!.Sectors!;
            }

            return result;
        }
    }
}
