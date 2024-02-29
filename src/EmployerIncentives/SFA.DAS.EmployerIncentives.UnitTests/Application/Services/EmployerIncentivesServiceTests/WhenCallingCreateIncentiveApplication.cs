using System.Net;
using AutoFixture.NUnit3;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerIncentives.Application.Services;
using SFA.DAS.EmployerIncentives.Configuration;
using SFA.DAS.EmployerIncentives.InnerApi.Requests.IncentiveApplication;
using SFA.DAS.EmployerIncentives.Interfaces;
using SFA.DAS.Testing.AutoFixture;
using System.Threading.Tasks;
using SFA.DAS.SharedOuterApi.Models;

namespace SFA.DAS.EmployerIncentives.UnitTests.Application.Services.EmployerIncentivesServiceTests
{
    public class WhenCallingCreateIncentiveApplication
    {
        [Test, MoqAutoData]
        public async Task Then_The_InnerApi_Is_Called(
            CreateIncentiveApplicationRequestData requestData,
            [Frozen] Mock<IEmployerIncentivesApiClient<EmployerIncentivesConfiguration>> client,
            ApplicationService sut)
        {
            client.Setup(x =>
                x.PostWithResponseCode<CreateIncentiveApplicationRequestData>(
                    It.IsAny<CreateIncentiveApplicationRequest>(), false)).ReturnsAsync(
                new ApiResponse<CreateIncentiveApplicationRequestData>(null, HttpStatusCode.Accepted, null));
            
            await sut.Create(requestData);

            client.Verify(x =>
                x.PostWithResponseCode<CreateIncentiveApplicationRequestData>(It.Is<CreateIncentiveApplicationRequest>(
                    c => (CreateIncentiveApplicationRequestData)c.Data == requestData &&
                        c.PostUrl.Contains("application")
                ), false), Times.Once);
        }
    }
}