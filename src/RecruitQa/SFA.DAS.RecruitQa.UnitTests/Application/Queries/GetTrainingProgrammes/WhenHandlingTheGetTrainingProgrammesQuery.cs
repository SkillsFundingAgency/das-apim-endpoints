using SFA.DAS.RecruitQa.Application.GetTrainingProgrammes;
using SFA.DAS.RecruitQa.Domain;
using SFA.DAS.RecruitQa.InnerApi.Requests;
using SFA.DAS.RecruitQa.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.ProviderCourses;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.ProviderCoursesService;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.RecruitQa.UnitTests.Application.Queries.GetTrainingProgrammes;

public class WhenHandlingTheGetTrainingProgrammesQuery
{
    [Test, MoqAutoData]
    public async Task Then_Returns_All_Standards_When_Ukprn_IsNull(
        GetStandardsListResponse apiResponse,
        List<GetStandardsListItem> apprenticeshipStandards,
        List<GetStandardsListItem> foundationStandards,
        [Frozen] Mock<ICourseService> mockCourseService,
        GetTrainingProgrammesQueryHandler handler)
    {
        foreach (var standard in apprenticeshipStandards)
        {
            standard.Level = (int)ApprenticeshipLevel.Advanced;
            standard.ApprenticeshipType = "Apprenticeship";
        }

        foreach (var standard in foundationStandards)
        {
            standard.Level = (int)ApprenticeshipLevel.Intermediate;
            standard.ApprenticeshipType = "Foundation";
        }

        apiResponse.Standards = apprenticeshipStandards.Concat(foundationStandards).ToList();

        mockCourseService
            .Setup(service => service.GetActiveStandards<GetStandardsListResponse>("ActiveStandards"))
            .ReturnsAsync(apiResponse);

        var result = await handler.Handle(new GetTrainingProgrammesQuery(null), CancellationToken.None);

        result._.Should().BeEquivalentTo(
            apiResponse.Standards.Select(item => (TrainingProgramme)item));
    }

    [Test, MoqAutoData]
    public async Task Then_Returns_Only_Standards_Offered_By_Provider_When_Filtering_By_Ukprn(
        GetStandardsListResponse apiResponse,
        List<GetStandardsListItem> allStandards,
        [Frozen] Mock<ICourseService> mockCourseService,
        [Frozen] Mock<IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration>> mockRoatpClient,
        GetTrainingProgrammesQueryHandler handler)
    {
        const int providerUkprn = 12345678;
        foreach (var standard in allStandards)
        {
            standard.ApprenticeshipType = "Apprenticeship";
            standard.Level = 4;
        }

        apiResponse.Standards = allStandards;

        mockCourseService
            .Setup(service => service.GetActiveStandards<GetStandardsListResponse>("ActiveStandards"))
            .ReturnsAsync(apiResponse);

        var offeredStandardCodes = allStandards
            .Take(2)
            .Select(s => s.LarsCode)
            .ToList();

        var providerCourses = offeredStandardCodes
            .Select(code => new ProviderCourse { LarsCode = code })
            .ToList();

        mockRoatpClient
            .Setup(client => client.Get<List<ProviderCourse>>(
                It.Is<GetAllProviderCoursesRequest>(r => r.GetUrl == $"api/providers/{providerUkprn}/courses")))
            .ReturnsAsync(providerCourses);

        var result = await handler.Handle(new GetTrainingProgrammesQuery(providerUkprn), CancellationToken.None);

        var expected = allStandards
            .Where(s => offeredStandardCodes.Contains(s.LarsCode))
            .Select(s => (TrainingProgramme)s);

        result._.Should().BeEquivalentTo(expected);
    }

    [Test, MoqAutoData]
    public async Task Then_If_Ukprn_Is_An_Employer_Provider_All_Training_Courses_Are_Returned(
        int ukprn,
        GetStandardsListResponse standardsListResponse,
        [Frozen] Mock<ICourseService> courseService,
        [Frozen] Mock<IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration>> roatpClient,
        GetTrainingProgrammesQueryHandler sut)
    {
        // arrange
        foreach (var standard in standardsListResponse.Standards)
        {
            standard.ApprenticeshipType = "Apprenticeship";
            standard.Level = 4;
        }
        var expectedProgrammes = standardsListResponse.Standards.Select(item => (TrainingProgramme)item);
        
        courseService
            .Setup(service => service.GetActiveStandards<GetStandardsListResponse>("ActiveStandards"))
            .ReturnsAsync(standardsListResponse);
        
        roatpClient
            .Setup(x => x.Get<GetProvidersListItem>(It.IsAny<GetProviderRequest>()))
            .ReturnsAsync(new GetProvidersListItem { ProviderTypeId = 2 } );

        // act
        var result = await sut.Handle(new GetTrainingProgrammesQuery(ukprn), CancellationToken.None);

        // assert
        result._.Should().BeEquivalentTo(expectedProgrammes);
        roatpClient.Verify(x => x.Get<List<ProviderCourse>>(It.IsAny<GetAllProviderCoursesRequest>()), Times.Never);
    }
}