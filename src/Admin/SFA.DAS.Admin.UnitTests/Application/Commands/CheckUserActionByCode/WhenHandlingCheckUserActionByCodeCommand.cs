using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.DigitalCertificates.Contracts.ApiRequests;
using SFA.DAS.DigitalCertificates.Contracts.ApiResponses;
using SFA.DAS.Apim.Shared.Models;
using SFA.DAS.DigitalCertificates.Contracts.Client;
using SFA.DAS.Testing.AutoFixture;
using SFA.DAS.Admin.Application.Commands.CheckUserActionByCode;
using System.Collections.Generic;

namespace SFA.DAS.Admin.UnitTests.Application.Commands.CheckUserActionByCode
{
	public class WhenHandlingCheckUserActionByCodeCommand
	{
		[Test, MoqAutoData]
		public async Task Then_AdminAction_Is_Posted_When_AdminActions_Empty(
			string code,
			CheckUserActionByCodeCommand command,
			UserActionDetail responseBody,
			[Frozen] Mock<IDigitalCertificatesApiClient<DigitalCertificatesApiConfiguration>> mockDigitalCertificatesApiClient,
			CheckUserActionByCodeCommandHandler handler)
		{
			// Arrange
			command.Code = code;
			command.Username = "tester";
			responseBody.AdminActions = new List<AdminActionDetail>();

			var apiResponse = new ApiResponse<UserActionDetail>(responseBody, HttpStatusCode.OK, string.Empty);

			mockDigitalCertificatesApiClient
				.Setup(c => c.GetWithResponseCode<UserActionDetail>(It.Is<GetUsersUseractionsByCodeApiRequest>(r => r.Code == code)))
				.ReturnsAsync(apiResponse);

			mockDigitalCertificatesApiClient
				.Setup(c => c.PostWithResponseCode<CreateAdminActionCommand>(It.IsAny<PostUsersAdminactionsApiRequest>()))
				.ReturnsAsync(new ApiResponse<CreateAdminActionCommand>(null, HttpStatusCode.OK, string.Empty));

			// Act
			var actual = await handler.Handle(command, CancellationToken.None);

			// Assert
			actual.Should().NotBeNull();
			actual.Id.Should().Be(responseBody.Id);

			mockDigitalCertificatesApiClient.Verify(c => c.PostWithResponseCode<CreateAdminActionCommand>(
				It.Is<PostUsersAdminactionsApiRequest>(p => ((CreateAdminActionCommand)p.Data).UserActionId == responseBody.Id && ((CreateAdminActionCommand)p.Data).Username == command.Username && ((CreateAdminActionCommand)p.Data).Action.ToString() == "Viewed")), Times.Once);

			mockDigitalCertificatesApiClient.Verify(c => c.GetWithResponseCode<UserActionDetail>(
				It.Is<GetUsersUseractionsByCodeApiRequest>(r => r.Code == code)), Times.Once);
		}

		[Test, MoqAutoData]
		public async Task Then_Does_Not_Post_If_MostRecent_Is_Same_User_Viewed(
			string code,
			CheckUserActionByCodeCommand command,
			UserActionDetail responseBody,
			[Frozen] Mock<IDigitalCertificatesApiClient<DigitalCertificatesApiConfiguration>> mockDigitalCertificatesApiClient,
			CheckUserActionByCodeCommandHandler handler)
		{
			// Arrange
			command.Code = code;
			command.Username = "tester";

			var action = new AdminActionDetail { Username = command.Username, ActionTime = DateTime.UtcNow, Action = AdminActionType.Viewed };
			responseBody.AdminActions = new List<AdminActionDetail> { action };

			var apiResponse = new ApiResponse<UserActionDetail>(responseBody, HttpStatusCode.OK, string.Empty);

			mockDigitalCertificatesApiClient
				.Setup(c => c.GetWithResponseCode<UserActionDetail>(It.IsAny<GetUsersUseractionsByCodeApiRequest>()))
				.ReturnsAsync(apiResponse);

			// Act
			var actual = await handler.Handle(command, CancellationToken.None);

			// Assert
			actual.Should().NotBeNull();

			mockDigitalCertificatesApiClient.Verify(c => c.PostWithResponseCode<CreateAdminActionCommand>(It.IsAny<PostUsersAdminactionsApiRequest>()), Times.Never);
		}

