using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.FindAnApprenticeship.Application.Queries.SearchByVacancyReference;
using SFA.DAS.FindAnApprenticeship.InnerApi.RecruitApi.Requests;
using SFA.DAS.FindAnApprenticeship.InnerApi.RecruitApi.Responses;
using SFA.DAS.FindAnApprenticeship.InnerApi.Requests;
using SFA.DAS.FindAnApprenticeship.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FindAnApprenticeship.UnitTests.Application.Queries.SearchByVacancyReference
{
    public class WhenHandlingGetApprenticeshipVacancyQuery
    {
        [Test, MoqAutoData]
        public async Task Then_The_Services_Are_Called_And_Data_Returned_Based_On_Request(
            GetApprenticeshipVacancyQuery query,
            GetApprenticeshipVacancyItemResponse apiResponse,
            GetStandardsListItemResponse courseResponse,
            GetCourseLevelsListResponse courseLevelsResponse,
            [Frozen] Mock<IFindApprenticeshipApiClient<FindApprenticeshipApiConfiguration>> apiClient,
            [Frozen] Mock<ICoursesApiClient<CoursesApiConfiguration>> courseApiClient,
            [Frozen] Mock<ICourseService> courseService,
            GetApprenticeshipVacancyQueryHandler handler)
        {
            // Arrange
            var expectedRequest = new GetVacancyRequest(query.VacancyReference);

            courseApiClient
                .Setup(x => x.Get<GetStandardsListItemResponse>(
                    It.Is<GetStandardRequest>(c => c.StandardId.Equals(apiResponse.CourseId))))
                .ReturnsAsync(courseResponse);
            apiClient
                .Setup(client =>
                    client.Get<GetApprenticeshipVacancyItemResponse>(
                        It.Is<GetVacancyRequest>(r => r.GetUrl == expectedRequest.GetUrl)))
                .ReturnsAsync(apiResponse);
            courseService.Setup(x => x.GetLevels()).ReturnsAsync(courseLevelsResponse);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            result.ApprenticeshipVacancy.Should().BeEquivalentTo(apiResponse);
            result.CourseDetail.Should().BeEquivalentTo(courseResponse);
            result.Levels.Should().BeEquivalentTo(courseLevelsResponse.Levels);
        }

        [Test, MoqAutoData]
        public async Task Then_When_Vacancy_Is_Not_Found_Then_Returns_Closed_Vacancy_Instead(
            GetApprenticeshipVacancyQuery query,
            GetClosedVacancyResponse closedVacancyResponse,
            GetStandardsListItemResponse courseResponse,
            GetCourseLevelsListResponse courseLevelsResponse,
            int courseCode,
            [Frozen] Mock<IFindApprenticeshipApiClient<FindApprenticeshipApiConfiguration>> apiClient,
            [Frozen] Mock<IRecruitApiClient<RecruitApiConfiguration>> recruitApiClient,
            [Frozen] Mock<ICoursesApiClient<CoursesApiConfiguration>> courseApiClient,
            [Frozen] Mock<ICourseService> courseService,
            GetApprenticeshipVacancyQueryHandler handler)
        {
            // Arrange
            closedVacancyResponse.ProgrammeId = courseCode.ToString();

            var expectedRequest = new GetVacancyRequest(query.VacancyReference);

            courseApiClient
                .Setup(x => x.Get<GetStandardsListItemResponse>(
                    It.Is<GetStandardRequest>(c => c.StandardId.Equals(Convert.ToInt32(closedVacancyResponse.ProgrammeId)))))
                .ReturnsAsync(courseResponse);

            courseService.Setup(x => x.GetLevels()).ReturnsAsync(courseLevelsResponse);

            apiClient
                .Setup(client =>
                    client.Get<GetApprenticeshipVacancyItemResponse>(
                        It.Is<GetVacancyRequest>(r => r.GetUrl == expectedRequest.GetUrl)))
                .ReturnsAsync(() => null);

            var expectedRecruitApiRequest = new GetClosedVacancyRequest(query.VacancyReference);
            recruitApiClient.Setup(x =>
                    x.Get<GetClosedVacancyResponse>(
                        It.Is<GetClosedVacancyRequest>(x => x.GetUrl == expectedRecruitApiRequest.GetUrl)))
                .ReturnsAsync(closedVacancyResponse);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            result.ApprenticeshipVacancy.Should().BeEquivalentTo((GetApprenticeshipVacancyQueryResult.Vacancy)closedVacancyResponse);
            result.CourseDetail.Should().BeEquivalentTo(courseResponse);
            result.Levels.Should().BeEquivalentTo(courseLevelsResponse.Levels);
        }
    }
}