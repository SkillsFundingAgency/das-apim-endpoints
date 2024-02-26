using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.RoatpCourseManagement.InnerApi.Requests;
using SFA.DAS.RoatpCourseManagement.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;

namespace SFA.DAS.RoatpCourseManagement.Application.SectorSubjectArea.Queries.GetAllSectorSubjectAreaTier1;

public class GetAllSectorSubjectAreaTier1QueryHandler : IRequestHandler<GetAllSectorSubjectAreaTier1Query, ApiResponse<GetAllSectorSubjectAreaTier1Response>>
{
    private readonly ICoursesApiClient<CoursesApiConfiguration> _coursesApiClient;

    public GetAllSectorSubjectAreaTier1QueryHandler(ICoursesApiClient<CoursesApiConfiguration> coursesApiClient)
    {
        _coursesApiClient = coursesApiClient;
    }

    public async Task<ApiResponse<GetAllSectorSubjectAreaTier1Response>> Handle(GetAllSectorSubjectAreaTier1Query request, CancellationToken cancellationToken)
    {
        return await _coursesApiClient.GetWithResponseCode<GetAllSectorSubjectAreaTier1Response>(new GetAllSectorSubjectAreaTier1Request());
    }
}
