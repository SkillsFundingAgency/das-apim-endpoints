using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerIncentives.Application.Queries.EligibleApprenticeshipsSearch;
using SFA.DAS.EmployerIncentives.InnerApi.Responses;
using SFA.DAS.EmployerIncentives.InnerApi.Responses.Commitments;
using SFA.DAS.EmployerIncentives.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerIncentives.UnitTests.Application.EligibleApprenticeships.Queries
{
    public class WhenHandlingGetEligibleApprenticeshipsQuery
    {
        [Test, MoqAutoData]
        public async Task Then_The_Eligible_Apprenticeships_Are_Returned(
            GetEligibleApprenticeshipsSearchQuery query,
            GetApprenticeshipListResponse response,
            ApprenticeshipItem[] items,
            GetIncentiveDetailsResponse incentiveDetails,
            [Frozen] Mock<ICommitmentsService> commitmentsService,
            [Frozen] Mock<IEmployerIncentivesService> employerIncentivesService,
            [Frozen] Mock<IApprenticeshipService> apprenticeshipService,
            GetEligibleApprenticeshipsSearchHandler handler
            )
        {
            employerIncentivesService.Setup(x => x.GetIncentiveDetails()).ReturnsAsync(incentiveDetails);

            commitmentsService.Setup(x =>
                x.Apprenticeships(query.AccountId, query.AccountLegalEntityId, incentiveDetails.EligibilityStartDate, incentiveDetails.EligibilityEndDate))
                .ReturnsAsync(items);

            apprenticeshipService.Setup(x =>
                x.GetEligibleApprenticeships(It.Is<IEnumerable<ApprenticeshipItem>>(c => c.Count().Equals(response.Apprenticeships.Count())))).ReturnsAsync(items);

            var actual = await handler.Handle(query, CancellationToken.None);
            
            actual.Apprentices.Should().BeEquivalentTo(items);
        }
        
    }
}