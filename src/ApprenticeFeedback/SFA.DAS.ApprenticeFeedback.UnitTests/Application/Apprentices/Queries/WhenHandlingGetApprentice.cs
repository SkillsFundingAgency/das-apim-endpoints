using System;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Moq;
using NUnit.Framework;
using SFA.DAS.ApprenticeFeedback.Application.Queries.GetApprentice;
using SFA.DAS.ApprenticeFeedback.InnerApi.Requests;
using SFA.DAS.ApprenticeFeedback.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.ApprenticeFeedback.UnitTests.Application.Apprentices.Queries
{
    public class WhenHandlingGetApprentice
    {
        [Test]
        [MoqAutoData]
        public async Task Then_TheApiIsCalledWithTheRequest_And_ReturnsApprentice(
            GetApprenticeQuery query,
            [Frozen] Mock<IApprenticeAccountsApiClient<ApprenticeAccountsApiConfiguration>> apiClient,
            GetApprenticeQueryHandler handler,
            GetApprenticeResponse apprenticeResponse,
            GetApprenticePreferencesResponse apprenticePreferencesResponse)
        {
            apiClient.Setup(x =>
                    x.Get<GetApprenticeResponse>(It.Is<GetApprenticeRequest>(x => x.Id == query.ApprenticeId)))
                .ReturnsAsync(apprenticeResponse);
            apiClient.Setup(x =>
                    x.Get<GetApprenticePreferencesResponse>(
                        It.Is<GetApprenticePreferencesRequest>(x => x.ApprenticeId == query.ApprenticeId)))
                .ReturnsAsync(apprenticePreferencesResponse);

            var actual = await handler.Handle(query, CancellationToken.None);

            actual.ApprenticeId.Should().Be(apprenticeResponse.ApprenticeId);
            actual.FirstName.Should().Be(apprenticeResponse.FirstName);
            actual.LastName.Should().Be(apprenticeResponse.LastName);
            actual.DateOfBirth.Should().Be(apprenticeResponse.DateOfBirth);
            actual.Email.Should().Be(apprenticeResponse.Email);
            actual.TermsOfUseAccepted.Should().Be(apprenticeResponse.TermsOfUseAccepted);
            actual.ReacceptTermsOfUseRequired.Should().Be(apprenticeResponse.ReacceptTermsOfUseRequired);
            actual.ApprenticePreferences.Should().BeEquivalentTo(apprenticePreferencesResponse.ApprenticePreferences);
        }

        [Test]
        [MoqAutoData]
        public async Task Then_The_Api_Is_Called_With_The_Request_And_No_Apprentice_Is_Returned(
            GetApprenticeQuery query,
            [Frozen] Mock<IApprenticeAccountsApiClient<ApprenticeAccountsApiConfiguration>> apiClient,
            GetApprenticeQueryHandler handler,
            [Greedy] GetApprenticePreferencesResponse response
        )
        {
            apiClient.Setup(x =>
                    x.Get<GetApprenticeResponse>(It.Is<GetApprenticeRequest>(x => x.Id == query.ApprenticeId)))
                .Returns(Task.FromResult((GetApprenticeResponse)null));

            apiClient.Setup(x =>
                    x.Get<GetApprenticePreferencesResponse>(
                        It.Is<GetApprenticePreferencesRequest>(x => x.ApprenticeId == query.ApprenticeId)))
                .Returns(Task.FromResult(response));

            var actual = await handler.Handle(query, CancellationToken.None);

            actual.Should().BeNull();
        }

        [Test]
        [MoqAutoData]
        public async Task Then_The_Api_Is_Called_With_The_Request_And_No_ApprenticePreferences_Is_Returned(
            GetApprenticeQuery query,
            [Frozen] Mock<IApprenticeFeedbackApiClient<ApprenticeAccountsApiConfiguration>> apiClient,
            GetApprenticeQueryHandler handler,
            [Greedy] GetApprenticeResponse response)
        {
            apiClient.Setup(x =>
                    x.Get<GetApprenticeResponse>(It.Is<GetApprenticeRequest>(x => x.Id == query.ApprenticeId)))
                .ReturnsAsync((response));

            apiClient.Setup(x =>
                    x.Get<GetApprenticePreferencesResponse>(
                        It.Is<GetApprenticePreferencesRequest>(x => x.ApprenticeId == query.ApprenticeId)))
                .ReturnsAsync((GetApprenticePreferencesResponse)null);

            var actual = await handler.Handle(query, CancellationToken.None);

            actual.ApprenticeId.Should().Be(response.ApprenticeId);
            actual.FirstName.Should().Be(response.FirstName);
            actual.LastName.Should().Be(response.LastName);
            actual.DateOfBirth.Should().Be(response.DateOfBirth);
            actual.Email.Should().Be(response.Email);
            actual.TermsOfUseAccepted.Should().Be(response.TermsOfUseAccepted);
            actual.ReacceptTermsOfUseRequired.Should().Be(response.ReacceptTermsOfUseRequired);
            actual.ApprenticePreferences.Should().BeEmpty();
        }

        [Test]
        [MoqAutoData]
        public async Task Then_The_Api_Is_Called_With_The_Request_And_No_ApprenticePreferences_Or_Apprentice_Is_Returned(
                GetApprenticeQuery query,
                [Frozen] Mock<IApprenticeFeedbackApiClient<ApprenticeAccountsApiConfiguration>> apiClient,
                GetApprenticeQueryHandler handler)
        {
            //var apprentice = _apiClient.Get<GetApprenticeResponse>(new GetApprenticeRequest(request.ApprenticeId));

            apiClient.Setup(x =>
                    x.Get<GetApprenticeResponse>(It.IsAny<GetApprenticeRequest>()))
                .Returns((Task<GetApprenticeResponse>)null);

            apiClient.Setup(x =>
                    x.Get<GetApprenticePreferencesResponse>(
                        It.Is<GetApprenticePreferencesRequest>(x => x.ApprenticeId == query.ApprenticeId)))
                .ReturnsAsync((GetApprenticePreferencesResponse)null);

            var actual = await handler.Handle(query, CancellationToken.None);

            actual.Should().BeNull();
        }
    }
}
//(x => x.Id == query.ApprenticeId)