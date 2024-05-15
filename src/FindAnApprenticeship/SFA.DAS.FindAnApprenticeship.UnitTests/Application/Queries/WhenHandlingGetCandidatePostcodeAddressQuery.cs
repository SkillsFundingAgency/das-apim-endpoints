using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.FindAnApprenticeship.Application.Queries.GetCandidatePostcodeAddress;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FindAnApprenticeship.UnitTests.Application.Queries;
public class WhenHandlingGetCandidatePostcodeAddressQuery
{
    [Test, MoqAutoData]
    public async Task Then_Gets_Locations_From_Location_Api(
        GetCandidatePostcodeAddressQuery query,
        GetLocationsListItem apiResponse,
        [Frozen] Mock<ILocationApiClient<LocationApiConfiguration>> mockApiClient,
        GetCandidatePostcodeAddressQueryHandler handler)
    {
        mockApiClient
            .Setup(client => client.Get<GetLocationsListItem>(
                It.Is<GetLocationByFullPostcodeRequest>(c => c.GetUrl.Contains(query.Postcode))))
            .ReturnsAsync(apiResponse);

        var result = await handler.Handle(query, CancellationToken.None);

        result.PostcodeExists.Should().BeTrue();
    }

    [Test, MoqAutoData]
    public async Task Then_No_Address_Returned_So_PostcodeExists_Is_False(
        GetCandidatePostcodeAddressQuery query,
        [Frozen] Mock<ILocationApiClient<LocationApiConfiguration>> mockApiClient,
        GetCandidatePostcodeAddressQueryHandler handler)
    {
        mockApiClient
            .Setup(client => client.Get<GetLocationsListItem>(
                It.Is<GetLocationByFullPostcodeRequest>(c => c.GetUrl.Contains(query.Postcode))))
            .ReturnsAsync(() => null);

        var result = await handler.Handle(query, CancellationToken.None);

        result.PostcodeExists.Should().BeFalse();
    }
}
