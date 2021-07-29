using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerIncentives.Application.Queries.GetApprenticeshipIncentives;
using SFA.DAS.EmployerIncentives.InnerApi.Responses;
using SFA.DAS.EmployerIncentives.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerIncentives.UnitTests.Application.EligibleApprenticeships.Queries
{
    public class WhenHandlingGetApprenticeshipIncentivesQuery
    {
        [Test, MoqAutoData]
        public async Task Then_The_ApprenticeshipIncentives_Are_Returned(
            GetApprenticeshipIncentivesQuery query,
            ApprenticeshipIncentiveDto[] response,
            [Frozen] Mock<IEmployerIncentivesService> employerIncentivesService,
            GetApprenticeshipIncentivesHandler handler
        )
        {
            employerIncentivesService.Setup(x =>
                x.GetApprenticeshipIncentives(query.AccountId, query.AccountLegalEntityId)).ReturnsAsync(response);
            
            var actual = await handler.Handle(query, CancellationToken.None);
            
            actual.ApprenticeshipIncentives.Should().BeEquivalentTo(response);
        }
    }
}