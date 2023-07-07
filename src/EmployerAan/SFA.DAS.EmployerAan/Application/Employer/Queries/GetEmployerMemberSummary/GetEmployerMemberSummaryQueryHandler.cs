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
            var responseEmployerAccountTask = _commitmentsV2ApiClient.GetEmployerAccounts(request.EmployerAccountId, cancellationToken);
            var responseEmployerSummaryTask = _commitmentsV2ApiClient.GetApprenticeshipsSummaryForEmployer(request.EmployerAccountId, cancellationToken);

            await Task.WhenAll(responseEmployerAccountTask, responseEmployerSummaryTask);

            GetEmployerMemberSummaryQueryResult result = new();

            if (responseEmployerAccountTask.Result.ResponseMessage.StatusCode == System.Net.HttpStatusCode.OK)
            {
                result.ActiveCount = responseEmployerAccountTask.Result.GetContent()!.ApprenticeshipStatusSummaryResponse!.FirstOrDefault()!.ActiveCount;
            }

            if (responseEmployerSummaryTask.Result.ResponseMessage.StatusCode == System.Net.HttpStatusCode.OK)
            {
                result.StartDate = responseEmployerSummaryTask.Result.GetContent()!.StartDates!.Min(x => x.Date);
                result.Sectors = responseEmployerSummaryTask.Result.GetContent()!.Sectors!;
            }

            return result;
        }
    }
}
