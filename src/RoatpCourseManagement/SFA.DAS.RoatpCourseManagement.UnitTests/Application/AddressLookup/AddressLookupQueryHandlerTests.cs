using Moq;
using NUnit.Framework;
using SFA.DAS.RoatpCourseManagement.Application.AddressLookup;
using SFA.DAS.SharedOuterApi.Interfaces;
using System.Threading;

namespace SFA.DAS.RoatpCourseManagement.UnitTests.Application.AddressLookup
{
    [TestFixture]
    public class AddressLookupQueryHandlerTests
    {
        [Test]
        public void Handle_CallsLocationService()
        {
            var postcode = "CV1 1ET";
            var mockService = new Mock<ILocationLookupService>();
            var sut = new AddressLookupQueryHandler(mockService.Object);

            sut.Handle(new AddresssLookupQuery(postcode), new CancellationToken());

            mockService.Verify(m => m.GetExactMatchAddresses(postcode));
        }
    }
}
