﻿using MediatR;
using SFA.DAS.Recruit.InnerApi.Recruit.Requests.Reports;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using System.Threading;
using System.Threading.Tasks;
using SFA.DAS.SharedOuterApi.Extensions;

namespace SFA.DAS.Recruit.Application.Report.Command.PostCreateReport;
public class PostCreateReportCommandHandler(
    IRecruitApiClient<RecruitApiConfiguration> recruitApiClient) : IRequestHandler<PostCreateReportCommand>
{
    public async Task Handle(PostCreateReportCommand command, CancellationToken cancellationToken)
    {
        var response = await recruitApiClient.PostWithResponseCode<Domain.Reports.Report>(new PostReportRequest(
            new PostReportRequest.PostReportRequestData
            {
                CreatedBy = command.CreatedBy,
                FromDate = command.FromDate,
                ToDate = command.ToDate,
                Name = command.Name,
                OwnerType = command.OwnerType,
                Ukprn = command.Ukprn,
                UserId = command.UserId
            }), true);

        response.EnsureSuccessStatusCode();
    }
}