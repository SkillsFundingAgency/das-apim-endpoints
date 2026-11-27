using SFA.DAS.Apim.Shared.Extensions;
using SFA.DAS.Apim.Shared.Infrastructure;
using SFA.DAS.Recruit.Contracts.ApiRequests;
using IRecruitApiClient = SFA.DAS.Recruit.Contracts.Client.IRecruitApiClient<SFA.DAS.Recruit.Contracts.Client.RecruitApiConfiguration>;

namespace SFA.DAS.RecruitQa.Application.Report.Commands.PostCreateReport;

public class PostCreateReportCommandHandler(IRecruitApiClient recruitApiClient) : IRequestHandler<PostCreateReportCommand>
{
    public async Task Handle(PostCreateReportCommand command, CancellationToken cancellationToken)
    {
        var response = await recruitApiClient.PostWithResponseCode<NullResponse>(new PostReportsApiRequest(new Recruit.Contracts.ApiResponses.PostReportRequest
        {
            Id = command.Id,
            CreatedBy = command.CreatedBy,
            FromDate = command.FromDate,
            ToDate = command.ToDate,
            Name = command.Name,
            UserId = command.UserId
        }));

        response.EnsureSuccessStatusCode();
    }
}