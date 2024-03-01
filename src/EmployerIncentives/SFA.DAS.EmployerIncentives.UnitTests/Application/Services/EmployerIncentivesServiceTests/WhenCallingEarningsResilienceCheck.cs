using System.Net;
using AutoFixture.NUnit3;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerIncentives.Application.Services;
using SFA.DAS.EmployerIncentives.Configuration;
using SFA.DAS.EmployerIncentives.InnerApi.Requests.EarningsResilienceCheck;
using SFA.DAS.EmployerIncentives.Interfaces;
using SFA.DAS.Testing.AutoFixture;
using System.Threading.Tasks;
using SFA.DAS.SharedOuterApi.Models;

namespace SFA.DAS.EmployerIncentives.UnitTests.Application.Services.EmployerIncentivesServiceTests
{
    public class WhenCallingEarningsResilienceCheck
    {
        [Test, MoqAutoData]
        public async Task Then_The_InnerApi_Is_Called(
            [Frozen] Mock<IEmployerIncentivesApiClient<EmployerIncentivesConfiguration>> client,
            EarningsResilienceCheckService service)
        {
            client.Setup(x =>
                x.PostWithResponseCode<string>(
                    It.IsAny<EarningsResilenceCheckRequest>(), false)).ReturnsAsync(
                new ApiResponse<string>(null, HttpStatusCode.Accepted, null));
            
            await service.RunCheck();

            client.Verify(x =>
                x.PostWithResponseCode<string>(It.IsAny<EarningsResilenceCheckRequest>(), false), Times.Once);
        }
    }
}