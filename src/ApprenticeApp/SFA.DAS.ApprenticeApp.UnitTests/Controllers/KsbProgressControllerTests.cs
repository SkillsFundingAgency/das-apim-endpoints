using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.ApprenticeApp.Api.Controllers;
using SFA.DAS.ApprenticeApp.Application.Queries.ApprenticeshipDetails;
using SFA.DAS.ApprenticeApp.Application.Queries.CourseOptionKsbs;
using SFA.DAS.ApprenticeApp.Application.Queries.Details;
using SFA.DAS.ApprenticeApp.Application.Queries.KsbProgress;
using SFA.DAS.ApprenticeApp.Models;
using SFA.DAS.ApprenticeApp.Telemetry;
using SFA.DAS.Testing.AutoFixture;
using System;
using System.Threading.Tasks;

namespace SFA.DAS.ApprenticeApp.UnitTests
{
    public class KsbProgressControllerTests
    {

        [Test, MoqAutoData]
        public async Task AddUpdateKsbProgress_Test(
            [Frozen] Mock<IMediator> mediator,
            [Greedy] KsbProgressController controller)
        {
            var httpContext = new DefaultHttpContext();
            var apprenticeId = Guid.NewGuid();
            ApprenticeKsbProgressData data = new()
            {
                ApprenticeshipId = 1,
                CurrentStatus = KSBStatus.InProgress,
                KSBId = new Guid(),
                KsbKey = "key",
                KSBProgressType = KSBProgressType.Behaviour,
                Note = "note"
            };

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
            var result = await controller.AddUpdateKsbProgress(apprenticeId, data);
            result.Should().BeOfType(typeof(Microsoft.AspNetCore.Mvc.OkResult));
        }

        [Test, MoqAutoData]
        public async Task AddUpdateKsbProgress_NoApprenticeship_Test(
            [Frozen] Mock<IMediator> mediator,
            [Greedy] KsbProgressController controller)
        {
            var httpContext = new DefaultHttpContext();
            var apprenticeId = Guid.NewGuid();
            ApprenticeKsbProgressData data = new()
            {
                ApprenticeshipId = 1,
                CurrentStatus = KSBStatus.InProgress,
                KSBId = new Guid(),
                KsbKey = "key",
                KSBProgressType = KSBProgressType.Behaviour,
                Note = "note"
            };

            controller.ControllerContext = new ControllerContext
            {
                HttpContext = httpContext
            };

            mediator.Setup(m => m.Send(It.IsAny<GetApprenticeDetailsQuery>(), default)).ReturnsAsync(new GetApprenticeDetailsQueryResult
            {
                ApprenticeDetails = new ApprenticeDetails
                {
                    Apprentice = new Apprentice { ApprenticeId = apprenticeId },
                    MyApprenticeship = null
                }
            });
            var result = await controller.AddUpdateKsbProgress(apprenticeId, data);
            result.Should().BeOfType(typeof(Microsoft.AspNetCore.Mvc.OkResult));
        }

        [Test, MoqAutoData]
        public async Task RemoveTaskToKsbProgress_test(
            [Frozen] Mock<IMediator> mediator,
            [Greedy] KsbProgressController controller)
        {
            var httpContext = new DefaultHttpContext();
            var apprenticeId = Guid.NewGuid();
            int ksbProgressId = 1;
            int taskId = 2;

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

            var result = await controller.RemoveTaskToKsbProgress(apprenticeId, ksbProgressId, taskId);
            result.Should().BeOfType(typeof(Microsoft.AspNetCore.Mvc.OkResult));
        }

        [Test, MoqAutoData]
        public async Task RemoveTaskToKsbProgress_NoApprenticeship_test(
           [Frozen] Mock<IMediator> mediator,
           [Greedy] KsbProgressController controller)
        {
            var httpContext = new DefaultHttpContext();
            var apprenticeId = Guid.NewGuid();
            int ksbProgressId = 1;
            int taskId = 2;

            controller.ControllerContext = new ControllerContext
            {
                HttpContext = httpContext
            };

            mediator.Setup(m => m.Send(It.IsAny<GetApprenticeDetailsQuery>(), default)).ReturnsAsync(new GetApprenticeDetailsQueryResult
            {
                ApprenticeDetails = new ApprenticeDetails
                {
                    Apprentice = new Apprentice { ApprenticeId = apprenticeId },
                    MyApprenticeship = null
                }
            });

            var result = await controller.RemoveTaskToKsbProgress(apprenticeId, ksbProgressId, taskId);
            result.Should().BeOfType(typeof(Microsoft.AspNetCore.Mvc.OkResult));
        }



