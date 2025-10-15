using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.ApprenticeApp.Api.Controllers;
using SFA.DAS.ApprenticeApp.Application.Queries.ApprenticeCommitments;
using SFA.DAS.ApprenticeApp.Application.Queries.ApprenticeshipRegistration;
using SFA.DAS.ApprenticeApp.Application.Queries.Details;
using SFA.DAS.ApprenticeApp.Models;
using SFA.DAS.ApprenticeApp.Telemetry;
using SFA.DAS.Testing.AutoFixture;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SFA.DAS.ApprenticeApp.UnitTests
{
    public class ApprenticeDetailsControllerTests
    {
        [Test, MoqAutoData]
        public async Task Get_Apprentice_Details_Test(
            [Frozen] Mock<IApprenticeAppMetrics> _apprenticeAppMetrics,
            [Frozen] Mock<IMediator> mediator,
            [Greedy] ApprenticeDetailsController controller)
        {
            var httpContext = new DefaultHttpContext();
            var apprenticeId = Guid.NewGuid();

            controller.ControllerContext = new ControllerContext
            {
                HttpContext = httpContext
            };

            mediator.Setup(m => m.Send(It.IsAny<GetApprenticeDetailsQuery>(), default)).ReturnsAsync(new GetApprenticeDetailsQueryResult
            {
                ApprenticeDetails = new ApprenticeDetails
                {
                    Apprentice = new Apprentice { ApprenticeId = apprenticeId },
                    MyApprenticeship = new MyApprenticeship { ApprenticeshipId = 1 }
                }
            });

            var result = await controller.GetApprenticeDetails(apprenticeId);
            result.Should().BeOfType(typeof(OkObjectResult));
        }

        [Test, MoqAutoData]
        public async Task Get_Apprentice_Details_NoApprenticeFound_Test(
            [Frozen] Mock<IMediator> mediator,
            [Frozen] Mock<IApprenticeAppMetrics> _apprenticeAppMetrics,
            [Greedy] ApprenticeDetailsController controller)
        {
            var httpContext = new DefaultHttpContext();
            var apprenticeId = Guid.NewGuid();

            controller.ControllerContext = new ControllerContext
            {
                HttpContext = httpContext
            };

            mediator.Setup(m => m.Send(It.IsAny<GetApprenticeDetailsQuery>(), default)).ReturnsAsync(new GetApprenticeDetailsQueryResult
            {
                ApprenticeDetails = new ApprenticeDetails
                {
                    Apprentice = null,
                    MyApprenticeship = new MyApprenticeship { ApprenticeshipId = 1 }
                }
            });
            var result = await controller.GetApprenticeDetails(apprenticeId);
            result.Should().BeOfType(typeof(NotFoundResult));
        }

        [Test, MoqAutoData]
        public async Task Get_ApprenticeApprenticeship_NoApprenticeshipsFound_Test(
            [Frozen] Mock<IMediator> mediator,
            [Greedy] ApprenticeDetailsController controller)
        {
            var httpContext = new DefaultHttpContext();
            var apprenticeId = Guid.NewGuid();

            controller.ControllerContext = new ControllerContext
            {
                HttpContext = httpContext
            };

            mediator.Setup(m => m.Send(It.IsAny<GetApprenticeApprenticeshipsQuery>(), default)).ReturnsAsync(new GetApprenticeApprenticeshipsQueryResult
            {
                Apprenticeships = null
            });

            var result = await controller.GetApprenticeApprenticeshipRegistration(apprenticeId);
            result.Should().BeOfType(typeof(NotFoundResult));
        }

        [Test, MoqAutoData]
        public async Task Get_ApprenticeApprenticeship_NoConfirmedApprenticeship_Test(
            [Frozen] Mock<IMediator> mediator,
            [Frozen] Apprenticeship apprenticeship,
            [Greedy] ApprenticeDetailsController controller)
        {
            var httpContext = new DefaultHttpContext();
            var apprenticeId = Guid.NewGuid();

            controller.ControllerContext = new ControllerContext
            {
                HttpContext = httpContext
            };

            GetApprenticeApprenticeshipsQueryResult apprenticeshipsResult = new() { Apprenticeships = new List<Apprenticeship>() { apprenticeship } };
            mediator.Setup(m => m.Send(It.IsAny<GetApprenticeApprenticeshipsQuery>(), default)).ReturnsAsync(apprenticeshipsResult);

            var result = await controller.GetApprenticeApprenticeshipRegistration(apprenticeId);
            result.Should().BeOfType(typeof(NotFoundResult));
        }

        [Test, MoqAutoData]
        public async Task Get_ApprenticeApprenticeship_NoRegistration_Test(
            [Frozen] Mock<IMediator> mediator,
            [Frozen] GetApprenticeApprenticeshipsQueryResult apprenticeshipsResult,
            [Frozen] Apprenticeship apprenticeship,
            [Greedy] ApprenticeDetailsController controller)
        {
            var httpContext = new DefaultHttpContext();
            var apprenticeId = Guid.NewGuid();

            controller.ControllerContext = new ControllerContext
            {
                HttpContext = httpContext
            };

            apprenticeship.ConfirmedOn = null;
            apprenticeshipsResult.Apprenticeships.Add(apprenticeship);
            GetApprenticeshipRegistrationQueryResult registrationresult = null;
            mediator.Setup(m => m.Send(It.IsAny<GetApprenticeApprenticeshipsQuery>(), default)).ReturnsAsync(apprenticeshipsResult);
            mediator.Setup(m => m.Send(It.IsAny<GetApprenticeshipRegistrationQuery>(), default)).ReturnsAsync(registrationresult);
            var result = await controller.GetApprenticeApprenticeshipRegistration(apprenticeId);
            result.Should().BeOfType(typeof(NotFoundResult));
        }

        [Test, MoqAutoData]
        public async Task Get_ApprenticeApprenticeship_Registration_Test(
            [Frozen] Mock<IMediator> mediator,
            [Frozen] GetApprenticeApprenticeshipsQueryResult apprenticeshipsResult,
            [Frozen] GetApprenticeshipRegistrationQueryResult registrationResult,
            [Frozen] Apprenticeship apprenticeship,
            [Greedy] ApprenticeDetailsController controller)
        {
            var httpContext = new DefaultHttpContext();
            var apprenticeId = Guid.NewGuid();

            controller.ControllerContext = new ControllerContext
            {
                HttpContext = httpContext
            };

            apprenticeship.ConfirmedOn = null;
            apprenticeshipsResult.Apprenticeships.Add(apprenticeship);
            mediator.Setup(m => m.Send(It.IsAny<GetApprenticeApprenticeshipsQuery>(), default)).ReturnsAsync(apprenticeshipsResult);
            var result = await controller.GetApprenticeApprenticeshipRegistration(apprenticeId);
            result.Should().BeOfType(typeof(OkObjectResult));
        }

        [Test, MoqAutoData]
        public async Task Get_RegistrationByEmail_NoApprenticeshipsFound_Test(
           [Frozen] Mock<IMediator> mediator,
           [Greedy] ApprenticeDetailsController controller)
        {
            var httpContext = new DefaultHttpContext();
            string email = "test@test.com";

            controller.ControllerContext = new ControllerContext
            {
                HttpContext = httpContext
            };

            mediator.Setup(m => m.Send(It.IsAny<GetApprenticeshipRegistrationByEmailQuery>(), default)).ReturnsAsync((GetApprenticeshipRegistrationQueryResult) null);
            var result = await controller.GetApprenticeshipRegistration(email);
            result.Should().BeOfType(typeof(NotFoundResult));
        }

        [Test, MoqAutoData]
        public async Task Get_RegistrationByEmail_Test(
           [Frozen] Mock<IMediator> mediator,
           [Greedy] ApprenticeDetailsController controller)
        {
            var httpContext = new DefaultHttpContext();
            string email = "test@test.com";

            controller.ControllerContext = new ControllerContext
            {
                HttpContext = httpContext
            };

            mediator.Setup(m => m.Send(It.IsAny<GetApprenticeshipRegistrationByEmailQuery>(), default)).ReturnsAsync(new GetApprenticeshipRegistrationQueryResult
            {
                RegistrationId = Guid.NewGuid()
            });

            var result = await controller.GetApprenticeshipRegistration(email);
            result.Should().BeOfType(typeof(OkObjectResult));
        }
    }
}