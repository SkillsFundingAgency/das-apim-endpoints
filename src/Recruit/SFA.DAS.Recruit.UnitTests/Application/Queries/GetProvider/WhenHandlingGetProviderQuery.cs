using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.Recruit.Application.Queries.GetProvider;
using SFA.DAS.Recruit.InnerApi.Requests;
using SFA.DAS.Recruit.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.Testing.AutoFixture;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.Recruit.UnitTests.Application.Queries.GetProvider
{
    public class WhenHandlingGetProviderQuery
    {
        [Test, MoqAutoData]
        public async Task Then_The_Query_Is_Handled_And_Data_Returned(
            GetProviderQuery query,
            GetProvidersListItem apiResponse,
            [Frozen] Mock<IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration>> apiClient,
            GetProviderQueryHandler handler)
        {
            //Arrange
            var expectedGetUrl = new GetProviderRequest(query.UKprn);
            apiClient
                .Setup(x => x.Get<GetProvidersListItem>(
                    It.Is<GetProviderRequest>(c => c.GetUrl.Equals(expectedGetUrl.GetUrl)))).ReturnsAsync(apiResponse);
            
            //Act
            var actual = await handler.Handle(query, CancellationToken.None);
            
            //Assert
            actual.Provider.Should().BeEquivalentTo(apiResponse);
        }
        
        [Test, MoqAutoData]
        public async Task Then_If_NotFound_Response_Then_Null_Returned(
            GetProviderQuery query,
            [Frozen] Mock<IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration>> apiClient,
            GetProviderQueryHandler handler)
        {
            //Arrange
            apiClient
                .Setup(x => x.Get<GetProvidersListItem>(
                    It.IsAny<GetProviderRequest>()))!.ReturnsAsync((GetProvidersListItem)null);

            //Act
            var actual = await handler.Handle(query, CancellationToken.None);
            
            //Assert
            actual.Provider.Should().BeNull();
        }
    }
}