        [Test, MoqAutoData]
        public async Task GetTaskKsbProgress_NoApprenticeship_Test(
           [Frozen] Mock<IMediator> mediator,
           [Greedy] KsbProgressController controller)
        {
            var httpContext = new DefaultHttpContext();
            var apprenticeId = Guid.NewGuid();
            Guid[] guids = { Guid.NewGuid() };

            controller.ControllerContext = new ControllerContext
            {
                HttpContext = httpContext
            };

            mediator.Setup(m => m.Send(It.IsAny<GetApprenticeDetailsQuery>(), default)).ReturnsAsync(new GetApprenticeDetailsQueryResult
            {
                ApprenticeDetails = new ApprenticeDetails
                {
                    Apprentice = new Apprentice { ApprenticeId = apprenticeId },
                    MyApprenticeship = null
                }
            });

            var result = await controller.GetKsbsByApprenticeshipIdAndGuidListQuery(apprenticeId, guids);
            result.Should().BeOfType(typeof(Microsoft.AspNetCore.Mvc.OkResult));
        }

        [Test, MoqAutoData]
        public async Task Get_ApprenticeKsbs_WithKsbs_Test(
            [Frozen] Mock<IApprenticeAppMetrics> metrics,
            Mock<IMediator> mediator)
        {
            var httpContext = new DefaultHttpContext();
            var apprenticeId = Guid.NewGuid();
            mediator.Setup(m => m.Send(It.IsAny<GetApprenticeDetailsQuery>(), default)).ReturnsAsync(new GetApprenticeDetailsQueryResult
            {
                ApprenticeDetails = new ApprenticeDetails
                {
                    Apprentice = new Apprentice
                    {
                        ApprenticeId = apprenticeId
                    },
                    MyApprenticeship = new MyApprenticeship
                    {
                        ApprenticeshipId = 1,
                        StandardUId = "TestStandardUid"
                    }
                }
            });

            var ksbQueryResult = (new GetStandardOptionKsbsQueryResult
            {
                KsbsResult = new GetStandardOptionKsbsResult { Ksbs = new System.Collections.Generic.List<Ksb>() }
            });

            var testKsb = new Ksb
            {
                Id = Guid.NewGuid(),
                Type = KsbType.Knowledge,
                Key = "K1",
                Detail = "TestKnowledgeKsb"
            };

            ksbQueryResult.KsbsResult.Ksbs.Add(testKsb);

            var ksbProgressResult = new GetKsbsByApprenticeshipIdQueryResult
            {
                KSBProgresses = new System.Collections.Generic.List<ApprenticeKsbProgressData>()
            };

            mediator.Setup(m => m.Send(It.IsAny<GetStandardOptionKsbsQuery>(), default)).ReturnsAsync(ksbQueryResult);
            mediator.Setup(m => m.Send(It.IsAny<GetKsbsByApprenticeshipIdQuery>(), default)).ReturnsAsync(ksbProgressResult);

            var controller = new KsbProgressController(mediator.Object, metrics.Object);
            
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = httpContext
            };

            var result = await controller.GetApprenticeshipKsbs(apprenticeId);
            result.Should().BeOfType(typeof(Microsoft.AspNetCore.Mvc.OkObjectResult));
        }

        [Test, MoqAutoData]
        public async Task Get_ApprenticeKsbs_NoKsbs_Test(
           GetApprenticeshipQueryResult queryResult,
           [Frozen] Mock<IApprenticeAppMetrics> metrics,
           Mock<IMediator> mediator)
        {
            var httpContext = new DefaultHttpContext();
            var apprenticeId = Guid.NewGuid();
            mediator.Setup(m => m.Send(It.IsAny<GetApprenticeDetailsQuery>(), default)).ReturnsAsync(new GetApprenticeDetailsQueryResult
            {
                ApprenticeDetails = new ApprenticeDetails
                {
                    Apprentice = new Apprentice
                    {
                        ApprenticeId = apprenticeId
                    },
                    MyApprenticeship = new MyApprenticeship
                    {
                        ApprenticeshipId = 1,
                        StandardUId = "TestStandardUid"
                    }
                }
            });

            mediator.Setup(m => m.Send(It.IsAny<GetApprenticeshipQuery>(), default)).ReturnsAsync(queryResult);
            mediator.Setup(m => m.Send(It.IsAny<GetStandardOptionKsbsQuery>(), default)).ReturnsAsync(new GetStandardOptionKsbsQueryResult { KsbsResult = null});

            var controller = new KsbProgressController(mediator.Object, metrics.Object);

            controller.ControllerContext = new ControllerContext
            {
                HttpContext = httpContext
            };

            var result = await controller.GetApprenticeshipKsbs(apprenticeId);
            result.Should().BeOfType(typeof(Microsoft.AspNetCore.Mvc.OkResult));
        }

