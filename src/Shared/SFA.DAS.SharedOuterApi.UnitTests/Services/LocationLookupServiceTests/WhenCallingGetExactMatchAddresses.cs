using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;
using SFA.DAS.SharedOuterApi.Services;
using System;
using System.Net;
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
            _locationApiClientMock.Verify(a => a.GetWithResponseCode<GetAddressesListResponse>(It.IsAny<GetAddressesQueryRequest>()), Times.Never);
        }

        [Test]
        public async Task When_Postcode_Is_Valid_Then_Calls_Api_With_Exact_Match_Param()
        {
            var expectedResponse = new GetAddressesListResponse();
            var apiResponse = new ApiResponse<GetAddressesListResponse>(expectedResponse, HttpStatusCode.OK, null);
            _locationApiClientMock.Setup(a => a.GetWithResponseCode<GetAddressesListResponse>(It.IsAny<GetAddressesQueryRequest>())).ReturnsAsync(apiResponse);

            var result = await _sut.GetExactMatchAddresses("CV1 2WT");
            _locationApiClientMock.Verify(a => a.GetWithResponseCode<GetAddressesListResponse>(It.IsAny<GetAddressesQueryRequest>()));
            result.Should().Be(expectedResponse);
        }

        [Test]
        public async Task When_Api_Does_Not_Return_Success_Code_Throws_Exception()
        {
            var expectedResponse = new GetAddressesListResponse();
            var apiResponse = new ApiResponse<GetAddressesListResponse>(expectedResponse, HttpStatusCode.BadRequest, null);
            _locationApiClientMock.Setup(a => a.GetWithResponseCode<GetAddressesListResponse>(It.IsAny<GetAddressesQueryRequest>())).ReturnsAsync(apiResponse);

            Func<Task> action = () => _sut.GetExactMatchAddresses("CV1 2WT");
            
            await action.Should().ThrowAsync<InvalidOperationException>();
            
        }
    }
}
