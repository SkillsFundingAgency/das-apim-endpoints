using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions.Common;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerIncentives.Application.Services;
using SFA.DAS.EmployerIncentives.Configuration;
using SFA.DAS.EmployerIncentives.InnerApi.Requests;
using SFA.DAS.EmployerIncentives.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerIncentives.UnitTests.Application.EligibleApprenticeships.Services
{
    public class WhenCallingUpdateVendorRegistrationFormDetails
    {
        [Test, MoqAutoData]
        public async Task Then_The_Api_Is_Called_To_Update_the_vendor_registration_form_details(
            long legalEntityId,
            UpdateVendorRegistrationFormRequest request,
            [Frozen] Mock<IEmployerIncentivesApiClient<EmployerIncentivesConfiguration>> client,
            EmployerIncentivesService service)
        {
            await service.UpdateVendorRegistrationFormDetails(legalEntityId, request);

            client.Verify(x =>
                x.Patch(It.Is<PatchVendorRegistrationFormRequest>(
                    c =>
                        c.PatchUrl.Contains(legalEntityId.ToString()) && c.Data.IsSameOrEqualTo(request)
                )), Times.Once
            );
        }
    }
}