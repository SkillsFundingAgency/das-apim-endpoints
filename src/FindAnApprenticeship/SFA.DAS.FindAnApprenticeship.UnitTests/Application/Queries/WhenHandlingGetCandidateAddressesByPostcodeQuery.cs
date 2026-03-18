using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.FindAnApprenticeship.Application.Queries.GetCandidateAddressesByPostcode;
using SFA.DAS.SharedOuterApi.Types.Configuration;

using SFA.DAS.SharedOuterApi.Types.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Types.InnerApi.Requests.Courses;
using SFA.DAS.SharedOuterApi.Types.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Types.InnerApi.Responses.Courses;
using SFA.DAS.SharedOuterApi.Types.Interfaces;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Types.InnerApi.Requests.Location;
using SFA.DAS.SharedOuterApi.Types.InnerApi.Responses.Location;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FindAnApprenticeship.UnitTests.Application.Queries;
public class WhenHandlingGetCandidateAddressesByPostcodeQuery
{
    [Test, MoqAutoData]
    public async Task Then_Gets_Locations_From_Location_Api(
        GetCandidateAddressesByPostcodeQuery query,
        GetAddressesListResponse apiResponse,
        [Frozen] Mock<ILocationApiClient<LocationApiConfiguration>> mockApiClient,
        GetCandidateAddressesByPostcodeQueryHandler handler)
    {
        mockApiClient
            .Setup(client => client.Get<GetAddressesListResponse>(
                It.Is<GetAddressesQueryRequest>(c => c.GetUrl.Contains(query.Postcode))))
            .ReturnsAsync(apiResponse);

        var result = await handler.Handle(query, CancellationToken.None);

        result.Addresses.Count().Should().Be(apiResponse.Addresses.Count());
    }
}
