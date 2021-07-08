using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerIncentives.Application.Queries.GetApplication;
using SFA.DAS.EmployerIncentives.Configuration;
using SFA.DAS.EmployerIncentives.InnerApi.Requests.Commitments;
using SFA.DAS.EmployerIncentives.InnerApi.Responses;
using SFA.DAS.EmployerIncentives.Interfaces;
using SFA.DAS.Testing.AutoFixture;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerIncentives.UnitTests.Application.EligibleApprenticeships.Queries
{
    public class WhenHandlingGetApplicationQuery
    {
        [Test, MoqAutoData]
        public async Task Then_The_Application_Is_Returned(
            GetApplicationQuery query,
            IncentiveApplicationDto applicationResponse,
            [Frozen] Mock<ICommitmentsApiClient<CommitmentsConfiguration>> commitmentsClient,
            [Frozen] Mock<IApplicationService> applicationService,
            GetApplicationHandler handler
            )
        {
            commitmentsClient.Setup(client => client.Get<GetApprenticeshipResponse>(It.IsAny<GetApprenticeshipRequest>()))
                .ReturnsAsync(new GetApprenticeshipResponse());

            applicationService.Setup(x => x.Get(query.AccountId, query.ApplicationId)).ReturnsAsync(applicationResponse);

            var actual = await handler.Handle(query, CancellationToken.None);

            actual.Application.Should().BeEquivalentTo(applicationResponse, opts => opts.ExcludingMissingMembers());
        }

        [Test, MoqAutoData]
        public async Task Then_The_Apprenticeship_Course_Is_Returned(
            GetApplicationQuery query,
            IncentiveApplicationDto applicationResponse,
            GetApprenticeshipResponse apprenticeshipResponse,
            [Frozen] Mock<ICommitmentsApiClient<CommitmentsConfiguration>> commitmentsClient,
            [Frozen] Mock<IApplicationService> applicationService,
            GetApplicationHandler handler
        )
        {
            apprenticeshipResponse.Id = applicationResponse.Apprenticeships.First().ApprenticeshipId;

            commitmentsClient
                .Setup(client => client.Get<GetApprenticeshipResponse>(It.IsAny<GetApprenticeshipRequest>()))
                .ReturnsAsync(new GetApprenticeshipResponse());
            commitmentsClient
                .Setup(client => client.Get<GetApprenticeshipResponse>(
                    It.Is<GetApprenticeshipRequest>(c => 
                        c.GetUrl.EndsWith($"/{applicationResponse.Apprenticeships.First().ApprenticeshipId}"))))
                .ReturnsAsync(apprenticeshipResponse);

            applicationService
                .Setup(x => x.Get(query.AccountId, query.ApplicationId))
                .ReturnsAsync(applicationResponse);

            var actual = await handler.Handle(query, CancellationToken.None);

            actual.Application.Apprenticeships.First().CourseName.Should().Be(apprenticeshipResponse.CourseName);
        }

        [Test, MoqAutoData]
        public async Task Then_The_Apprenticeships_Are_Not_Returned_If_Opted_Out(
            GetApplicationQuery query,
            IncentiveApplicationDto applicationResponse,
            GetApprenticeshipResponse apprenticeshipResponse,
            [Frozen] Mock<ICommitmentsApiClient<CommitmentsConfiguration>> commitmentsClient,
            [Frozen] Mock<IApplicationService> applicationService,
            GetApplicationHandler handler
        )
        {
            query.IncludeApprenticeships = false;

            applicationService.Setup(x => x.Get(query.AccountId, query.ApplicationId)).ReturnsAsync(applicationResponse);

            var actual = await handler.Handle(query, CancellationToken.None);

            actual.Application.Apprenticeships.Count().Should().Be(0);
            commitmentsClient.Verify(client => client.Get<GetApprenticeshipResponse>(It.IsAny<GetApprenticeshipRequest>()), Times.Never);
        }
    }
}