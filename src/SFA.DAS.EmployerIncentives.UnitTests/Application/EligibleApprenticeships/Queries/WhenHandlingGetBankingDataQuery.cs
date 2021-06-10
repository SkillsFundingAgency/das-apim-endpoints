using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerIncentives.Application.Queries.GetBankingData;
using SFA.DAS.EmployerIncentives.InnerApi.Responses;
using SFA.DAS.EmployerIncentives.InnerApi.Responses.Accounts;
using SFA.DAS.EmployerIncentives.Interfaces;
using SFA.DAS.Testing.AutoFixture;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerIncentives.UnitTests.Application.EligibleApprenticeships.Queries
{
    public class WhenHandlingGetBankingDataQuery
    {
        [Test, MoqAutoData]
        public async Task Then_The_Application_Data_Is_Returned(
            GetBankingDataQuery query,
            IncentiveApplicationDto applicationResponse,
            [Frozen] Mock<IApplicationService> applicationService,
            GetBankingDataHandler handler
            )
        {
            applicationService.Setup(x => x.Get(query.AccountId, query.ApplicationId)).ReturnsAsync(applicationResponse);

            var actual = await handler.Handle(query, CancellationToken.None);

            actual.Data.SubmittedByEmail.Should().Be(applicationResponse.SubmittedByEmail);
            actual.Data.SubmittedByName.Should().Be(applicationResponse.SubmittedByName);
            actual.Data.LegalEntityId.Should().Be(applicationResponse.LegalEntityId);
            actual.Data.ApplicationValue.Should().Be(applicationResponse.Apprenticeships.Sum(x => x.TotalIncentiveAmount));
            actual.Data.VendorCode.Should().Be("00000000");
            actual.Data.NumberOfApprenticeships.Should().Be(applicationResponse.Apprenticeships.Count());
        }

        [Test, MoqAutoData]
        public async Task Then_The_Signed_Agreements_Are_Returned(
            GetBankingDataQuery query,
            IncentiveApplicationDto applicationResponse,
            LegalEntity legalEntityResponse,
            [Frozen] Mock<IApplicationService> applicationService,
            [Frozen] Mock<IAccountsService> accountsService,
            GetBankingDataHandler handler
        )
        {
            applicationService.Setup(x => x.Get(query.AccountId, query.ApplicationId)).ReturnsAsync(applicationResponse);
            accountsService.Setup(x => x.GetLegalEntity(query.HashedAccountId, applicationResponse.LegalEntityId)).ReturnsAsync(legalEntityResponse);

            var actual = await handler.Handle(query, CancellationToken.None);

            actual.Data.SignedAgreements.Should().BeEquivalentTo(legalEntityResponse.Agreements.Where(x => x.Status == EmployerAgreementStatus.Signed || x.Status == EmployerAgreementStatus.Expired || x.Status == EmployerAgreementStatus.Superseded), opts => opts.ExcludingMissingMembers());
        }

        [Test, MoqAutoData]
        public async Task Then_Agreements_Without_a_Signed_Date_Are_Not_Returned(
            GetBankingDataQuery query,
            IncentiveApplicationDto applicationResponse,
            LegalEntity legalEntityResponse,
            [Frozen] Mock<IApplicationService> applicationService,
            [Frozen] Mock<IAccountsService> accountsService,
            GetBankingDataHandler handler
        )
        {
            var agreementWithoutDate = legalEntityResponse.Agreements.First(x => x.Status == EmployerAgreementStatus.Signed || x.Status == EmployerAgreementStatus.Expired || x.Status == EmployerAgreementStatus.Superseded);
            agreementWithoutDate.SignedDate = null;
            applicationService.Setup(x => x.Get(query.AccountId, query.ApplicationId)).ReturnsAsync(applicationResponse);
            accountsService.Setup(x => x.GetLegalEntity(query.HashedAccountId, applicationResponse.LegalEntityId)).ReturnsAsync(legalEntityResponse);

            var actual = await handler.Handle(query, CancellationToken.None);

            actual.Data.SignedAgreements.Count().Should().Be(legalEntityResponse.Agreements.Count(x => x.Status == EmployerAgreementStatus.Signed || x.Status == EmployerAgreementStatus.Expired || x.Status == EmployerAgreementStatus.Superseded) - 1);
        }
    }
}