        [Test, MoqAutoData]
        public async Task Get_ApprenticeKsbs_NoApprenticeshipDetailsFound_Test(
            [Frozen] Mock<IApprenticeAppMetrics> metrics,
            Mock<IMediator> mediator)
        {
            var httpContext = new DefaultHttpContext();
            var apprenticeId = Guid.NewGuid();
            mediator.Setup(m => m.Send(It.IsAny<GetApprenticeDetailsQuery>(), default)).ReturnsAsync(new GetApprenticeDetailsQueryResult
            {
                ApprenticeDetails = new ApprenticeDetails
                {
                    Apprentice = new Apprentice
                    {
                        ApprenticeId = apprenticeId
                    },
                    MyApprenticeship = null
                }
            });

            var controller = new KsbProgressController(mediator.Object, metrics.Object);

            controller.ControllerContext = new ControllerContext
            {
                HttpContext = httpContext
            };

            var result = await controller.GetApprenticeshipKsbs(apprenticeId);
            result.Should().BeOfType(typeof(Microsoft.AspNetCore.Mvc.OkResult));
        }

        [Test, MoqAutoData]
        public async Task Get_ApprenticeKsbs_NoApprenticeshipFound_Test(
           [Frozen] Mock<IApprenticeAppMetrics> metrics,
           Mock<IMediator> mediator)
        {
            var httpContext = new DefaultHttpContext();
            var apprenticeId = Guid.NewGuid();
            mediator.Setup(m => m.Send(It.IsAny<GetApprenticeDetailsQuery>(), default)).ReturnsAsync(new GetApprenticeDetailsQueryResult
            {
                ApprenticeDetails = new ApprenticeDetails
                {
                    Apprentice = new Apprentice
                    {
                        ApprenticeId = apprenticeId
                    },
                    MyApprenticeship = new MyApprenticeship
                    {
                        ApprenticeshipId = 1,
                        StandardUId = "TestStandardUid"
                    }
                }
            });

            mediator.Setup(m => m.Send(It.IsAny<GetApprenticeshipQuery>(), default)).ReturnsAsync((GetApprenticeshipQueryResult) null);

            var controller = new KsbProgressController(mediator.Object, metrics.Object);

            controller.ControllerContext = new ControllerContext
            {
                HttpContext = httpContext
            };

            var result = await controller.GetApprenticeshipKsbs(apprenticeId);
            result.Should().BeOfType(typeof(Microsoft.AspNetCore.Mvc.OkResult));
        }

        [Test, MoqAutoData]
        public async Task GetKsbProgressForTask(
           [Frozen] Mock<IMediator> mediator,
           [Greedy] KsbProgressController controller)
        {
            var httpContext = new DefaultHttpContext();
            var apprenticeId = Guid.NewGuid();
            var taskId = 1;

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
            var result = await controller.GetKsbProgressForTask(apprenticeId, taskId);
            result.Should().BeOfType(typeof(Microsoft.AspNetCore.Mvc.OkObjectResult));
        }

        [Test, MoqAutoData]
        public async Task GetKsbProgressForTask_NoApprenticeship(
           [Frozen] Mock<IMediator> mediator,
           [Greedy] KsbProgressController controller)
        {
            var httpContext = new DefaultHttpContext();
            var apprenticeId = Guid.NewGuid();
            var taskId = 1;

            controller.ControllerContext = new ControllerContext
            {
                HttpContext = httpContext
            };
            mediator.Setup(m => m.Send(It.IsAny<GetApprenticeDetailsQuery>(), default)).ReturnsAsync(new GetApprenticeDetailsQueryResult
            {
                ApprenticeDetails = new ApprenticeDetails
                {
                    Apprentice = new Apprentice { ApprenticeId = apprenticeId },
                    MyApprenticeship = null
                }
            });
            var result = await controller.GetKsbProgressForTask(apprenticeId, taskId);
            result.Should().BeOfType(typeof(Microsoft.AspNetCore.Mvc.OkResult));
        }

