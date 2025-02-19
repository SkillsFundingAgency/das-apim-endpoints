using MediatR;
using SFA.DAS.FindApprenticeshipTraining.InnerApi.Requests;
using SFA.DAS.FindApprenticeshipTraining.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.FindApprenticeshipTraining.Application.AcademicYears.Queries.GetLatest;

public class GetAcademicYearsLatestQueryHandler(IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration> _roatpCourseManagementApiClient) : IRequestHandler<GetAcademicYearsLatestQuery, GetAcademicYearsLatestQueryResult>
{
    public async Task<GetAcademicYearsLatestQueryResult> Handle(GetAcademicYearsLatestQuery query, CancellationToken cancellationToken)
    {
        var result =
            await _roatpCourseManagementApiClient.Get<GetAcademicYearsLatestQueryResponse>(
                new GetAcademicYearsLatestRequest());

        return new GetAcademicYearsLatestQueryResult { QarPeriod = result.QarPeriod, ReviewPeriod = result.ReviewPeriod };
    }
}