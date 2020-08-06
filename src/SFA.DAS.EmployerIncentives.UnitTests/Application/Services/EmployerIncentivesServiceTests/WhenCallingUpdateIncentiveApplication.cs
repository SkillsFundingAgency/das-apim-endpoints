using AutoFixture.NUnit3;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerIncentives.Application.Services;
using SFA.DAS.EmployerIncentives.Configuration;
using SFA.DAS.EmployerIncentives.InnerApi.Requests;
using SFA.DAS.EmployerIncentives.Interfaces;
using SFA.DAS.Testing.AutoFixture;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerIncentives.UnitTests.Application.Services.EmployerIncentivesServiceTests
{
    public class WhenCallingUpdateIncentiveApplication
    {
        [Test, MoqAutoData]
        public async Task Then_The_InnerApi_Is_Called(
            UpdateIncentiveApplicationRequest request,
            [Frozen] Mock<IEmployerIncentivesApiClient<EmployerIncentivesConfiguration>> client,
            EmployerIncentivesService sut)
        {
            await sut.UpdateIncentiveApplication(request);

            client.Verify(x =>
                x.Put(It.Is<PutIncentiveApplicationRequest>(
                    c => (UpdateIncentiveApplicationRequest)c.Data == request &&
                         c.PutUrl.Contains("application")
                )), Times.Once);
        }
    }
}