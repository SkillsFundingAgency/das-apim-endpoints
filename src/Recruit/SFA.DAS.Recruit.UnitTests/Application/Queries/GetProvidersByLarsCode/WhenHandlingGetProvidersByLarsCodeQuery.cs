using System.Collections.Generic;
using System.Linq;
using System.Threading;
using AutoFixture;
using SFA.DAS.Recruit.Application.Queries.GetProvidersByLarsCode;
using SFA.DAS.Recruit.InnerApi.Requests;
using SFA.DAS.Recruit.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Recruit.UnitTests.Application.Queries.GetProvidersByLarsCode;

public class WhenHandlingGetProvidersByLarsCodeQuery
{
    [Test, MoqAutoData]
    public async Task Then_Providers_Are_Returned(
        GetProvidersByLarsCodeQuery query,
        List<ProviderData> providers,
        [Frozen] Mock<IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration>> roatpCourseManagementApiClient,
        [Greedy] GetProvidersByLarsCodeQueryHandler sut)
    {
        // arrange
        var response = new GetCourseProvidersApiResponse()
        {
            Providers = providers,
            TotalPages = 1
        };
        
        GetCourseProvidersByLarsCodeRequest? capturedRequest = null;
        roatpCourseManagementApiClient
            .Setup(x => x.Get<GetCourseProvidersApiResponse>(It.IsAny<GetCourseProvidersByLarsCodeRequest>()))
            .Callback<IGetApiRequest>(x => capturedRequest = x as GetCourseProvidersByLarsCodeRequest)
            .ReturnsAsync(response);

        // act
        var result = await sut.Handle(query, CancellationToken.None);

        // assert
        roatpCourseManagementApiClient.Verify(x => x.Get<GetCourseProvidersApiResponse>(It.IsAny<GetCourseProvidersByLarsCodeRequest>()), Times.Once);
        result.Should().NotBeNull();
        result.Providers.Should().BeEquivalentTo(providers, opt => opt.WithMapping<ProviderByLarsCodeItem>(x => x.ProviderName, x => x.Name));
        capturedRequest.Should().NotBeNull();
        capturedRequest!.LarsCode.Should().Be(query.LarsCode);
        capturedRequest.Page.Should().Be(1);
    }
    
    [Test, MoqAutoData]
    public async Task Then_Courses_Api_Is_Called_Multiple_Times_To_Return_All_The_Data_Required(
        GetProvidersByLarsCodeQuery query,
        IFixture fixture,
        [Frozen] Mock<IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration>> roatpCourseManagementApiClient,
        [Greedy] GetProvidersByLarsCodeQueryHandler sut)
    {
        // arrange
        var providers = fixture.CreateMany<ProviderData>(10).ToList();
        var response1 = new GetCourseProvidersApiResponse
        {
            Providers = providers.Take(5).ToList(),
            TotalPages = 2
        };
        var response2 = new GetCourseProvidersApiResponse
        {
            Providers = providers.Skip(5).ToList(),
            TotalPages = 2
        };
        
        roatpCourseManagementApiClient
            .SetupSequence(x => x.Get<GetCourseProvidersApiResponse>(It.IsAny<GetCourseProvidersByLarsCodeRequest>()))
            .ReturnsAsync(response1)
            .ReturnsAsync(response2);

        // act
        var result = await sut.Handle(query, CancellationToken.None);

        // assert
        roatpCourseManagementApiClient.Verify(x => x.Get<GetCourseProvidersApiResponse>(It.IsAny<GetCourseProvidersByLarsCodeRequest>()), Times.Exactly(2));
        result.Should().NotBeNull();
        result.Providers.Should().BeEquivalentTo(providers, opt => opt.WithMapping<ProviderByLarsCodeItem>(x => x.ProviderName, x => x.Name));
    }
}