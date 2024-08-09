using System.Net;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.ApprenticePortal.Application.Commands.ApprenticeAccounts;
using SFA.DAS.ApprenticePortal.InnerApi.ApprenticeAccounts.Requests;
using SFA.DAS.ApprenticePortal.Models;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.ApprenticePortal.UnitTests.Application.Queries;

public class WhenHandlingUpsertApprenticeCommand
{
    [Test, MoqAutoData]
    public async Task Then_The_Api_Is_Called_And_Apprentice_Returned(
        UpsertApprenticeCommand command,
        Apprentice apprentice,
        [Frozen] Mock<IApprenticeAccountsApiClient<ApprenticeAccountsApiConfiguration>> apiClient,
        UpsertApprenticeCommandHandler handler)
    {
        apiClient.Setup(x => x.PutWithResponseCode<Apprentice>(It.Is<PutApprenticeApiRequest>(
            c => ((PutApprenticeApiRequestData)c.Data).Email == command.Email
                 && ((PutApprenticeApiRequestData)c.Data).GovUkIdentifier == command.GovUkIdentifier
        ))).ReturnsAsync(new ApiResponse<Apprentice>(apprentice, HttpStatusCode.OK, ""));

        var actual = await handler.Handle(command, CancellationToken.None);

        actual.Apprentice.Should().BeEquivalentTo(apprentice);
    }
}