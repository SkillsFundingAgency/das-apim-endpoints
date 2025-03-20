using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.ApprenticeApp.Application.Queries.ApprenticeshipRegistration;
using SFA.DAS.Testing.AutoFixture;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.ApprenticeApp.UnitTests.Handlers
{
    public class GetApprenticeshipRegistrationQueryHandlerTests
    {
        [Test, MoqAutoData]
        public async Task GetApprenticeshipRegistrationQuery
          (GetApprenticeshipRegistrationQueryHandler sut,
          GetApprenticeshipRegistrationQuery query,
          CancellationToken cancellationToken)
        {
            query.ApprenticeshipId = 1;

            await sut.Handle(query, cancellationToken);
            sut.Should().NotBeNull();
        }
    }
}
