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
    public class WhenCallingConfirmIncentiveApplication
    {
        [Test, MoqAutoData]
        public async Task Then_The_InnerApi_Is_Called(
            ConfirmIncentiveApplicationRequest request,
            [Frozen] Mock<IEmployerIncentivesApiClient<EmployerIncentivesConfiguration>> client,
            EmployerIncentivesService service)
        {

            await service.ConfirmIncentiveApplication(request);

            client.Verify(x =>
                x.Patch(It.Is<ConfirmIncentiveApplicationRequest>(
                    c =>
                        c.PatchUrl.Contains(request.IncentiveApplicationId.ToString())
                )), Times.Once);
        }
    }
}