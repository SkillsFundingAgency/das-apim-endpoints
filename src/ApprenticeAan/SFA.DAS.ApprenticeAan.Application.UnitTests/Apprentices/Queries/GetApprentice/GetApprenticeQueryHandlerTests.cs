using AutoFixture.NUnit3;
using Moq;
using SFA.DAS.ApprenticeAan.Api.Configuration;
using SFA.DAS.ApprenticeAan.Application.Apprentices.Queries.GetApprentice;
using SFA.DAS.ApprenticeAan.Application.InnerApi.Apprentices;
using SFA.DAS.ApprenticeAan.Application.Services;

namespace SFA.DAS.ApprenticeAan.Application.UnitTests.Apprentices.Queries.GetApprentice;

public class GetApprenticeQueryHandlerTests
{
    [Test, AutoData]
    public async Task Handle_InvokesApiClient(GetApprenticeQuery query, CancellationToken cancellationToken)
    {
        Mock<IAanHubApiClient<AanHubApiConfiguration>> apiClientMock = new();
        GetApprenticeQueryHandler sut = new(apiClientMock.Object);

        await sut.Handle(query, cancellationToken);

        apiClientMock.Verify(c => c.Get<GetApprenticeQueryResult?>(It.Is<GetApprenticeRequest>(r => r.ApprenticeId == query.ApprenticeId)));
    }
}
