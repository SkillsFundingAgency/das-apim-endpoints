using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture;
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
            GetEligibleApprenticeshipsSearchHandler handler
            )
        {
            employerIncentivesService.Setup(x => x.GetIncentiveDetails()).ReturnsAsync(incentiveDetails);

            commitmentsService.Setup(x =>
                x.Apprenticeships(query.AccountId, query.AccountLegalEntityId, incentiveDetails.EligibilityStartDate, incentiveDetails.EligibilityEndDate, query.PageNumber, query.PageSize))
                .ReturnsAsync(response);

            employerIncentivesService.Setup(x =>
                x.GetEligibleApprenticeships(It.Is<IEnumerable<ApprenticeshipItem>>(c => c.Count().Equals(response.Apprenticeships.Count())))).ReturnsAsync(items);

            var actual = await handler.Handle(query, CancellationToken.None);
            
            actual.Apprentices.Should().BeEquivalentTo(items);
        }
        
        [Test]
        public async Task Then_any_stopped_apprenticeships_are_filtered_out_of_the_eligibility_check()
        {
            // Arrange
            var fixture = new Fixture();
            var commitmentsService = new Mock<ICommitmentsService>();
            var employerIncentivesService = new Mock<IEmployerIncentivesService>();
            var handler = new GetEligibleApprenticeshipsSearchHandler(commitmentsService.Object, employerIncentivesService.Object);
            var incentiveDetails = fixture.Create<GetIncentiveDetailsResponse>();

            employerIncentivesService.Setup(x => x.GetIncentiveDetails()).ReturnsAsync(incentiveDetails);
            var query = fixture.Create<GetEligibleApprenticeshipsSearchQuery>();
            var apprenticeItems = fixture.CreateMany<ApprenticeshipItem>(5).ToList();
            apprenticeItems[0].ApprenticeshipStatus = ApprenticeshipStatus.Stopped;
            apprenticeItems[1].ApprenticeshipStatus = ApprenticeshipStatus.Live;
            apprenticeItems[2].ApprenticeshipStatus = ApprenticeshipStatus.Paused;
            apprenticeItems[3].ApprenticeshipStatus = ApprenticeshipStatus.WaitingToStart;
            apprenticeItems[4].ApprenticeshipStatus = ApprenticeshipStatus.Completed;
            var eligibleItems = apprenticeItems.Skip(1).Take(4);

            var response = new GetApprenticeshipListResponse 
            {
                Apprenticeships = apprenticeItems, 
                PageNumber = fixture.Create<int>(), 
                TotalApprenticeships = fixture.Create<int>()
            };
            commitmentsService.Setup(x =>
                    x.Apprenticeships(query.AccountId, query.AccountLegalEntityId, incentiveDetails.EligibilityStartDate, incentiveDetails.EligibilityEndDate, query.PageNumber, query.PageSize))
                .ReturnsAsync(response);

            employerIncentivesService.Setup(x =>
                x.GetEligibleApprenticeships(It.Is<IEnumerable<ApprenticeshipItem>>(c => c.Count().Equals(response.Apprenticeships.Count()-1)))).ReturnsAsync(eligibleItems.ToArray());

            // Act
            var actual = await handler.Handle(query, CancellationToken.None);

            // Assert
            actual.Apprentices.Count().Should().Be(apprenticeItems.Count-1);
        }

        [Test]
        public async Task Then_no_eligibility_checks_are_performed_if_all_apprenticeships_are_stopped()
        {
            // Arrange
            var fixture = new Fixture();
            var commitmentsService = new Mock<ICommitmentsService>();
            var employerIncentivesService = new Mock<IEmployerIncentivesService>();
            var handler = new GetEligibleApprenticeshipsSearchHandler(commitmentsService.Object, employerIncentivesService.Object);
            var incentiveDetails = fixture.Create<GetIncentiveDetailsResponse>();

            employerIncentivesService.Setup(x => x.GetIncentiveDetails()).ReturnsAsync(incentiveDetails);
            var query = fixture.Create<GetEligibleApprenticeshipsSearchQuery>();
            var apprenticeItems = fixture.CreateMany<ApprenticeshipItem>(5).ToList();
            apprenticeItems[0].ApprenticeshipStatus = ApprenticeshipStatus.Stopped;
            apprenticeItems[1].ApprenticeshipStatus = ApprenticeshipStatus.Stopped;
            apprenticeItems[2].ApprenticeshipStatus = ApprenticeshipStatus.Stopped;
            apprenticeItems[3].ApprenticeshipStatus = ApprenticeshipStatus.Stopped;
            apprenticeItems[4].ApprenticeshipStatus = ApprenticeshipStatus.Stopped;

            var response = new GetApprenticeshipListResponse
            {
                Apprenticeships = apprenticeItems,
                PageNumber = fixture.Create<int>(),
                TotalApprenticeships = fixture.Create<int>()
            };
            commitmentsService.Setup(x =>
                    x.Apprenticeships(query.AccountId, query.AccountLegalEntityId, incentiveDetails.EligibilityStartDate, incentiveDetails.EligibilityEndDate, query.PageNumber, query.PageSize))
                .ReturnsAsync(response);
            
            // Act
            var actual = await handler.Handle(query, CancellationToken.None);

            // Assert
            employerIncentivesService.Verify(x =>
                x.GetEligibleApprenticeships(It.IsAny<IEnumerable<ApprenticeshipItem>>()), Times.Never());
            actual.PageNumber.Should().Be(query.PageNumber);
            actual.TotalApprenticeships.Should().Be(response.TotalApprenticeships);
            actual.Apprentices.Should().BeEmpty();
        }
    }
}