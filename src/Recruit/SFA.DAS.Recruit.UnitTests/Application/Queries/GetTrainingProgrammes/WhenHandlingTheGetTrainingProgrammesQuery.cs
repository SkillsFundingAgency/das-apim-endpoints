using SFA.DAS.Recruit.Application.Queries.GetTrainingProgrammes;
using SFA.DAS.Recruit.Domain;
using SFA.DAS.Recruit.InnerApi.Requests;
using SFA.DAS.Recruit.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.ProviderCoursesService;
using SFA.DAS.SharedOuterApi.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace SFA.DAS.Recruit.UnitTests.Application.Queries.GetTrainingProgrammes;

public class WhenHandlingTheGetTrainingProgrammesQuery
{
    [Test, MoqAutoData]
    public async Task Then_Returns_All_Standards_When_Ukprn_IsNull(
        GetTrainingProgrammesQuery query,
        GetStandardsListResponse apiResponse,
        List<GetStandardsListItem> apprenticeshipStandards,
        List<GetStandardsListItem> foundationStandards,
        [Frozen] Mock<ICourseService> mockCourseService,
        GetTrainingProgrammesQueryHandler handler)
    {
        query.Ukprn = null;

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

        var result = await handler.Handle(query, CancellationToken.None);

        result.TrainingProgrammes.Should().BeEquivalentTo(
            apiResponse.Standards.Select(item => (TrainingProgramme)item));
    }

    [Test, MoqAutoData]
    public async Task Then_Returns_Only_Standards_Offered_By_Provider_When_Filtering_By_Ukprn(
        GetTrainingProgrammesQuery query,
        GetStandardsListResponse apiResponse,
        List<GetStandardsListItem> allStandards,
        [Frozen] Mock<ICourseService> mockCourseService,
        [Frozen] Mock<IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration>> mockRoatpClient,
        GetTrainingProgrammesQueryHandler handler)
    {
        const int providerUkprn = 12345678;
        query.Ukprn = providerUkprn;

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

        var result = await handler.Handle(query, CancellationToken.None);

        var expected = allStandards
            .Where(s => offeredStandardCodes.Contains(s.LarsCode))
            .Select(s => (TrainingProgramme)s);

        result.TrainingProgrammes.Should().BeEquivalentTo(expected);
    }
}