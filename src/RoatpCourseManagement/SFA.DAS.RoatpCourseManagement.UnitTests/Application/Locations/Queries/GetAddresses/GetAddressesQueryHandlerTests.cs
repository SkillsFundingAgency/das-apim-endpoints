using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.RoatpCourseManagement.Application.AddressLookup.Queries;
using SFA.DAS.RoatpCourseManagement.Application.Locations.Queries.GetAddresses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.Testing.AutoFixture;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.RoatpCourseManagement.UnitTests.Application.Locations.Queries.GetAddresses;

[TestFixture]
public class GetAddressesQueryHandlerTests
{

    [Test]
    [MoqInlineAutoData("CV1 1FF", 1)]
    [MoqInlineAutoData("10 Downing Street, London", 0.1)]
    public async Task Handle_CallsInnerApi(
        string queryString,
        double minMatch,
        [Frozen] Mock<ILocationApiClient<LocationApiConfiguration>> apiClientMock,
        GetAddressesListResponse result,
        GetAddressesQueryHandler sut)
    {
        GetAddressesQuery query = new GetAddressesQuery(queryString);
        apiClientMock.Setup(c => c.Get<GetAddressesListResponse>(It.Is<GetAddressesQuery>(
            q => q.Query == queryString
            && q.GetUrl.Equals($"api/addresses?query={queryString}&MinMatch={minMatch}")
            ))).ReturnsAsync(result);
        var queryResult = await sut.Handle(query, new CancellationToken());
        var expectedAddresses = result.Addresses.Select(a => (AddressItem)a);

        queryResult.Addresses.Should().BeEquivalentTo(expectedAddresses);
    }
}
