using System.Net;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.SharedOuterApi.Apprentice.GovUK.Auth.Application.Commands;
using SFA.DAS.SharedOuterApi.Apprentice.GovUK.Auth.InnerApi;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.SharedOuterApi.Apprentice.GovUK.Auth.UnitTests.Application;

public class WhenHandlingUpsertApprenticeCommand
{
    [Test, MoqAutoData]
    public async Task Then_The_Api_Is_Called_And_Apprentice_Returned(
        UpsertApprenticeCommand command,
        Models.Apprentice apprentice,
        [Frozen] Mock<IApprenticeAccountsApiClient<ApprenticeAccountsApiConfiguration>> apiClient,
        UpsertApprenticeCommandHandler handler)
    {
        apiClient.Setup(x => x.PutWithResponseCode<Models.Apprentice>(It.Is<PutApprenticeApiRequest>(
            c => ((PutApprenticeApiRequestData)c.Data).Email == command.Email
                 && ((PutApprenticeApiRequestData)c.Data).GovUkIdentifier == command.GovUkIdentifier
        ))).ReturnsAsync(new ApiResponse<Models.Apprentice>(apprentice, HttpStatusCode.OK, ""));

        var actual = await handler.Handle(command, CancellationToken.None);

        actual.Apprentice.Should().BeEquivalentTo(apprentice);
    }
}