        [Test, MoqAutoData]
        public async Task Get_ApprenticeKsb_Test(
            [Frozen] Mock<IApprenticeAppMetrics> metrics,
           Mock<IMediator> mediator)
        {
            var httpContext = new DefaultHttpContext();
            var apprenticeId = Guid.NewGuid();

            mediator.Setup(m => m.Send(It.IsAny<GetApprenticeDetailsQuery>(), default)).ReturnsAsync(new GetApprenticeDetailsQueryResult
            {
                ApprenticeDetails = new ApprenticeDetails
                {
                    Apprentice = new Apprentice
                    {
                        ApprenticeId = apprenticeId
                    },
                    MyApprenticeship = new MyApprenticeship
                    {
                        ApprenticeshipId = 1,
                        StandardUId = "TestStandardUid"
                    }
                }
            });

            var ksbQueryResult = (new GetStandardOptionKsbsQueryResult
            {
                KsbsResult = new GetStandardOptionKsbsResult { Ksbs = new System.Collections.Generic.List<Ksb>() }
            });

            var testKsb = new Ksb
            {
                Id = Guid.NewGuid(),
                Type = KsbType.Knowledge,
                Key = "K1",
                Detail = "TestKnowledgeKsb"
            };

            ksbQueryResult.KsbsResult.Ksbs.Add(testKsb);

            var ksbProgressResult = new GetKsbsByApprenticeshipIdQueryResult
            {
                KSBProgresses = new System.Collections.Generic.List<ApprenticeKsbProgressData>()
            };


            mediator.Setup(m => m.Send(It.IsAny<GetStandardOptionKsbsQuery>(), default)).ReturnsAsync(ksbQueryResult);
            mediator.Setup(m => m.Send(It.IsAny<GetKsbsByApprenticeshipIdQuery>(), default)).ReturnsAsync(ksbProgressResult);

            var controller = new KsbProgressController(mediator.Object, metrics.Object);
            
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = httpContext
            };

