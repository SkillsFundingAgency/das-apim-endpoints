using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.Approvals.InnerApi.CommitmentsV2Api.Requests.Courses;
using SFA.DAS.Approvals.InnerApi.CommitmentsV2Api.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Approvals.Application.Courses.Queries;

public class GetFundingBandQueryHandler : IRequestHandler<GetFundingBandQuery, GetFundingBandResult>
{
    private readonly ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration> _apiClient;

    public GetFundingBandQueryHandler(ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration> apiClient)
    {
        _apiClient = apiClient;
    }

    public async Task<GetFundingBandResult> Handle(GetFundingBandQuery request, CancellationToken cancellationToken)
    {
        var result = await _apiClient.Get<GetTrainingProgrammeResponse>(new GetCalculatedVersionOfTrainingProgrammeRequest(request.CourseCode, request.StartDate));

        if (result?.TrainingProgramme == null)
            return null;

        return new GetFundingBandResult
        {
            StandardUId = result.TrainingProgramme.StandardUId,
            Version = result.TrainingProgramme.Version,
            StandardPageUrl = result.TrainingProgramme.StandardPageUrl,
            ProposedMaxFunding = GetFundingBandForDate(result.TrainingProgramme.FundingPeriods, request.StartDate),
        };
    }

    private static int? GetFundingBandForDate(List<TrainingProgrammeFundingPeriod> bands, DateTime? forDate)
    {
        forDate ??= DateTime.Today;
        var match = bands.FirstOrDefault(x =>
            x.EffectiveFrom <= forDate && (x.EffectiveTo ?? DateTime.Today.AddYears(5)) >= forDate);
        return match?.FundingCap;
    }
}


