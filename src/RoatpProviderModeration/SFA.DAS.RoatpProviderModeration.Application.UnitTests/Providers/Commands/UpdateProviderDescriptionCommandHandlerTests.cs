using AutoFixture.NUnit3;
using Moq;
using NUnit.Framework;
using SFA.DAS.RoatpProviderModeration.Application.Infrastructure;
using SFA.DAS.RoatpProviderModeration.Application.InnerApi.Requests;
using SFA.DAS.RoatpProviderModeration.Application.Provider.Commands.UpdateProviderDescription;
using SFA.DAS.Testing.AutoFixture;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.RoatpProviderModeration.Application.UnitTests.Providers.Commands;

[TestFixture]
public class UpdateProviderDescriptionCommandHandlerTests
{
    [Test, MoqAutoData]
    public async Task Handle_CallsApiClient(
        [Frozen] Mock<IRoatpV2ApiClient> apiClientMock,
        UpdateProviderDescriptionCommandHandler sut,
        UpdateProviderDescriptionCommand command,
        CancellationToken cancellationToken)
    {
        apiClientMock.Setup(c => c.UpdateProviderDescription(command.Ukprn, It.IsAny<string>(), It.IsAny<string>(), It.IsAny<List<PatchOperation>>(), cancellationToken))
           .ReturnsAsync(new HttpResponseMessage { StatusCode = HttpStatusCode.Accepted, Version = new Version() });

        await sut.Handle(command, cancellationToken);
        apiClientMock.Verify(a => a.UpdateProviderDescription(command.Ukprn, It.IsAny<string>(), It.IsAny<string>(), It.IsAny<List<PatchOperation>>(), cancellationToken), Times.Once);
    }
}
