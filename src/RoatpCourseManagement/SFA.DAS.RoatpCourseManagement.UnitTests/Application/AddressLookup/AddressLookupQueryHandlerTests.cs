using AutoFixture.NUnit3;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.RoatpCourseManagement.Application.AddressLookup.Queries;
using SFA.DAS.SharedOuterApi.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Interfaces;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.RoatpCourseManagement.UnitTests.Application.AddressLookup
{
    [TestFixture]
    public class AddressLookupQueryHandlerTests
    {
        [Test, AutoData]
        public async Task Handle_CallsLocationService(GetAddressesListResponse data)
        {
            var postcode = "CV1 1ET";
            var expectedAddresses = data.Addresses.Select(a => (AddressItem)a).ToArray();
            var mockService = new Mock<ILocationLookupService>();
            mockService.Setup(m => m.GetExactMatchAddresses(postcode)).ReturnsAsync(data);
            var sut = new AddressLookupQueryHandler(mockService.Object, Mock.Of<ILogger<AddressLookupQueryHandler>>());

            var result = await sut.Handle(new AddresssLookupQuery(postcode), new CancellationToken());

            mockService.VerifyAll();
            result.Addresses.Should().BeEquivalentTo(expectedAddresses);
        }

        [Test, AutoData]
        public async Task Handle_BadPostcode_ReturnsNull()
        {
            var postcode = "CV1";
            var mockService = new Mock<ILocationLookupService>();
            mockService.Setup(m => m.GetExactMatchAddresses(postcode)).ReturnsAsync((GetAddressesListResponse)null);
            var sut = new AddressLookupQueryHandler(mockService.Object, Mock.Of<ILogger<AddressLookupQueryHandler>>());

            var result = await sut.Handle(new AddresssLookupQuery(postcode), new CancellationToken());

            mockService.VerifyAll();
            result.Should().BeNull();
        }
    }
}
