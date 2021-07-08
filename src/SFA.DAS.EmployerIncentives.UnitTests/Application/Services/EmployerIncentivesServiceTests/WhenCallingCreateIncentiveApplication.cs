using AutoFixture.NUnit3;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerIncentives.Application.Services;
using SFA.DAS.EmployerIncentives.Configuration;
using SFA.DAS.EmployerIncentives.InnerApi.Requests.IncentiveApplication;
using SFA.DAS.EmployerIncentives.Interfaces;
using SFA.DAS.Testing.AutoFixture;
using System.Threading.Tasks;

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
            await sut.Create(requestData);

            client.Verify(x =>
                x.Post<CreateIncentiveApplicationRequestData>(It.Is<CreateIncentiveApplicationRequest>(
                    c => (CreateIncentiveApplicationRequestData)c.Data == requestData &&
                        c.PostUrl.Contains("application")
                )), Times.Once);
        }
    }
}