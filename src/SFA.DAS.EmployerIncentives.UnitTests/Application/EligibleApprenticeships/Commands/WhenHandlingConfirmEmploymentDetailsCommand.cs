using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerIncentives.Application.Commands.SaveApprenticeshipDetails;
using SFA.DAS.EmployerIncentives.InnerApi.Requests.ApprenticeshipDetails;
using SFA.DAS.EmployerIncentives.InnerApi.Requests.IncentiveApplication;
using SFA.DAS.EmployerIncentives.InnerApi.Responses.Commitments;
using SFA.DAS.EmployerIncentives.Interfaces;

namespace SFA.DAS.EmployerIncentives.UnitTests.Application.EligibleApprenticeships.Commands
{
    [TestFixture]
    public class WhenHandlingConfirmEmploymentDetailsCommand
    {
        [Test]
        public async Task Then_The_Service_Is_Called_With_The_Request_To_Confirm_Employment_Details()
        {
            var fixture = new Fixture();

            var request = new ApprenticeshipDetailsRequest
            {
                AccountId = fixture.Create<long>(), 
                ApplicationId = Guid.NewGuid(),
                ApprenticeshipDetails = new List<ApprenticeDetailsDto>()
            };
            request.ApprenticeshipDetails.Add(new ApprenticeDetailsDto { ApprenticeId = fixture.Create<long>(), EmploymentStartDate = fixture.Create<DateTime>()} );
            request.ApprenticeshipDetails.Add(new ApprenticeDetailsDto { ApprenticeId = fixture.Create<long>(), EmploymentStartDate = fixture.Create<DateTime>() });
            request.ApprenticeshipDetails.Add(new ApprenticeDetailsDto { ApprenticeId = fixture.Create<long>(), EmploymentStartDate = fixture.Create<DateTime>() });
            var apprenticeIds = request.ApprenticeshipDetails.Select(x => x.ApprenticeId).ToList();

            var command = new SaveApprenticeshipDetailsCommand(request);

            var commitmentsService = new Mock<ICommitmentsService>();
            var commitmentApprenticeships = new List<ApprenticeshipResponse>();
            foreach(var apprenticeId in apprenticeIds)
            {
                var commitment = new ApprenticeshipResponse {Id = apprenticeId, EmployerAccountId = request.AccountId, Uln = fixture.Create<long>()};
                commitmentApprenticeships.Add(commitment);
            }
            commitmentsService.Setup(x => x.GetApprenticeshipDetails(request.AccountId, apprenticeIds)).ReturnsAsync(commitmentApprenticeships.ToArray());

            var applicationService = new Mock<IApplicationService>();
            var handler = new SaveApprenticeshipDetailsCommandHandler(commitmentsService.Object, applicationService.Object);

            await handler.Handle(command, CancellationToken.None);

            commitmentsService.Verify(x => x.GetApprenticeshipDetails(
                    It.Is<long>(r => r == command.ApprenticeshipDetailsRequest.AccountId),
                    It.IsAny<IEnumerable<long>>()), Times.Once);

            applicationService.Verify(x => x.Update(
                    It.Is<UpdateIncentiveApplicationRequestData>(
                        r =>
                            r.AccountId == command.ApprenticeshipDetailsRequest.AccountId &&
                            r.IncentiveApplicationId == command.ApprenticeshipDetailsRequest.ApplicationId)),
                        Times.Once);
        }
    }
}
