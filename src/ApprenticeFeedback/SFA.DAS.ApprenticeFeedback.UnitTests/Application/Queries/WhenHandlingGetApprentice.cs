using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.ApprenticeFeedback.Application.Queries.GetApprentice;
using SFA.DAS.ApprenticeFeedback.InnerApi.Requests;
using SFA.DAS.ApprenticeFeedback.InnerApi.Responses;
using SFA.DAS.ApprenticeFeedback.Models;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;
using SFA.DAS.Testing.AutoFixture;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.ApprenticeFeedback.UnitTests.Application.Queries
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
                    x.GetWithResponseCode<GetApprenticeResponse>(It.Is<GetApprenticeRequest>(x => x.Id == query.ApprenticeId)))
                .ReturnsAsync(new ApiResponse<GetApprenticeResponse>(apprenticeResponse, System.Net.HttpStatusCode.OK, string.Empty));
            apiClient.Setup(x =>
                    x.GetWithResponseCode<GetApprenticePreferencesResponse>(
                        It.Is<GetApprenticePreferencesRequest>(x => x.ApprenticeId == query.ApprenticeId)))
                .ReturnsAsync(new ApiResponse<GetApprenticePreferencesResponse>(apprenticePreferencesResponse, System.Net.HttpStatusCode.OK, string.Empty));

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
                    x.GetWithResponseCode<GetApprenticeResponse>(It.Is<GetApprenticeRequest>(x => x.Id == query.ApprenticeId)))
                .ReturnsAsync(new ApiResponse<GetApprenticeResponse>(null, System.Net.HttpStatusCode.OK, string.Empty));

            apiClient.Setup(x =>
                    x.GetWithResponseCode<GetApprenticePreferencesResponse>(
                        It.Is<GetApprenticePreferencesRequest>(x => x.ApprenticeId == query.ApprenticeId)))
                .ReturnsAsync(new ApiResponse<GetApprenticePreferencesResponse>(response, System.Net.HttpStatusCode.OK, string.Empty));

            var actual = await handler.Handle(query, CancellationToken.None);

            actual.Should().BeNull();
        }

        [Test]
        [MoqAutoData]
        public async Task Then_The_Api_Is_Called_With_The_Request_And_No_ApprenticePreferences_Is_Returned(
            GetApprenticeQuery query,
            [Frozen] Mock<IApprenticeAccountsApiClient<ApprenticeAccountsApiConfiguration>> apiClient,
            GetApprenticeQueryHandler handler,
            [Greedy] GetApprenticeResponse response)
        {
            apiClient.Setup(x =>
                    x.GetWithResponseCode<GetApprenticeResponse>(It.Is<GetApprenticeRequest>(x => x.Id == query.ApprenticeId)))
                .ReturnsAsync(new ApiResponse<GetApprenticeResponse>(response, System.Net.HttpStatusCode.OK, string.Empty));

            apiClient.Setup(x =>
                    x.GetWithResponseCode<GetApprenticePreferencesResponse>(
                        It.Is<GetApprenticePreferencesRequest>(x => x.ApprenticeId == query.ApprenticeId)))
                .ReturnsAsync(new ApiResponse<GetApprenticePreferencesResponse>(null, System.Net.HttpStatusCode.OK, string.Empty));

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
        public async Task Then_The_Api_Is_Called_With_The_Request_And_No_ApprenticePreferences_Nor_Apprentice_Is_Returned(
                GetApprenticeQuery query,
                [Frozen] Mock<IApprenticeAccountsApiClient<ApprenticeAccountsApiConfiguration>> apiClient,
                GetApprenticeQueryHandler handler)
        {
            apiClient.Setup(x =>
                    x.GetWithResponseCode<GetApprenticeResponse>(It.Is<GetApprenticeRequest>(x => x.Id == query.ApprenticeId)))
                .ReturnsAsync(new ApiResponse<GetApprenticeResponse>(null, System.Net.HttpStatusCode.OK, string.Empty));

            apiClient.Setup(x =>
                    x.GetWithResponseCode<GetApprenticePreferencesResponse>(
                        It.Is<GetApprenticePreferencesRequest>(x => x.ApprenticeId == query.ApprenticeId)))
                .ReturnsAsync(new ApiResponse<GetApprenticePreferencesResponse>(null, System.Net.HttpStatusCode.OK, string.Empty));

            var actual = await handler.Handle(query, CancellationToken.None);

            actual.Should().BeNull();
        }

        [Test]
        [MoqAutoData]
        public void Then_The_Api_Is_Called_With_The_Request_And_ApprenticeNotFound_ThrowsException(
        GetApprenticeQuery query,
        [Frozen] Mock<IApprenticeAccountsApiClient<ApprenticeAccountsApiConfiguration>> apiClient,
        GetApprenticeQueryHandler handler)
        {
            apiClient.Setup(x =>
                    x.GetWithResponseCode<GetApprenticeResponse>(It.Is<GetApprenticeRequest>(x => x.Id == query.ApprenticeId)))
                .ReturnsAsync(new ApiResponse<GetApprenticeResponse>(null, System.Net.HttpStatusCode.NotFound, string.Empty));

            Func<Task> invocation = async () => await handler.Handle(query, CancellationToken.None);

            invocation.Should().ThrowAsync<ApprenticeNotFoundException>();
        }
    }
}