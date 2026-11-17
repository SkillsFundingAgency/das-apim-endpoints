using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.Approvals.Services;

namespace SFA.DAS.Approvals.Application.TrainingCourses.Queries;

public class GetApprenticeshipUnitVersionQueryHandler(IApprenticeshipUnitVersionService apprenticeshipUnitVersionService)
    : IRequestHandler<GetApprenticeshipUnitVersionQuery, GetApprenticeshipUnitVersionResult>
{
    public async Task<GetApprenticeshipUnitVersionResult> Handle(
        GetApprenticeshipUnitVersionQuery request, CancellationToken cancellationToken)
    {
        var version = await apprenticeshipUnitVersionService.Resolve(request.CourseCode, request.StartDate);
        return version == null ? null : new GetApprenticeshipUnitVersionResult(version.Version, version.StandardUId);
    }
}
