using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Services;
using System.Threading.Tasks;

namespace SFA.DAS.SharedOuterApi.UnitTests.Services.LocationLookupServiceTests
{
    [TestFixture]
    public class WhenCallingGetExactMatchAddresses
    {
        private Mock<ILocationApiClient<LocationApiConfiguration>> _locationApiClientMock;
        private LocationLookupService _sut;

        [SetUp]
        public void Before_Each_Test()
        {
            _locationApiClientMock = new Mock<ILocationApiClient<LocationApiConfiguration>>();
            _sut = new LocationLookupService(_locationApiClientMock.Object);
        }

        [Test]
        public async Task When_Postcode_Is_Invalid_Then_Returns_Null()
        {
            var result = await _sut.GetExactMatchAddresses("rubbish");
            Assert.Null(result);
            _locationApiClientMock.Verify(a => a.Get<GetAddressesListResponse>(It.IsAny<GetAddressesQueryRequest>()), Times.Never);
        }

        [Test]
        public async Task When_Postcode_Is_Valid_Then_Calls_Api_With_Exact_Match_Param()
        {
            var expectedResponse = new GetAddressesListResponse();
            _locationApiClientMock.Setup(a => a.Get<GetAddressesListResponse>(It.IsAny<GetAddressesQueryRequest>())).ReturnsAsync(expectedResponse);

            var result = await _sut.GetExactMatchAddresses("CV1 2WT");
            _locationApiClientMock.Verify(a => a.Get<GetAddressesListResponse>(It.IsAny<GetAddressesQueryRequest>()));
            result.Should().Be(expectedResponse);
        }
    }
}
