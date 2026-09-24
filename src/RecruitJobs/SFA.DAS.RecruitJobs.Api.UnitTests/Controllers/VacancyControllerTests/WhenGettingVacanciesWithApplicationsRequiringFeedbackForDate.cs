using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using Microsoft.AspNetCore.Http.HttpResults;
using SFA.DAS.Apim.Shared.Interfaces;
using SFA.DAS.Apim.Shared.Models;
using SFA.DAS.Recruit.Contracts.ApiRequests;
using SFA.DAS.Recruit.Contracts.ApiResponses;
using SFA.DAS.RecruitJobs.Api.Controllers;
using SFA.DAS.RecruitJobs.Api.Models;
using SFA.DAS.SharedOuterApi.Recruit.GraphQL;
using StrawberryShake;

namespace SFA.DAS.RecruitJobs.Api.UnitTests.Controllers.VacancyControllerTests;

public class WhenGettingVacanciesWithApplicationsRequiringFeedbackForDate
{
    [Test, MoqAutoData]
    public async Task Then_If_No_Vacancies_Found_On_Date_Ok_Is_Returned(
        DateTime date,
        Mock<IOperationResult<IGetClosedVacanciesBetweenDatesResult>> operationResult,
        Mock<IRecruitGqlClient> recruitGqlClient,
        [Greedy] VacanciesController sut)
    {
        // arrange
        operationResult.Setup(x => x.Data!.Vacancies).Returns([]);
        operationResult.Setup(x => x.Errors).Returns([]);
        
        recruitGqlClient
            .Setup(x => x.GetClosedVacanciesBetweenDates.ExecuteAsync(It.IsAny<VacancyEntityFilterInput>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(operationResult.Object);

        // act
        var result = await sut.GetVacanciesWithApplicationsRequiringFeedbackForDate(date, recruitGqlClient.Object, null, CancellationToken.None) as Ok<List<VacancyApplicationsCountRequiringFeedback>>;

        // assert
        result.Should().NotBeNull();
    }
    
    [Test, MoqAutoData]
    public async Task Then_Vacancies_Are_Returned(
        DateTime date,
        IReadOnlyList<IGetClosedVacanciesBetweenDates_Vacancies> gqlVacancies,
        Mock<IOperationResult<IGetClosedVacanciesBetweenDatesResult>> operationResult,
        DataResponse<List<VacancyApplicationsCountRequiringFeedback>> dataResponse,
        Mock<IRecruitGqlClient> recruitGqlClient,
        Mock<Recruit.Contracts.Client.IRecruitApiClient<Recruit.Contracts.Client.RecruitApiConfiguration>> recruitApiClient,
        [Greedy] VacanciesController sut)
    {
        // arrange
        operationResult.Setup(x => x.Data!.Vacancies).Returns(gqlVacancies);
        operationResult.Setup(x => x.Errors).Returns([]);
        
        VacancyEntityFilterInput? capturedFilter = null;
        recruitGqlClient
            .Setup(x => x.GetClosedVacanciesBetweenDates.ExecuteAsync(It.IsAny<VacancyEntityFilterInput>(), It.IsAny<CancellationToken>()))
            .Callback<VacancyEntityFilterInput, CancellationToken>((filter, _) => capturedFilter = filter)
            .ReturnsAsync(operationResult.Object);
        
        GetApplicationreviewsRequiringFeedbackByVacanciesApiRequest? capturedRequest = null;
        recruitApiClient
            .Setup(x => x.GetWithResponseCode<DataResponse<List<VacancyApplicationsCountRequiringFeedback>>>(It.IsAny<GetApplicationreviewsRequiringFeedbackByVacanciesApiRequest>()))
            .Callback<IGetApiRequest>(x => capturedRequest = x as GetApplicationreviewsRequiringFeedbackByVacanciesApiRequest)
            .ReturnsAsync(new ApiResponse<DataResponse<List<VacancyApplicationsCountRequiringFeedback>>>(dataResponse, HttpStatusCode.OK, null!));
        
        // act
        var result = await sut.GetVacanciesWithApplicationsRequiringFeedbackForDate(date, recruitGqlClient.Object, recruitApiClient.Object, CancellationToken.None) as Ok<List<VacancyApplicationsCountRequiringFeedback>>;

        // assert
        capturedFilter.Should().NotBeNull();
        capturedFilter.And.Should().NotBeNull();
        capturedFilter.And.Should().ContainEquivalentOf(new VacancyEntityFilterInput { ClosedDate = new DateTimeOperationFilterInput { Gte = date.Date } });
        capturedFilter.And.Should().ContainEquivalentOf(new VacancyEntityFilterInput { ClosedDate = new DateTimeOperationFilterInput { Lt = date.Date.AddDays(1) } });
        
        capturedRequest.Should().NotBeNull();
        capturedRequest.VacancyReferences.Should().BeEquivalentTo(gqlVacancies.Select(x => x.VacancyReference));
        
        result.Should().NotBeNull();
        result.Value.Should().BeEquivalentTo(dataResponse.Data);
    }
}