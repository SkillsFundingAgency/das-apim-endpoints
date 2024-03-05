using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerAccounts.Application.Commands.AddProviderDetailsFromInvitation;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.ProviderRegistrations;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.ProviderRelationships;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.ProviderRegistrations;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.ProviderRelationships;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerAccounts.UnitTests.Application.Commands.AddProviderDetailsFromInvitation
{
    public class WhenCallingHandle
    {
        [Test, MoqAutoData]
        public async Task And_Invitation_Exists_Then_Send_PostAddProviderDetailsFromInvitationRequest(
           [Frozen] Mock<IProviderRelationshipsApiClient<ProviderRelationshipsApiConfiguration>> mockPRelationsAPI,
           [Frozen] Mock<IProviderRegistrationsApiClient<ProviderRegistrationsApiConfiguration>> mockPRegistrationApi,
           GetInvitationResponse invitation,
           AddAccountProviderFromInvitationResponse addAccountProviderResponse,
           AddProviderDetailsFromInvitationCommand command,
           AddProviderDetailsFromInvitationHandler handler)
        {
            var relationApiResponse = new ApiResponse<AddAccountProviderFromInvitationResponse>(addAccountProviderResponse, System.Net.HttpStatusCode.OK, "");
            var invitationResponse = new ApiResponse<GetInvitationResponse>(invitation, System.Net.HttpStatusCode.OK, "");

            mockPRegistrationApi
                .Setup(m => m.GetWithResponseCode<GetInvitationResponse>(It.IsAny<GetInvitationRequest>()))
                .ReturnsAsync(invitationResponse);
           
            mockPRelationsAPI.Setup(m => m.PostWithResponseCode<AddAccountProviderFromInvitationResponse>(
                It.IsAny<PostAddProviderDetailsFromInvitationRequest>(), true))
                .ReturnsAsync(relationApiResponse);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            mockPRegistrationApi.Verify(m => m.GetWithResponseCode<GetInvitationResponse>(It.IsAny<GetInvitationRequest>()), Times.Once);
            mockPRelationsAPI.Verify(m => m.PostWithResponseCode<AddAccountProviderFromInvitationResponse>(It.IsAny<PostAddProviderDetailsFromInvitationRequest>(), true), Times.Once);
            result.Should().Be(Unit.Value);
        }

        [Test, MoqAutoData]
        public async Task And_Invitation_DoesNot_Exists_ShouldNotAddProviderRelationship(
           [Frozen] Mock<IProviderRelationshipsApiClient<ProviderRelationshipsApiConfiguration>> mockPRelationsAPI,
           [Frozen] Mock<IProviderRegistrationsApiClient<ProviderRegistrationsApiConfiguration>> mockPRegistrationApi,
           AddProviderDetailsFromInvitationCommand command,
           AddProviderDetailsFromInvitationHandler handler)
        {
            var invitationResponse = new ApiResponse<GetInvitationResponse>(null, System.Net.HttpStatusCode.OK, "");

            mockPRegistrationApi
                .Setup(m => m.GetWithResponseCode<GetInvitationResponse>(It.IsAny<GetInvitationRequest>()))
                .ReturnsAsync(invitationResponse);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            mockPRegistrationApi.Verify(m => m.GetWithResponseCode<GetInvitationResponse>(It.IsAny<GetInvitationRequest>()), Times.Once);
            mockPRelationsAPI.Verify(m => m.PostWithResponseCode<AddAccountProviderFromInvitationResponse>(It.IsAny<PostAddProviderDetailsFromInvitationRequest>(), true), Times.Never);
            result.Should().Be(Unit.Value);
        }
    }
}
