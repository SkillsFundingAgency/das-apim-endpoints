using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.Funding.Application.Queries.GetApprenticeships;
using SFA.DAS.Funding.InnerApi.Responses;
using SFA.DAS.Funding.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.Funding.UnitTests.Application.Queries.GetApprenticeships
{
    public class WhenHandlingGetApprenticeshipsQuery
    {
        [Test, MoqAutoData]
        public async Task Then_The_Apprenticeships_Are_Returned(
            GetApprenticeshipsQuery query,
            IEnumerable<ApprenticeshipDto> apprenticeshipResponse,
            [Frozen] Mock<IApprenticeshipsService> apprenticeshipsService,
            GetApprenticeshipsHandler handler
            )
        {
            apprenticeshipsService.Setup(x => x.GetAll(query.Ukprn)).ReturnsAsync(apprenticeshipResponse);

            var actual = await handler.Handle(query, CancellationToken.None);

            actual.Apprenticeships.Should().BeEquivalentTo(apprenticeshipResponse, opts => opts.ExcludingMissingMembers());
        }
    }
}