using System;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerIncentives.Application.Services;
using SFA.DAS.EmployerIncentives.Configuration;
using SFA.DAS.EmployerIncentives.InnerApi.Requests;
using SFA.DAS.EmployerIncentives.InnerApi.Responses;
using SFA.DAS.EmployerIncentives.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerIncentives.UnitTests.Application.Services.EmployerIncentivesServiceTests
{
    public class WhenCallingGetApplication
    {
        [Test, MoqAutoData]
        public async Task Then_The_Api_Is_Called_Returning_The_Application(
            long accountId,
            Guid applicationId,
            IncentiveApplicationDto apiResponse,
            [Frozen] Mock<IEmployerIncentivesApiClient<EmployerIncentivesConfiguration>> client,
            EmployerIncentivesService service
        )
        {
            client.Setup(x =>
                    x.Get<IncentiveApplicationDto>(
                        It.Is<GetApplicationRequest>(c => c.GetUrl.Contains(accountId.ToString()) && c.GetUrl.Contains(applicationId.ToString()))))
                .ReturnsAsync(apiResponse);

            var actual = await service.GetApplication(accountId, applicationId);

            actual.Should().BeEquivalentTo(apiResponse);
        }
    }
}