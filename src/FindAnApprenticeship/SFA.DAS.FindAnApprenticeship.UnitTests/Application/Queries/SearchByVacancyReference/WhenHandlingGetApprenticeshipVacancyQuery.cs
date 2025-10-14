using SFA.DAS.FindAnApprenticeship.Application.Queries.SearchByVacancyReference;
using SFA.DAS.FindAnApprenticeship.Domain.Models;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Requests;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Responses;
using SFA.DAS.FindAnApprenticeship.InnerApi.Responses;
using SFA.DAS.FindAnApprenticeship.Services;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Extensions;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.FindAnApprenticeship.UnitTests.Application.Queries.SearchByVacancyReference
{
    public class WhenHandlingGetApprenticeshipVacancyQuery
    {
        [Test, MoqAutoData]
        public async Task Then_The_Vacancy_And_Associated_Course_Data_Is_Returned(
            GetApprenticeshipVacancyQuery query,
            GetApprenticeshipVacancyItemResponse vacancy,
            GetStandardsListItemResponse courseResponse,
            GetCourseLevelsListResponse courseLevelsResponse,
            [Frozen] Mock<IVacancyService> vacancyService,
            [Frozen] Mock<ICoursesApiClient<CoursesApiConfiguration>> courseApiClient,
            [Frozen] Mock<ICourseService> courseService,
            GetApprenticeshipVacancyQueryHandler handler)
        {
            // Arrange
            query.CandidateId = null;
            vacancy.ClosedDate = null;
            vacancy.IsClosed = false;
            courseApiClient
                .Setup(x => x.Get<GetStandardsListItemResponse>(
                    It.Is<GetStandardRequest>(c => c.StandardId.Equals(vacancy.CourseId))))
                .ReturnsAsync(courseResponse);

            vacancyService
                .Setup(x => x.GetVacancy(query.VacancyReference))
                .ReturnsAsync(vacancy);

            courseService.Setup(x => x.GetLevels()).ReturnsAsync(courseLevelsResponse);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            result.ApprenticeshipVacancy.Should().BeEquivalentTo(vacancy, options => options
                .ExcludingMissingMembers()
                .Excluding(x => x.WageUnit));
            result.ApprenticeshipVacancy.WageUnit.Should().Be((int)vacancy.WageUnit!);
            result.CourseDetail.Should().BeEquivalentTo(courseResponse);
            result.Levels.Should().BeEquivalentTo(courseLevelsResponse.Levels);
            result.Application.Should().BeNull();
            result.ApprenticeshipVacancy.ClosingDate.Should().Be(vacancy.ClosedDate ?? vacancy.ClosingDate);
            result.IsSavedVacancy.Should().BeFalse();
        }

        [Test, MoqAutoData]
        public async Task Then_The_Vacancy_And_Candidate_Data_Is_Returned(
            GetApprenticeshipVacancyQuery query,
            GetApprenticeshipVacancyItemResponse vacancy,
            GetStandardsListItemResponse courseResponse,
            GetCourseLevelsListResponse courseLevelsResponse,
            GetApplicationByReferenceApiResponse applicationResponse,
            GetCandidateApiResponse candidateApiResponse,
            GetSavedVacancyApiResponse savedVacancyApiResponse,
            [Frozen] Mock<IVacancyService> vacancyService,
            [Frozen] Mock<ICoursesApiClient<CoursesApiConfiguration>> courseApiClient,
            [Frozen] Mock<ICourseService> courseService,
            [Frozen] Mock<ICandidateApiClient<CandidateApiConfiguration>> candidateApiClient,
            GetApprenticeshipVacancyQueryHandler handler)
        {
            // Arrange
            vacancy.ClosedDate = null;
            vacancy.IsClosed = false;
            courseApiClient
                .Setup(x => x.Get<GetStandardsListItemResponse>(
                    It.Is<GetStandardRequest>(c => c.StandardId.Equals(vacancy.CourseId))))
                .ReturnsAsync(courseResponse);

            vacancyService
                .Setup(x => x.GetVacancy(query.VacancyReference))
                .ReturnsAsync(vacancy);

            courseService.Setup(x => x.GetLevels()).ReturnsAsync(courseLevelsResponse);

            var expectedGetCandidateApplicationRequest =
                new GetApplicationByReferenceApiRequest(query.CandidateId.Value, query.VacancyReference.TrimVacancyReference());
            candidateApiClient
                .Setup(client =>
                    client.Get<GetApplicationByReferenceApiResponse>(
                        It.Is<GetApplicationByReferenceApiRequest>(c=>c.GetUrl == expectedGetCandidateApplicationRequest.GetUrl)))
                .ReturnsAsync(applicationResponse);
            
            var expectedGetCandidateAddressRequest = new GetCandidateApiRequest(query.CandidateId.Value.ToString());
            candidateApiClient
                .Setup(client =>
                    client.Get<GetCandidateApiResponse>(
                        It.Is<GetCandidateApiRequest>(c=>c.GetUrl == expectedGetCandidateAddressRequest.GetUrl)))
                .ReturnsAsync(candidateApiResponse);

            var expectedGetSavedVacancyApiRequest = new GetSavedVacancyApiRequest(query.CandidateId.Value, null, query.VacancyReference.TrimVacancyReference());
            candidateApiClient
                .Setup(client =>
                    client.Get<GetSavedVacancyApiResponse>(
                        It.Is<GetSavedVacancyApiRequest>(c => c.GetUrl == expectedGetSavedVacancyApiRequest.GetUrl)))
                .ReturnsAsync(savedVacancyApiResponse);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            result.ApprenticeshipVacancy.Should().BeEquivalentTo(vacancy, options => options
                .ExcludingMissingMembers()
                .Excluding(x => x.WageUnit));
            result.ApprenticeshipVacancy.WageUnit.Should().Be((int)vacancy.WageUnit!);
            result.CourseDetail.Should().BeEquivalentTo(courseResponse);
            result.Levels.Should().BeEquivalentTo(courseLevelsResponse.Levels);
            result.Application.Should().NotBeNull();
            result.Application.ApplicationId.Should().Be(applicationResponse.Id);
            result.Application.Status.Should().Be(applicationResponse.Status);
            result.Application.SubmittedDate.Should().Be(applicationResponse.SubmittedDate);
            result.ApprenticeshipVacancy.ClosingDate.Should().Be(vacancy.ClosedDate ?? vacancy.ClosingDate);
            result.CandidatePostcode.Should().Be(candidateApiResponse.Address.Postcode);
            result.CandidateDateOfBirth.Should().Be(candidateApiResponse.DateOfBirth);
            result.IsSavedVacancy.Should().BeTrue();
        }

        [Test, MoqAutoData]
        public async Task Then_The_Vacancy_Is_Closed_And_Associated_Course_Data_Is_Returned(
            GetApprenticeshipVacancyQuery query,
            GetApprenticeshipVacancyItemResponse vacancy,
            GetStandardsListItemResponse courseResponse,
            GetCourseLevelsListResponse courseLevelsResponse,
            [Frozen] Mock<IVacancyService> vacancyService,
            [Frozen] Mock<ICoursesApiClient<CoursesApiConfiguration>> courseApiClient,
            [Frozen] Mock<ICourseService> courseService,
            GetApprenticeshipVacancyQueryHandler handler)
        {
            // Arrange
            query.CandidateId = null;
            vacancy.ClosedDate = null;
            vacancy.IsClosed = false;
            vacancy.VacancySource = VacancyDataSource.Raa;
            courseApiClient
                .Setup(x => x.Get<GetStandardsListItemResponse>(
                    It.Is<GetStandardRequest>(c => c.StandardId.Equals(vacancy.CourseId))))
                .ReturnsAsync(courseResponse);

            vacancyService
                .Setup(x => x.GetVacancy(query.VacancyReference))
                .ReturnsAsync((GetApprenticeshipVacancyItemResponse)null!);

            vacancyService
                .Setup(x => x.GetClosedVacancy(query.VacancyReference))
                .ReturnsAsync(vacancy);

            courseService.Setup(x => x.GetLevels()).ReturnsAsync(courseLevelsResponse);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            result.ApprenticeshipVacancy.Should().BeEquivalentTo(vacancy, options => options
                .ExcludingMissingMembers()
                .Excluding(x => x.WageUnit));
            result.ApprenticeshipVacancy.WageUnit.Should().Be((int)vacancy.WageUnit!);
            result.CourseDetail.Should().BeEquivalentTo(courseResponse);
            result.Levels.Should().BeEquivalentTo(courseLevelsResponse.Levels);
            result.Application.Should().BeNull();
            result.ApprenticeshipVacancy.ClosingDate.Should().Be(vacancy.ClosedDate ?? vacancy.ClosingDate);
            result.IsSavedVacancy.Should().BeFalse();
        }

        [Test, MoqAutoData]
        public async Task If_CourseId_Is_Not_In_Correct_Format_Then_Return_Null(
            GetApprenticeshipVacancyQuery request,
            CancellationToken token,
            Mock<IVacancy> vacancy,
            [Frozen] Mock<IVacancyService> vacancyService,
            GetApprenticeshipVacancyQueryHandler sut)
        {
            // arrange
            vacancy.Setup(x => x.CourseId).Returns(-1);
            vacancyService.Setup(x => x.GetVacancy(request.VacancyReference)).ReturnsAsync((IVacancy)null!);
            vacancyService.Setup(x => x.GetClosedVacancy(request.VacancyReference)).ReturnsAsync(vacancy.Object);

            // act
            var result = await sut.Handle(request, token);

            // assert
            result.Should().BeNull();
            vacancy.Verify(x => x.CourseId, Times.Once);
        }
    }
}