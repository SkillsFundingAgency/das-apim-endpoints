using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerAccounts.Application.Queries.GetEmployerAgreementTemplates;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.EmployerAccounts;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerAccounts.UnitTests.Application.Queries.GetEmployerAgreementTemplates;
[TestFixture]
public class WhenCallingHandler
{
    [Test, MoqAutoData]
    public async Task Then_Gets_English_Fraction_Current_From_Finance_Api(
        GetEmployerAgreementTemplatesQuery query,
        GetEmployerAgreementTemplatesResponse apiResponse,
        [Frozen] Mock<IAccountsApiClient<AccountsConfiguration>> accountsApiClientMock,
        GetEmployerAgreementTemplatesQueryHandler handler)
    {
        accountsApiClientMock
            .Setup(client => client.Get<GetEmployerAgreementTemplatesResponse>(It.Is<GetEmployerAgreementTemplatesRequest>(
            c =>
                c.GetUrl.Equals($"api/employeragreementtemplates"))))
            .ReturnsAsync(apiResponse);

        var result = await handler.Handle(query, CancellationToken.None);

        result.EmployerAgreementTemplates.Should().BeEquivalentTo(apiResponse.EmployerAgreementTemplates);
    }
}