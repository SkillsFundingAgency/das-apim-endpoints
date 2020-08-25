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
            long accountId,
            long accountLegalEntityId,
            SignAgreementRequest request,
            [Frozen] Mock<IEmployerIncentivesApiClient<EmployerIncentivesConfiguration>> client,
            EmployerIncentivesService service)
        {
            await service.SignAgreement(accountId, accountLegalEntityId, request);

            client.Verify(x =>
                x.Patch(It.Is<PatchSignAgreementRequest>(
                    c =>
                        c.PatchUrl.Contains(accountId.ToString()) && c.PatchUrl.Contains(accountLegalEntityId.ToString())
                                                                  && c.Data.IsSameOrEqualTo(request)
                )), Times.Once
            );
        }
    }
}