            var result = await controller.GetApprenticeshipKsbProgress(apprenticeId, testKsb.Id);
            result.Should().BeOfType(typeof(Microsoft.AspNetCore.Mvc.OkObjectResult));
        }

        [Test, MoqAutoData]
        public async Task Get_ApprenticeKsb_NoKsb_Test(
            [Frozen] Mock<IApprenticeAppMetrics> metrics,
            Mock<IMediator> mediator)
        {
            var httpContext = new DefaultHttpContext();
            var apprenticeId = Guid.NewGuid();
            mediator.Setup(m => m.Send(It.IsAny<GetApprenticeDetailsQuery>(), default)).ReturnsAsync(new GetApprenticeDetailsQueryResult
            {
                ApprenticeDetails = new ApprenticeDetails
                {
                    Apprentice = new Apprentice
                    {
                        ApprenticeId = apprenticeId
                    },
                    MyApprenticeship = new MyApprenticeship
                    {
                        ApprenticeshipId = 1,
                        StandardUId = "TestStandardUid"
                    }
                }
            });

            var ksbQueryResult = (new GetStandardOptionKsbsQueryResult
            {
                KsbsResult = new GetStandardOptionKsbsResult { Ksbs = new System.Collections.Generic.List<Ksb>() }
            });

            var testKsb = new Ksb
            {
                Id = Guid.NewGuid(),
                Type = KsbType.Knowledge,
                Key = "K1",
                Detail = "TestKnowledgeKsb"
            };

            var testKsbProgress = new ApprenticeKsbProgressData
            {
                ApprenticeshipId = 1,
                CurrentStatus = KSBStatus.InProgress,
                KSBId = testKsb.Id,
                KsbKey = testKsb.Key,
                KSBProgressType = KSBProgressType.Knowledge,
                Note = "TestNote"
            };

            ksbQueryResult.KsbsResult.Ksbs.Add(testKsb);

            var ksbProgressResult = new GetKsbsByApprenticeshipIdQueryResult
            {
                KSBProgresses = new System.Collections.Generic.List<ApprenticeKsbProgressData>() { testKsbProgress }
            };

            mediator.Setup(m => m.Send(It.IsAny<GetStandardOptionKsbsQuery>(), default)).ReturnsAsync(ksbQueryResult);
            mediator.Setup(m => m.Send(It.IsAny<GetKsbsByApprenticeshipIdQuery>(), default)).ReturnsAsync(ksbProgressResult);

            var controller = new KsbProgressController(mediator.Object, metrics.Object);
           
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = httpContext
            };

            var result = await controller.GetApprenticeshipKsbProgress(apprenticeId, testKsb.Id);
            result.Should().BeOfType(typeof(Microsoft.AspNetCore.Mvc.OkObjectResult));
        }

        [Test, MoqAutoData]
        public async Task Get_ApprenticeKsb_NoKsbResult_Test(
            [Frozen] Mock<IApprenticeAppMetrics> metrics,
            Mock<IMediator> mediator)
        {
            var httpContext = new DefaultHttpContext();
            var apprenticeId = Guid.NewGuid();

            Guid ksbId = Guid.NewGuid();
            mediator.Setup(m => m.Send(It.IsAny<GetApprenticeDetailsQuery>(), default)).ReturnsAsync(new GetApprenticeDetailsQueryResult
            {
                ApprenticeDetails = new ApprenticeDetails
                {
                    Apprentice = new Apprentice
                    {
                        ApprenticeId = apprenticeId
                    },
                    MyApprenticeship = new MyApprenticeship
                    {
                        ApprenticeshipId = 1,
                        StandardUId = "TestStandardUid"
                    }
                }
            });
            var ksbQueryResult = new GetStandardOptionKsbsQueryResult();

            mediator.Setup(m => m.Send(It.IsAny<GetStandardOptionKsbsQuery>(), default)).ReturnsAsync(ksbQueryResult);

            var controller = new KsbProgressController(mediator.Object, metrics.Object);
            
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = httpContext
            };

            var result = await controller.GetApprenticeshipKsbProgress(apprenticeId, ksbId);
            result.Should().BeOfType(typeof(Microsoft.AspNetCore.Mvc.OkResult));
        }

        [Test, MoqAutoData]
        public async Task Get_ApprenticeKsb_NoKsbResult_NoApprenticeship_Test(
            [Frozen] Mock<IApprenticeAppMetrics> metrics,
            Mock<IMediator> mediator)
        {
            var httpContext = new DefaultHttpContext();
            var apprenticeId = Guid.NewGuid();

            Guid ksbId = Guid.NewGuid();
            mediator.Setup(m => m.Send(It.IsAny<GetApprenticeDetailsQuery>(), default)).ReturnsAsync(new GetApprenticeDetailsQueryResult
            {
                ApprenticeDetails = new ApprenticeDetails
                {
                    Apprentice = new Apprentice
                    {
                        ApprenticeId = apprenticeId
                    },
                    MyApprenticeship = null
                }
            });

            var controller = new KsbProgressController(mediator.Object, metrics.Object);
            
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = httpContext
            };

            var result = await controller.GetApprenticeshipKsbProgress(apprenticeId, ksbId);
            result.Should().BeOfType(typeof(Microsoft.AspNetCore.Mvc.OkResult));
        }

        [Test, MoqAutoData]
        public async Task GetKsbProgressForTask_NoApprenticeshipDetailsFound_Test(
           [Frozen] Mock<IApprenticeAppMetrics> metrics,
           Mock<IMediator> mediator)
        {
            var httpContext = new DefaultHttpContext();
            var apprenticeId = Guid.NewGuid();
            Guid ksbId = Guid.NewGuid();
            mediator.Setup(m => m.Send(It.IsAny<GetApprenticeDetailsQuery>(), default)).ReturnsAsync(new GetApprenticeDetailsQueryResult
            {
                ApprenticeDetails = new ApprenticeDetails
                {
                    Apprentice = new Apprentice
                    {
                        ApprenticeId = apprenticeId
                    },
                    MyApprenticeship = new MyApprenticeship
                    {
                        ApprenticeshipId = 1,
                        StandardUId = "TestStandardUid"
                    }
                }
            });

            mediator.Setup(m => m.Send(It.IsAny<GetApprenticeshipQuery>(), default)).ReturnsAsync((GetApprenticeshipQueryResult)null);

            var controller = new KsbProgressController(mediator.Object, metrics.Object);

            controller.ControllerContext = new ControllerContext
            {
                HttpContext = httpContext
            };

            var result = await controller.GetApprenticeshipKsbProgress(apprenticeId, ksbId);
            result.Should().BeOfType(typeof(Microsoft.AspNetCore.Mvc.OkResult));
        }
    }
}