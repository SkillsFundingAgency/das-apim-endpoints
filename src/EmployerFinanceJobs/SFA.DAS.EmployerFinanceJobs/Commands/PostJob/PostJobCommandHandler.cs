using MediatR;
using SFA.DAS.Apim.Shared.Extensions;
using SFA.DAS.Apim.Shared.Infrastructure;
using SFA.DAS.EmployerFinanceJobs.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Types.Configuration;
using SFA.DAS.SharedOuterApi.Types.Interfaces;

namespace SFA.DAS.EmployerFinanceJobs.Commands.PostJob;

public sealed class PostJobCommandHandler(
    IFundingProjectionApiClient<FundingProjectionApiConfiguration> fundingProjectionApiClient)
    : IRequestHandler<PostJobCommand, Unit>
{
    public async Task<Unit> Handle(PostJobCommand request, CancellationToken cancellationToken)
    {
        var response = await fundingProjectionApiClient.PostWithResponseCode<NullResponse>(
            new PostJobRequest(request.Id, new PostJobRequestData
            {
                AccountId = request.Data.AccountId,
                JobName = request.Data.JobName,
                LastSuccessfulImportDate = request.Data.LastSuccessfulImportDate,
                LastAttemptedDate = request.Data.LastAttemptedDate,
                LastAttemptSuccessful = request.Data.LastAttemptSuccessful,
                TotalRecordsLastRun = request.Data.TotalRecordsLastRun,
                FailedRecordsLastRun = request.Data.FailedRecordsLastRun
            }));

        response.EnsureSuccessStatusCode();
        return Unit.Value;
    }
}