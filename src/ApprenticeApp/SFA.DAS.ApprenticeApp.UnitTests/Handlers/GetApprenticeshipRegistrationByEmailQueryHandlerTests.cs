using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.ApprenticeApp.Application.Queries.ApprenticeCommitments;
using SFA.DAS.Testing.AutoFixture;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.ApprenticeApp.UnitTests.Handlers
{
    public class GetApprenticeshipRegistrationByEmailQueryHandlerTests
    {
        [Test, MoqAutoData]
        public async Task GetApprenticeshipRegistrationByEmailQuery
        (GetApprenticeshipRegistrationByEmailQueryHandler sut,
        GetApprenticeshipRegistrationByEmailQuery query,
        CancellationToken cancellationToken)
        {
            query.Email = "test@test.com";

            await sut.Handle(query, cancellationToken);
            sut.Should().NotBeNull();
        }
    }
}
