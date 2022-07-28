using System;
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
using FluentAssertions;
using SFA.DAS.EmployerIncentives.Exceptions;
using SFA.DAS.SharedOuterApi.Models;

namespace SFA.DAS.EmployerIncentives.UnitTests.Application.Services.EmployerIncentivesServiceTests
{
    public class WhenCallingConfirmIncentiveApplication
    {
        [Test, MoqAutoData]
        public async Task Then_The_InnerApi_Is_Called(
            ConfirmIncentiveApplicationRequest request,
            [Frozen] Mock<IEmployerIncentivesApiClient<EmployerIncentivesConfiguration>> client,
            ApplicationService service)
        {
            client.Setup(x =>
                x.PatchWithResponseCode(It.Is<ConfirmIncentiveApplicationRequest>(
                    c =>
                        c.PatchUrl.Contains(request.Data.IncentiveApplicationId.ToString())
                ))).ReturnsAsync(new ApiResponse<string>("The Body", HttpStatusCode.OK, ""));

            await service.Confirm(request);

            client.Verify(x =>
                x.PatchWithResponseCode(It.Is<ConfirmIncentiveApplicationRequest>(
                    c =>
                        c.PatchUrl.Contains(request.Data.IncentiveApplicationId.ToString())
                )), Times.Once);
        }

        [Test, MoqAutoData]
        public void Then_An_Exception_Is_Thrown_When_The_InnerApi_Returns_A_Conflict(
            ConfirmIncentiveApplicationRequest request,
            [Frozen] Mock<IEmployerIncentivesApiClient<EmployerIncentivesConfiguration>> client,
            ApplicationService service)
        {
            client.Setup(x =>
                x.PatchWithResponseCode(It.Is<ConfirmIncentiveApplicationRequest>(
                    c =>
                        c.PatchUrl.Contains(request.Data.IncentiveApplicationId.ToString())
                ))).ReturnsAsync(new ApiResponse<string>("The Body", HttpStatusCode.Conflict, ""));

            Func<Task> action = async () => await service.Confirm(request);

            action.Should().Throw<UlnAlreadySubmittedException>();
        }
    }
}