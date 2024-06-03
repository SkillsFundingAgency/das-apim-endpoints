using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.FindAnApprenticeship.Application.Queries.CreateAccount.SignIntoYourOldAccount;
using SFA.DAS.FindAnApprenticeship.InnerApi.LegacyApi.Requests;
using SFA.DAS.FindAnApprenticeship.InnerApi.LegacyApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FindAnApprenticeship.UnitTests.Application.Queries.Candidate
{
    [TestFixture]
    public class WhenGettingSignIntoYourOldAccountQuery
    {
        [Test, MoqAutoData]
        public async Task Then_Validity_Of_Credentials_Is_Returned(
            GetSignIntoYourOldAccountQuery query,
            GetLegacyValidateCredentialsApiResponse apiResponse,
            [Frozen] Mock<IFindApprenticeshipLegacyApiClient<FindApprenticeshipLegacyApiConfiguration>> candidateAccountApiClient,
            GetSignIntoYourOldAccountQueryHandler handler)
        {
            var expectedApiRequest = new GetLegacyValidateCredentialsApiRequest(query.Email, query.Password);
            candidateAccountApiClient.Setup(x =>
                    x.Get<GetLegacyValidateCredentialsApiResponse>(
                        It.Is<GetLegacyValidateCredentialsApiRequest>(r => r.GetUrl == expectedApiRequest.GetUrl)))
                .ReturnsAsync(apiResponse);

            var result = await handler.Handle(query, CancellationToken.None);

            result.IsValid.Should().Be(apiResponse.IsValid);
        }
    }
}
