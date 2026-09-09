using MediatR;
using SFA.DAS.Apim.Shared.Extensions;
using SFA.DAS.Apim.Shared.Infrastructure;
using SFA.DAS.RecruitQa.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Types.Configuration;
using SFA.DAS.SharedOuterApi.Types.Interfaces;

namespace SFA.DAS.RecruitQa.Application.Report.Commands.PostCreateReport;

public class PostCreateReportCommandHandler(
    IRecruitApiClient<RecruitApiConfiguration> recruitApiClient) : IRequestHandler<PostCreateReportCommand>
{
    public async Task Handle(PostCreateReportCommand command, CancellationToken cancellationToken)
    {
        var response = await recruitApiClient.PostWithResponseCode<NullResponse>(new PostReportRequest(
            new PostReportRequest.PostReportRequestData
            {
                Id = command.Id,
                CreatedBy = command.CreatedBy,
                FromDate = command.FromDate,
                ToDate = command.ToDate,
                Name = command.Name,
                UserId = command.UserId
            }), false);

        response.EnsureSuccessStatusCode();
    }
}
