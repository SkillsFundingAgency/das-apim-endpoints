using System.Net;
using System.Threading;
using SFA.DAS.Recruit.Application.Vacancies.Queries.GetVacancyById;
using SFA.DAS.Recruit.InnerApi.Mappers;
using SFA.DAS.Recruit.InnerApi.Recruit.Requests.Vacancies;
using SFA.DAS.Recruit.InnerApi.Recruit.Responses.Vacancies;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;

namespace SFA.DAS.Recruit.UnitTests.Application.Vacancies.Queries;

public class WhenGettingVacancyById
{
    [Test, MoqAutoData]
    public async Task None_Is_Returned_When_Vacancy_Not_Found(
        GetVacancyByIdQuery query,
        [Frozen] Mock<IRecruitApiClient<RecruitApiConfiguration>> recruitApiClient,
        [Greedy] GetVacancyByIdQueryHandler sut)
    {
        // arrange
        GetVacancyByIdRequest? capturedRequest = null;
        recruitApiClient
            .Setup(x => x.GetWithResponseCode<GetVacancyByIdResponse>(It.IsAny<GetVacancyByIdRequest>()))
            .Callback<IGetApiRequest>(x => capturedRequest = x as GetVacancyByIdRequest)
            .ReturnsAsync(new ApiResponse<GetVacancyByIdResponse>(null!, HttpStatusCode.NotFound, null));

        // act
        var result = await sut.Handle(query, CancellationToken.None);

        // assert
        result.Should().Be(GetVacancyByIdQueryResult.None);
        capturedRequest.Should().NotBeNull();
        capturedRequest!.Id.Should().Be(query.Id);
    }
    
    [Test, MoqAutoData]
    public async Task Then_The_Vacancy_Is_Returned(
        GetVacancyByIdQuery query,
        GetVacancyByIdResponse vacancyResponse,
        [Frozen] Mock<IRecruitApiClient<RecruitApiConfiguration>> recruitApiClient,
        [Greedy] GetVacancyByIdQueryHandler sut)
    {
        // arrange
        GetVacancyByIdRequest? capturedRequest = null;
        recruitApiClient
            .Setup(x => x.GetWithResponseCode<GetVacancyByIdResponse>(It.IsAny<GetVacancyByIdRequest>()))
            .Callback<IGetApiRequest>(x => capturedRequest = x as GetVacancyByIdRequest)
            .ReturnsAsync(new ApiResponse<GetVacancyByIdResponse>(vacancyResponse, HttpStatusCode.OK, null));

        // act
        var result = await sut.Handle(query, CancellationToken.None);

        // assert
        result.Vacancy.Should().BeEquivalentTo(vacancyResponse.ToDomain());
        capturedRequest.Should().NotBeNull();
        capturedRequest!.Id.Should().Be(query.Id);
    }
}