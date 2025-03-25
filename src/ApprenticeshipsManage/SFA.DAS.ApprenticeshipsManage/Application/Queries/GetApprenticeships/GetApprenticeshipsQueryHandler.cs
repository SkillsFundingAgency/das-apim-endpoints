using MediatR;
using SFA.DAS.ApprenticeshipsManage.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.Apprenticeships;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.CollectionCalendar;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.CollectionCalendar;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.ApprenticeshipsManage.Application.Queries.GetApprenticeships;
public class GetApprenticeshipsQueryHandler(
    IApprenticeshipsApiClient<ApprenticeshipsApiConfiguration> apprenticeshipApiClient,
    ICollectionCalendarApiClient<CollectionCalendarApiConfiguration> collectionCalendarApiClient) 
    : IRequestHandler<GetApprenticeshipsQuery, GetApprenticeshipsQueryResult>
{
    public async Task<GetApprenticeshipsQueryResult> Handle(GetApprenticeshipsQuery request, CancellationToken cancellationToken)
    {
        var academicDatesResponse = await collectionCalendarApiClient.Get<GetAcademicYearsResponse>(new GetAcademicYearByDateRequest(request.AcademicYearDate));
        //apprenticeshipApiClient.GenerateServiceToken("ApprenticeshipsManage");
      
        var applicationsResponse = await apprenticeshipApiClient.Get<PagedApprenticeshipsResponse>(
            new GetAllApprenticeshipsByDatesRequest(
                request.Ukprn,         
                academicDatesResponse.StartDate.ToString("yyy-MM-dd"),
                academicDatesResponse.EndDate.ToString("yyy-MM-dd"),
                request.Page,
                request.PageSize
                )
            );

        return (GetApprenticeshipsQueryResult)applicationsResponse;
    }
}   