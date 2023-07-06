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
            var response1 = await _commitmentsV2ApiClient.GetEmployerAccounts(request.employerAccontId, cancellationToken);
            //var response2 = await _commitmentsV2ApiClient.GetApprenticeshipsSummaryForEmployer(request.employerAccontId, cancellationToken);

            //var result = new GetEmployerMemberSummaryQueryResult() { ActiveCount = 100, StartDate = DateTime.Now, Sectors = new List<string>() };
            //var result = response.ResponseMessage.StatusCode switch
            //{
            //    HttpStatusCode.OK => response.GetContent(),
            //    HttpStatusCode.NotFound => null,
            //    _ => throw new InvalidOperationException($"Get employer member didn't some back with successful response")
            //};
            //return result;

            return new();
        }
    }
}
