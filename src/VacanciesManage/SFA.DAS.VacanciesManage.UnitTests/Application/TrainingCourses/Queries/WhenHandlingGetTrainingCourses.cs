using System.Collections.Generic;
using System.Linq;
using System.Threading;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.Testing.AutoFixture;
using SFA.DAS.VacanciesManage.Application.TrainingCourses.Queries;
using SFA.DAS.VacanciesManage.InnerApi.Requests;
using SFA.DAS.VacanciesManage.InnerApi.Responses;

namespace SFA.DAS.VacanciesManage.UnitTests.Application.TrainingCourses.Queries;

public class WhenHandlingGetTrainingCourses
{
    [Test, MoqAutoData]
    public async Task And_Courses_Returned_From_Service(
        GetStandardsListResponse coursesFromCache,
        [Frozen] Mock<ICourseService> mockCacheService,
        GetTrainingCoursesQueryHandler handler)
    {
        mockCacheService
            .Setup(service => service.GetActiveStandards<GetStandardsListResponse>(nameof(GetStandardsListResponse)))
            .ReturnsAsync(coursesFromCache);

        var result = await handler.Handle(new GetTrainingCoursesQuery(), CancellationToken.None);

        result.TrainingCourses.Should().BeEquivalentTo(coursesFromCache.Standards);
    }
    
    [Test, MoqAutoData]
    public async Task Courses_Are_Filtered_To_The_Training_Provider(
        int ukprn,
        GetStandardsListResponse coursesFromCache,
        [Frozen] Mock<ICourseService> mockCacheService,
        [Frozen] Mock<IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration>> roatpCourseManagementApiClient,
        GetTrainingCoursesQueryHandler handler)
    {
        // arrange
        mockCacheService
            .Setup(service => service.GetActiveStandards<GetStandardsListResponse>(nameof(GetStandardsListResponse)))
            .ReturnsAsync(coursesFromCache);

        GetProviderAdditionalStandardsRequest? capturedRequest = null;
        roatpCourseManagementApiClient
            .Setup(x => x.Get<List<GetRoatpProviderAdditionalStandardsItem>>(It.IsAny<GetProviderAdditionalStandardsRequest>()))
            .Callback<IGetApiRequest>(x => capturedRequest = x as GetProviderAdditionalStandardsRequest)
            .ReturnsAsync([new GetRoatpProviderAdditionalStandardsItem() { LarsCode = coursesFromCache.Standards.First().LarsCode}]);
        
        // act
        var result = await handler.Handle(new GetTrainingCoursesQuery(ukprn), CancellationToken.None);

        // assert
        capturedRequest.Should().NotBeNull();
        capturedRequest!.GetUrl.Should().Be($"api/providers/{ukprn}/courses");
        
        result.TrainingCourses.Should().BeEquivalentTo([coursesFromCache.Standards.First()]);
    }
}