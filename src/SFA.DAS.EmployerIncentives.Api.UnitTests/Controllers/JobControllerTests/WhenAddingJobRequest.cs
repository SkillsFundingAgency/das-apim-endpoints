using AutoFixture.NUnit3;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerIncentives.Api.Controllers;
using SFA.DAS.EmployerIncentives.Models.PassThrough;
using SFA.DAS.SharedOuterApi.Infrastructure;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.Testing.AutoFixture;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerIncentives.Api.UnitTests.Controllers.JobControllerTests
{
    [TestFixture]
    public class WhenAddingJobRequest
    {
        private InnerApiResponse _innerApiResponse;

        [SetUp]
        public void Arrange()
        {
            _innerApiResponse = new InnerApiResponse
            {
                StatusCode = HttpStatusCode.OK
            };
        }

        [Test, MoqAutoData]
        public async Task Then_Request_Is_Passed_To_Inner_Api(
            JobRequest request,
            [Frozen] Mock<IPassThroughApiClient> passThroughMock,
            [Greedy] JobController sut)
        {
            passThroughMock
                .Setup(x => x.Put(It.IsAny<string>(), It.IsAny<JobRequest>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(_innerApiResponse);

            await sut.AddJob(request);

            passThroughMock.Verify(x => x.Put($"/jobs", request, It.IsAny<CancellationToken>()));
        }

        [Test, MoqAutoData]
        public async Task Then_Response_Is_Returned_From_Inner_Api(
            JobRequest request,
            [Frozen] Mock<IPassThroughApiClient> passThroughMock,
            [Greedy] JobController sut)
        {
            passThroughMock
                .Setup(x => x.Put(It.IsAny<string>(), It.IsAny<JobRequest>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(_innerApiResponse);

            var result = await sut.AddJob(request);

            result.Should().NotBeNull();
            result.Should().BeOfType<ObjectResult>();
            var objectResult = (ObjectResult)result;
            objectResult.StatusCode.Should().Be((int)_innerApiResponse.StatusCode);
        }
    }
}
