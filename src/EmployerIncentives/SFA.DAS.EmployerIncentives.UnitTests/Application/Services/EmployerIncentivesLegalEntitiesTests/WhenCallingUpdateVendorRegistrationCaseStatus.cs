using AutoFixture.NUnit3;
using FluentAssertions.Common;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerIncentives.Application.Services;
using SFA.DAS.EmployerIncentives.Configuration;
using SFA.DAS.EmployerIncentives.InnerApi.Requests.VendorRegistrationForm;
using SFA.DAS.EmployerIncentives.Interfaces;
using SFA.DAS.Testing.AutoFixture;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerIncentives.UnitTests.Application.Services.EmployerIncentivesLegalEntitiesTests
{
    public class WhenCallingUpdateVendorRegistrationCaseStatus
    {
        [Test, MoqAutoData]
        public async Task Then_The_Api_Is_Called_To_Update_the_vendor_registration_case_status(
            UpdateVendorRegistrationCaseStatusRequest request,
            [Frozen] Mock<IEmployerIncentivesApiClient<EmployerIncentivesConfiguration>> client,
            VendorRegistrationService service)
        {
            await service.UpdateVendorRegistrationCaseStatus(request);

            client.Verify(x =>
                x.Patch(It.Is<PatchVendorRegistrationCaseStatusRequest>(
                    c =>
                        c.PatchUrl.Contains(request.HashedLegalEntityId) && c.Data.Equals(request)
                )), Times.Once
            );
        }
    }
}