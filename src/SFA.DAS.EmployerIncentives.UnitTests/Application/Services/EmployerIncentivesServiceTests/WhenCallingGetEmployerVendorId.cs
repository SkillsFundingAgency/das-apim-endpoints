using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerIncentives.Application.Services;
using SFA.DAS.EmployerIncentives.Configuration;
using SFA.DAS.EmployerIncentives.InnerApi.Requests.VendorRegistrationForm;
using SFA.DAS.EmployerIncentives.Interfaces;
using SFA.DAS.Testing.AutoFixture;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerIncentives.UnitTests.Application.Services.EmployerIncentivesServiceTests
{
    [TestFixture]
    public class WhenCallingGetEmployerVendorId
    {
        [Test, MoqAutoData]
        public async Task Then_The_Api_Is_Called_Returning_The_Vendor_Id(
            string hashedLegalEntityId,
            [Frozen] Mock<IEmployerIncentivesApiClient<EmployerIncentivesConfiguration>> client,
            EmployerIncentivesService service
        )
        {
            var vrfStatus = "Status";
            client.Setup(x =>
                    x.Get<string>(
                        It.Is<GetVrfVendorIdRequest>(c => c.HashedLegalEntityId == hashedLegalEntityId)))
                .ReturnsAsync(vrfStatus);

            var actual = await service.GetVrfVendorId(hashedLegalEntityId);

            actual.Should().BeEquivalentTo(vrfStatus);
        }
    }
}
