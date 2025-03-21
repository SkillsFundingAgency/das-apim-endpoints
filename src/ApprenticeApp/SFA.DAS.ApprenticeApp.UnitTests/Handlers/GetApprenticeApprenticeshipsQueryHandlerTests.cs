using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.ApprenticeApp.Application.Queries.ApprenticeCommitments;
using SFA.DAS.Testing.AutoFixture;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.ApprenticeApp.UnitTests.Handlers
{
    public class GetApprenticeApprenticeshipsQueryHandlerTests
    {
        [Test, MoqAutoData]
        public async Task GetApprenticeApprenticeshipsQuery
         (GetApprenticeApprenticeshipsQueryHandler sut,
         GetApprenticeApprenticeshipsQuery query,
         CancellationToken cancellationToken)
        {
            query.ApprenticeId = Guid.NewGuid();

            await sut.Handle(query, cancellationToken);
            sut.Should().NotBeNull();
        }
    }
}