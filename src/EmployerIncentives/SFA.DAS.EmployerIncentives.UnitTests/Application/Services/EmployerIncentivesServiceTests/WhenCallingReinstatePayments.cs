using System;
using System.Net;
using System.Threading.Tasks;
using AutoFixture;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerIncentives.Application.Services;
using SFA.DAS.EmployerIncentives.Configuration;
using SFA.DAS.EmployerIncentives.InnerApi.Requests;
using SFA.DAS.EmployerIncentives.Interfaces;
using SFA.DAS.SharedOuterApi.Infrastructure;
using SFA.DAS.SharedOuterApi.Models;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerIncentives.UnitTests.Application.Services.EmployerIncentivesServiceTests
{
    [TestFixture]
    public class WhenCallingReinstatePayments
    {
        [Test, MoqAutoData]
        public async Task Then_The_Api_Is_Called_With_The_Revert_Payments_Request(
            ReinstatePaymentsRequest request,
            [Frozen] Mock<IEmployerIncentivesApiClient<EmployerIncentivesConfiguration>> client,
            EmployerIncentivesService service
        )
        {
            client.Setup(x => x.PostWithResponseCode<PostReinstatePaymentsRequest>(
                It.Is<PostReinstatePaymentsRequest>(
                    c => c.PostUrl.Contains("reinstate-payments")
                ))).ReturnsAsync(new ApiResponse<PostReinstatePaymentsRequest>(null, HttpStatusCode.NoContent, ""));

            await service.ReinstatePayments(request);

            client.Verify(x => x.PostWithResponseCode<PostReinstatePaymentsRequest>(
                It.Is<PostReinstatePaymentsRequest>(c => c.PostUrl.Contains("reinstate-payments")
                )), Times.Once);
        }

        [Test]
        public async Task Then_an_exception_is_thrown_for_a_non_success_http_status_code()
        {
            // Arrange
            var fixture = new Fixture();
            var client = new Mock<IEmployerIncentivesApiClient<EmployerIncentivesConfiguration>>();
            client.Setup(x => x.PostWithResponseCode<PostReinstatePaymentsRequest>(
                It.Is<PostReinstatePaymentsRequest>(
                    c => c.PostUrl.Contains("reinstate-payments")
                ))).ReturnsAsync(new ApiResponse<PostReinstatePaymentsRequest>(null, HttpStatusCode.BadRequest, "Invalid request"));

            var request = fixture.Create<ReinstatePaymentsRequest>();

            var service = new EmployerIncentivesService(client.Object);

            // Act
            Func<Task> act = async () => { await service.ReinstatePayments(request); };

            // Assert
            await act.Should().ThrowAsync<HttpRequestContentException>();
        }
    }
}
