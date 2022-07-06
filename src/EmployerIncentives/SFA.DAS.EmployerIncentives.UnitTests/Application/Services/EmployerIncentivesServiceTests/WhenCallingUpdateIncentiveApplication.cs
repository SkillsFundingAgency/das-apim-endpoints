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
    public class WhenCallingUpdateIncentiveApplication
    {
        [Test, MoqAutoData]
        public async Task Then_The_InnerApi_Is_Called(
            UpdateIncentiveApplicationRequestData requestData,
            [Frozen] Mock<IEmployerIncentivesApiClient<EmployerIncentivesConfiguration>> client,
            ApplicationService sut)
        {
            await sut.Update(requestData);

            client.Verify(x =>
                x.Put(It.Is<UpdateIncentiveApplicationRequest>(
                    c => c.Data == requestData &&
                         c.PutUrl.Contains("application") && 
                         c.PutUrl.Contains(requestData.IncentiveApplicationId.ToString())
                )), Times.Once);
        }
    }
}