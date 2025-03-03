using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.Forecasting.InnerApi.Requests;
using SFA.DAS.Forecasting.InnerApi.Responses;
using SFA.DAS.Forecasting.Services;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Forecasting.Application.Approvals.Queries.GetApprenticeships;

public class GetApprenticeshipsQueryHandler(
    ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration> commitmentsV2Api,
    ICourseLookupService courseLookupService)
    : IRequestHandler<GetApprenticeshipsQuery, GetApprenticeshipsQueryResult>
{
    public async Task<GetApprenticeshipsQueryResult> Handle(GetApprenticeshipsQuery request, CancellationToken cancellationToken)
    {
        var courses = await courseLookupService.GetAllCourses();
        var apiRequest = new GetApprenticeshipsRequest(request.AccountId, request.Status, request.PageNumber, request.PageItemCount);
        var response = await commitmentsV2Api.Get<GetApprenticeshipsResponse>(apiRequest);

        return new GetApprenticeshipsQueryResult
        {
            TotalApprenticeshipsFound = response.TotalApprenticeshipsFound,
            Apprenticeships = response.Apprenticeships.Select(apprenticeship => new GetApprenticeshipsQueryResult.Apprenticeship
            {
                Id = apprenticeship.Id,
                TransferSenderId = apprenticeship.TransferSenderId,
                Uln = apprenticeship.Uln,
                ProviderId = apprenticeship.ProviderId,
                ProviderName = apprenticeship.ProviderName,
                FirstName = apprenticeship.FirstName,
                LastName = apprenticeship.LastName,
                CourseCode = apprenticeship.CourseCode,
                CourseName = apprenticeship.CourseName,
                CourseLevel = courses.FirstOrDefault(x => x.Id == apprenticeship.CourseCode)?.Level ?? 0,
                StartDate = apprenticeship.StartDate,
                EndDate = apprenticeship.EndDate,
                Cost = apprenticeship.Cost,
                PledgeApplicationId = apprenticeship.PledgeApplicationId,
                HasHadDataLockSuccess = apprenticeship.HasHadDataLockSuccess
            })
        };
    }
}