using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.Admin.InnerApi.Requests;
using SFA.DAS.Admin.InnerApi.Responses;
using SFA.DAS.Apim.Shared.Models;
using SFA.DAS.SharedOuterApi.Types.Configuration;
using SFA.DAS.SharedOuterApi.Types.Interfaces;
using SFA.DAS.Testing.AutoFixture;
using SFA.DAS.Admin.Application.Commands.CheckUserActionByCode;

namespace SFA.DAS.Admin.UnitTests.Application.Commands.CheckUserActionByCode
{
	public class WhenHandlingCheckUserActionByCodeCommand
	{
		[Test, MoqAutoData]
		public async Task Then_AdminAction_Is_Posted_When_AdminActions_Empty(
			string code,
			CheckUserActionByCodeCommand command,
			GetUserActionByCodeResponse responseBody,
			[Frozen] Mock<IDigitalCertificatesApiClient<DigitalCertificatesApiConfiguration>> mockDigitalCertificatesApiClient,
			CheckUserActionByCodeCommandHandler handler)
		{
			// Arrange
			command.Code = code;
			command.Username = "tester";

			responseBody.AdminActions = new System.Collections.Generic.List<AdminAction>();

			var apiResponse = new ApiResponse<GetUserActionByCodeResponse>(responseBody, HttpStatusCode.OK, string.Empty);

			mockDigitalCertificatesApiClient
				.Setup(c => c.GetWithResponseCode<GetUserActionByCodeResponse>(It.Is<GetUserActionByCodeRequest>(r => r.Code == code)))
				.ReturnsAsync(apiResponse);

			mockDigitalCertificatesApiClient
				.Setup(c => c.PostWithResponseCode<PostAdminActionRequestData, object>(It.IsAny<PostAdminActionRequest>()))
				.ReturnsAsync(new ApiResponse<object>(null, HttpStatusCode.OK, string.Empty));

			// Act
			var actual = await handler.Handle(command, CancellationToken.None);

			// Assert
			actual.Should().NotBeNull();
			actual.Id.Should().Be(responseBody.Id);

			mockDigitalCertificatesApiClient.Verify(c => c.PostWithResponseCode<PostAdminActionRequestData, object>(
				It.Is<PostAdminActionRequest>(p => p.Data.UserActionId == responseBody.Id && p.Data.Username == command.Username && p.Data.Action == "Viewed")), Times.Once);

			mockDigitalCertificatesApiClient.Verify(c => c.GetWithResponseCode<GetUserActionByCodeResponse>(
				It.Is<GetUserActionByCodeRequest>(r => r.Code == code)), Times.Once);
		}

		[Test, MoqAutoData]
		public async Task Then_Does_Not_Post_If_MostRecent_Is_Same_User_Viewed(
			string code,
			CheckUserActionByCodeCommand command,
			GetUserActionByCodeResponse responseBody,
			[Frozen] Mock<IDigitalCertificatesApiClient<DigitalCertificatesApiConfiguration>> mockDigitalCertificatesApiClient,
			CheckUserActionByCodeCommandHandler handler)
		{
			// Arrange
			command.Code = code;
			command.Username = "tester";

			var action = new AdminAction { Username = command.Username, ActionTime = DateTime.UtcNow, Action = "Viewed" };
			responseBody.AdminActions = new System.Collections.Generic.List<AdminAction> { action };

			var apiResponse = new ApiResponse<GetUserActionByCodeResponse>(responseBody, HttpStatusCode.OK, string.Empty);

			mockDigitalCertificatesApiClient
				.Setup(c => c.GetWithResponseCode<GetUserActionByCodeResponse>(It.IsAny<GetUserActionByCodeRequest>()))
				.ReturnsAsync(apiResponse);

			// Act
			var actual = await handler.Handle(command, CancellationToken.None);

			// Assert
			actual.Should().NotBeNull();

			mockDigitalCertificatesApiClient.Verify(c => c.PostWithResponseCode<PostAdminActionRequestData, object>(It.IsAny<PostAdminActionRequest>()), Times.Never);
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

			var apiResponse = new ApiResponse<GetUserActionByCodeResponse>(null, HttpStatusCode.NotFound, string.Empty);

			mockDigitalCertificatesApiClient
				.Setup(c => c.GetWithResponseCode<GetUserActionByCodeResponse>(It.IsAny<GetUserActionByCodeRequest>()))
				.ReturnsAsync(apiResponse);

			// Act
			var actual = await handler.Handle(command, CancellationToken.None);

			// Assert
			actual.Should().BeNull();

			mockDigitalCertificatesApiClient.Verify(c => c.PostWithResponseCode<PostAdminActionRequestData, object>(It.IsAny<PostAdminActionRequest>()), Times.Never);
		}

		[Test, MoqAutoData]
		public void Then_Exception_Is_Thrown_If_Api_CALL_Fails(
			CheckUserActionByCodeCommand command,
			[Frozen] Mock<IDigitalCertificatesApiClient<DigitalCertificatesApiConfiguration>> mockDigitalCertificatesApiClient,
			CheckUserActionByCodeCommandHandler handler)
		{
			// Arrange
			mockDigitalCertificatesApiClient
				.Setup(c => c.GetWithResponseCode<GetUserActionByCodeResponse>(It.IsAny<GetUserActionByCodeRequest>()))
				.ThrowsAsync(new Exception("Bad request"));

			// Act
			Func<Task> act = async () => await handler.Handle(command, CancellationToken.None);

			// Assert
			act.Should().ThrowAsync<Exception>();

			mockDigitalCertificatesApiClient.Verify(c => c.GetWithResponseCode<GetUserActionByCodeResponse>(It.IsAny<GetUserActionByCodeRequest>()), Times.Once);
		}
	}
}
