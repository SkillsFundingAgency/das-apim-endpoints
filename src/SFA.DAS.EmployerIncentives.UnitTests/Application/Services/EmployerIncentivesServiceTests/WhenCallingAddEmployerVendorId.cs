using AutoFixture.NUnit3;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerIncentives.Application.Services;
using SFA.DAS.EmployerIncentives.Configuration;
using SFA.DAS.EmployerIncentives.Interfaces;
using SFA.DAS.Testing.AutoFixture;
using System.Threading.Tasks;
using SFA.DAS.EmployerIncentives.InnerApi.Requests;

namespace SFA.DAS.EmployerIncentives.UnitTests.Application.Services.EmployerIncentivesServiceTests
{
    public class WhenCallingAddEmployerVendorId
    {
        [Test, MoqAutoData]
        public async Task Then_The_InnerApi_Is_Called(
            string hashedLegalEntityId,
            string employerVendorId,
            [Frozen] Mock<IEmployerIncentivesApiClient<EmployerIncentivesConfiguration>> client,
            VendorRegistrationService sut)
        {
            await sut.AddEmployerVendorIdToLegalEntity(hashedLegalEntityId, employerVendorId);

            client.Verify(x =>
                x.Put(It.Is<PutEmployerVendorIdForLegalEntityRequest>(
                    c => ((PutEmployerVendorIdForLegalEntityRequestData)c.Data).EmployerVendorId == employerVendorId &&
                          c.PutUrl.Contains(hashedLegalEntityId))
                ), Times.Once);
        }
    }
}