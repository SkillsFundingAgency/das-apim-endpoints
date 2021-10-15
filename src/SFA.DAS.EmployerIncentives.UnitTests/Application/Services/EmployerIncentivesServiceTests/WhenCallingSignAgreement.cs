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
    public class WhenCallingSignAgreement
    {
        [Test, MoqAutoData]
        public async Task Then_The_Api_Is_Called_To_Sign_The_Agreement(
            SignAgreementRequest request,
            [Frozen] Mock<IEmployerIncentivesApiClient<EmployerIncentivesConfiguration>> client,
            LegalEntitiesService service)
        {
            await service.SignAgreement(request);

            client.Verify(x =>
                x.Patch(It.Is<PatchSignAgreementRequest>(
                    c =>
                        c.PatchUrl.Contains(request.AccountId.ToString()) && c.PatchUrl.Contains(request.AccountLegalEntityId.ToString())
                                                                  && c.Data.IsSameOrEqualTo(request)
                )), Times.Once
            );
        }
    }
}