using System.Linq;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerIncentives.InnerApi.Responses;
using SFA.DAS.EmployerIncentives.Interfaces;
using SFA.DAS.Testing.AutoFixture;
using System.Threading;
using System.Threading.Tasks;
using SFA.DAS.EmployerIncentives.Application.Queries.GetBankingData;
using SFA.DAS.EmployerIncentives.InnerApi.Responses.Accounts;

namespace SFA.DAS.EmployerIncentives.UnitTests.Application.EligibleApprenticeships.Queries
{
    public class WhenHandlingGetBankingDataQuery
    {
        [Test, MoqAutoData]
        public async Task Then_The_Application_Data_Is_Returned(
            GetBankingDataQuery query,
            IncentiveApplicationDto applicationResponse,
            [Frozen] Mock<IEmployerIncentivesService> employerIncentivesService,
            GetBankingDataHandler handler
            )
        {
            employerIncentivesService.Setup(x => x.GetApplication(query.AccountId, query.ApplicationId)).ReturnsAsync(applicationResponse);

            var actual = await handler.Handle(query, CancellationToken.None);

            actual.Data.ApplicantEmail.Should().Be("TODO");
            actual.Data.ApplicantName.Should().Be("TODO");
            actual.Data.LegalEntityId.Should().Be(applicationResponse.LegalEntityId);
            actual.Data.ApplicationValue.Should().Be(applicationResponse.Apprenticeships.Sum(x => x.TotalIncentiveAmount));
            actual.Data.VendorCode.Should().Be("00000000");
        }

        [Test, MoqAutoData]
        public async Task Then_The_Signed_Agreements_Are_Returned(
            GetBankingDataQuery query,
            IncentiveApplicationDto applicationResponse,
            LegalEntity legalEntityResponse,
            [Frozen] Mock<IEmployerIncentivesService> employerIncentivesService,
            [Frozen] Mock<IAccountsService> accountsService,
            GetBankingDataHandler handler
        )
        {
            employerIncentivesService.Setup(x => x.GetApplication(query.AccountId, query.ApplicationId)).ReturnsAsync(applicationResponse);
            accountsService.Setup(x => x.GetLegalEntity(query.HashedAccountId, applicationResponse.LegalEntityId)).ReturnsAsync(legalEntityResponse);

            var actual = await handler.Handle(query, CancellationToken.None);

            actual.Data.SignedAgreements.Should().BeEquivalentTo(legalEntityResponse.Agreements.Where(x => x.Status == EmployerAgreementStatus.Signed || x.Status == EmployerAgreementStatus.Expired || x.Status == EmployerAgreementStatus.Superseded), opts => opts.ExcludingMissingMembers());
        }
    }
}