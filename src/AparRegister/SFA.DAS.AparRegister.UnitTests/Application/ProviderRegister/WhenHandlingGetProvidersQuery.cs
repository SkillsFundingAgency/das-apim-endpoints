using System.Net;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.AparRegister.Application.ProviderRegister.Queries;
using SFA.DAS.AparRegister.InnerApi.Requests;
using SFA.DAS.AparRegister.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.AparRegister.UnitTests.Application.ProviderRegister
{
    public class WhenHandlingGetProvidersQuery
    {
        [Test, MoqAutoData]
        public async Task Then_The_EndPoint_Is_Called_And_Provider_Data_Returned(
            GetProvidersQuery query,
            GetProvidersResponse apiResponse,
            [Frozen] Mock<IRoatpServiceApiClient<RoatpConfiguration>> apiClient,
            GetProvidersQueryHandler handler)
        {
            apiClient.Setup(x => x.GetWithResponseCode<GetProvidersResponse>(It.IsAny<GetProvidersRequest>()))
                .ReturnsAsync(new ApiResponse<GetProvidersResponse>(apiResponse, HttpStatusCode.OK, ""));

            var actual = await handler.Handle(query, CancellationToken.None);
            
            actual.RegisteredProviders.Should().BeEquivalentTo(apiResponse.RegisteredProviders);
        }

        [Test, MoqAutoData]
        public async Task Then_If_Not_Successful_Response_Then_Empty_List_Returned(
            GetProvidersQuery query,
            GetProvidersResponse apiResponse,
            [Frozen] Mock<IRoatpServiceApiClient<RoatpConfiguration>> apiClient,
            GetProvidersQueryHandler handler)
        {
            apiClient.Setup(x => x.GetWithResponseCode<GetProvidersResponse>(It.IsAny<GetProvidersRequest>()))
                .ReturnsAsync(new ApiResponse<GetProvidersResponse>(null, HttpStatusCode.InternalServerError, "Error"));

            var actual = await handler.Handle(query, CancellationToken.None);
            
            actual.RegisteredProviders.Should().BeEmpty();
        }
    }
}