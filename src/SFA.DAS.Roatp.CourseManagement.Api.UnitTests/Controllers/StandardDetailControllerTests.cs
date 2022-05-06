using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.Roatp.CourseManagement.Api.Controllers;
using SFA.DAS.Roatp.CourseManagement.Application.Standards.Queries.GetAllCoursesQuery;
using SFA.DAS.Roatp.CourseManagement.Application.Standards.Queries.GetStandardQuery;
using SFA.DAS.Roatp.CourseManagement.InnerApi.Responses;

namespace SFA.DAS.Roatp.CourseManagement.Api.UnitTests.Controllers
{
    [TestFixture]
    public class StandardDetailControllerTests
    {
        const int ValidLarsCode = 101;
        [TestCase(0, 400)]
        [TestCase(-1, 400)]
        [TestCase(ValidLarsCode, 200)]
        [Test]
        public async Task GetStandardDetail_ReturnsExpectedState(int larsCode, int expectedStatusCode)
        {
            var mediatorMock = new Mock<IMediator>();
            mediatorMock.Setup(m => m.Send(It.Is<GetStandardQuery>(q => q.LarsCode == larsCode), It.IsAny<CancellationToken>())).ReturnsAsync(new GetStandardResult {LarsCode = ValidLarsCode});


            var controller = new StandardDetailController(Mock.Of<ILogger<StandardDetailController>>(), mediatorMock.Object);

            var response = await controller.GetStandardDetail(larsCode);

            var statusCodeResult = response as IStatusCodeActionResult;

            Assert.AreEqual(expectedStatusCode, statusCodeResult.StatusCode.GetValueOrDefault());
        }
    }
}