		[Test, MoqAutoData]
		public async Task Then_NotFound_Returns_Null_And_No_Post(
			string code,
			CheckUserActionByCodeCommand command,
			[Frozen] Mock<IDigitalCertificatesApiClient<DigitalCertificatesApiConfiguration>> mockDigitalCertificatesApiClient,
			CheckUserActionByCodeCommandHandler handler)
		{
			// Arrange
			command.Code = code;

			var apiResponse = new ApiResponse<UserActionDetail>(null, HttpStatusCode.NotFound, string.Empty);

			mockDigitalCertificatesApiClient
				.Setup(c => c.GetWithResponseCode<UserActionDetail>(It.IsAny<GetUsersUseractionsByCodeApiRequest>()))
				.ReturnsAsync(apiResponse);

			// Act
			var actual = await handler.Handle(command, CancellationToken.None);

			// Assert
			actual.Should().BeNull();

			mockDigitalCertificatesApiClient.Verify(c => c.PostWithResponseCode<CreateAdminActionCommand>(It.IsAny<PostUsersAdminactionsApiRequest>()), Times.Never);
		}

		[Test, MoqAutoData]
		public void Then_Exception_Is_Thrown_If_Post_AdminAction_Fails(
			string code,
			CheckUserActionByCodeCommand command,
			UserActionDetail responseBody,
			[Frozen] Mock<IDigitalCertificatesApiClient<DigitalCertificatesApiConfiguration>> mockDigitalCertificatesApiClient,
			CheckUserActionByCodeCommandHandler handler)
		{
			// Arrange
			command.Code = code;
			command.Username = "tester";
			responseBody.AdminActions = new List<AdminActionDetail>();

			var apiResponse = new ApiResponse<UserActionDetail>(responseBody, HttpStatusCode.OK, string.Empty);
			var postResponse = new ApiResponse<CreateAdminActionCommand>(null, HttpStatusCode.BadRequest, string.Empty);

			mockDigitalCertificatesApiClient
				.Setup(c => c.GetWithResponseCode<UserActionDetail>(It.Is<GetUsersUseractionsByCodeApiRequest>(r => r.Code == code)))
				.ReturnsAsync(apiResponse);

			mockDigitalCertificatesApiClient
				.Setup(c => c.PostWithResponseCode<CreateAdminActionCommand>(It.IsAny<PostUsersAdminactionsApiRequest>()))
				.ReturnsAsync(postResponse);

			// Act
			Func<Task> act = async () => await handler.Handle(command, CancellationToken.None);

			// Assert
			act.Should().ThrowAsync<Exception>();
		}

		[Test, MoqAutoData]
		public void Then_Exception_Is_Thrown_If_Api_CALL_Fails(
			CheckUserActionByCodeCommand command,
			[Frozen] Mock<IDigitalCertificatesApiClient<DigitalCertificatesApiConfiguration>> mockDigitalCertificatesApiClient,
			CheckUserActionByCodeCommandHandler handler)
		{
			// Arrange
			mockDigitalCertificatesApiClient
				.Setup(c => c.GetWithResponseCode<UserActionDetail>(It.IsAny<GetUsersUseractionsByCodeApiRequest>()))
				.ThrowsAsync(new Exception("Bad request"));

			// Act
			Func<Task> act = async () => await handler.Handle(command, CancellationToken.None);

			// Assert
			act.Should().ThrowAsync<Exception>();

			mockDigitalCertificatesApiClient.Verify(c => c.GetWithResponseCode<UserActionDetail>(It.IsAny<GetUsersUseractionsByCodeApiRequest>()), Times.Once);
		}
	}
}
