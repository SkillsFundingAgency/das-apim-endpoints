using System.Threading;
using System.Threading.Tasks;
using SFA.DAS.Approvals.Application.AgreementNotSigned.Queries;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.EmployerAccounts;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.EmployerAccounts;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Approvals.UnitTests.Application.AgreementNotSigned;

public class WhenGettingAgreementNotSigned
{
    [Test, MoqAutoData]
    public async Task Then_The_Api_To_GetSelectDirectTransferConnection_Returns_ExpectedValues(
        GetAgreementNotSignedQuery query,
        GetAccountByIdResponse accountResponse,
        [Frozen] Mock<IAccountsApiClient<AccountsConfiguration>> reservationsApiClient,
        GetAgreementNotSignedQueryHandler handler
    )
    {
        reservationsApiClient.Setup(x =>
                x.Get<GetAccountByIdResponse>(
                    It.Is<GetAccountByIdRequest>(x =>
                        x.AccountId == query.AccountId)))
            .ReturnsAsync(accountResponse);

        var actual = await handler.Handle(query, CancellationToken.None);

        actual.IsLevyAccount.Should().BeFalse();
    }
}