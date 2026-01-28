using MediatR;
using SFA.DAS.Recruit.InnerApi.Recruit.Requests.Reports;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Extensions;
using SFA.DAS.SharedOuterApi.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.Recruit.Application.Report.Command.PostCreateReport;
public class PostCreateReportCommandHandler(
    IRecruitApiClient<RecruitApiConfiguration> recruitApiClient) : IRequestHandler<PostCreateReportCommand>
{
    public async Task Handle(PostCreateReportCommand command, CancellationToken cancellationToken)
    {
        var response = await recruitApiClient.PostWithResponseCode<Domain.Reports.Report>(new PostReportRequest(
            new PostReportRequest.PostReportRequestData
            {
                Id = command.Id,
                CreatedBy = command.CreatedBy,
                FromDate = command.FromDate,
                ToDate = command.ToDate,
                Name = command.Name,
                OwnerType = command.OwnerType.ToString(),
                Ukprn = command.Ukprn,
                UserId = command.UserId
            }), true);

        response.EnsureSuccessStatusCode();
    }
}