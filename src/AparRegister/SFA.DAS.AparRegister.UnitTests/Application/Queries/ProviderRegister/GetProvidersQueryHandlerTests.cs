using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.AparRegister.Application.Queries.ProviderRegister;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Exceptions;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.Roatp;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.Roatp;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.Roatp.Common;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.AparRegister.UnitTests.Application.Queries.ProviderRegister;

public class GetProvidersQueryHandlerTests
{
    [Test, MoqAutoData]
    public async Task Handler_InvokesApiClient(
        GetOrganisationsResponse apiResponse,
        GetProvidersQuery query,
        [Frozen] Mock<IRoatpServiceApiClient<RoatpConfiguration>> apiClient,
        GetProvidersQueryHandler handler)
    {
        apiClient.Setup(x => x.GetWithResponseCode<GetOrganisationsResponse>(It.IsAny<GetOrganisationsRequest>()))
          .ReturnsAsync(new ApiResponse<GetOrganisationsResponse>(apiResponse, HttpStatusCode.OK, ""));

        await handler.Handle(query, CancellationToken.None);

        apiClient.Verify(x => x.GetWithResponseCode<GetOrganisationsResponse>(It.IsAny<GetOrganisationsRequest>()));
    }

    [Test, MoqAutoData]
    public async Task Handler_ReturnsExpectedResponse(
        GetProvidersQuery query,
        [Frozen] Mock<IRoatpServiceApiClient<RoatpConfiguration>> apiClient,
        GetProvidersQueryHandler handler)
    {
        GetOrganisationsResponse apiResponse = new()
        {
            Organisations =
            [
                new OrganisationResponse
                {
                    Ukprn = 12345678,
                    LegalName = "Test Provider",
                    TradingName = "Test Trading Name",
                    ProviderType =  ProviderType.Employer,
                    Status = OrganisationStatus.Active
                },
                new OrganisationResponse
                {
                    Ukprn = 12345678,
                    LegalName = "Test Provider",
                    TradingName = "Test Trading Name",
                    ProviderType =  ProviderType.Main,
                    Status = OrganisationStatus.ActiveNoStarts
                },
                new OrganisationResponse
                {
                    Ukprn = 12345678,
                    LegalName = "Test Provider",
                    TradingName = "Test Trading Name",
                    ProviderType =  ProviderType.Supporting,
                    Status = OrganisationStatus.OnBoarding
                },
                new OrganisationResponse
                {
                    Ukprn = 12345678,
                    LegalName = "Test Provider",
                    TradingName = "Test Trading Name",
                    ProviderType =  ProviderType.Employer,
                    Status = OrganisationStatus.Removed
                }

            ]
        };

        apiClient.Setup(x => x.GetWithResponseCode<GetOrganisationsResponse>(It.IsAny<GetOrganisationsRequest>()))
            .ReturnsAsync(new ApiResponse<GetOrganisationsResponse>(apiResponse, HttpStatusCode.OK, ""));

        var actual = await handler.Handle(query, CancellationToken.None);

        actual.RegisteredProviders.Should().HaveCount(3);
        actual.RegisteredProviders.Should().NotContain(apiResponse.Organisations.Where(o => o.Status == OrganisationStatus.Removed));
    }

    [Test, MoqAutoData]
    public async Task Handler_ApiClientReturnsUnsuccessfulStatusCode_ThrowsException(
        GetProvidersQuery query,
        [Frozen] Mock<IRoatpServiceApiClient<RoatpConfiguration>> apiClient,
        GetProvidersQueryHandler handler)
    {
        apiClient.Setup(x => x.GetWithResponseCode<GetOrganisationsResponse>(It.IsAny<GetOrganisationsRequest>()))
            .ReturnsAsync(new ApiResponse<GetOrganisationsResponse>(null, HttpStatusCode.InternalServerError, "Error"));

        var action = () => handler.Handle(query, CancellationToken.None);

        await action.Should().ThrowAsync<ApiResponseException>();
    }
}
