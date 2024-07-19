using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.FindAnApprenticeship.Application.Queries.CreateAccount.Inform;
using SFA.DAS.FindAnApprenticeship.Domain.Models;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Requests;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FindAnApprenticeship.UnitTests.Application.Queries.Candidate
{
    [TestFixture]
    public class WhenGettingCreateAccountInformQuery
    {
        [Test, MoqAutoData]
        public async Task Then_Show_AccountRecoveryBanner_Is_False_When_MigratedEmail_Has_Been_Recorded_And_IsAccountCreated_Set(
            GetInformQuery query,
            GetCandidateApiResponse apiResponse,
            [Frozen] Mock<ICandidateApiClient<CandidateApiConfiguration>> candidateAccountApiClient,
            GetInformQueryHandler handler)
        {
            apiResponse.Status = UserStatus.Incomplete;
            var expectedApiRequest = new GetCandidateApiRequest(query.CandidateId.ToString());
            candidateAccountApiClient.Setup(x =>
                x.Get<GetCandidateApiResponse>(
                    It.Is<GetCandidateApiRequest>(r => r.GetUrl == expectedApiRequest.GetUrl)))
                .ReturnsAsync(apiResponse);

            var result = await handler.Handle(query, CancellationToken.None);

            result.ShowAccountRecoveryBanner.Should().Be(false);
            result.IsAccountCreated.Should().Be(false);
        }

        [Test, MoqAutoData]
        public async Task Then_Show_AccountRecoveryBanner_Is_True_When_MigratedEmail_Has_Not_Been_Recorded_And_IsAccountCreated_Set(
            GetInformQuery query,
            GetCandidateApiResponse apiResponse,
            [Frozen] Mock<ICandidateApiClient<CandidateApiConfiguration>> candidateAccountApiClient,
            GetInformQueryHandler handler)
        {
            apiResponse.MigratedEmail = null;
            apiResponse.Status = UserStatus.Completed;

            var expectedApiRequest = new GetCandidateApiRequest(query.CandidateId.ToString());
            candidateAccountApiClient.Setup(x =>
                    x.Get<GetCandidateApiResponse>(
                        It.Is<GetCandidateApiRequest>(r => r.GetUrl == expectedApiRequest.GetUrl)))
                .ReturnsAsync(apiResponse);

            var result = await handler.Handle(query, CancellationToken.None);

            result.ShowAccountRecoveryBanner.Should().Be(true);
            result.IsAccountCreated.Should().Be(true);
        }
    }
}
