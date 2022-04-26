using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmploymentCheck.Application.Commands.RegisterCheck;
using SFA.DAS.EmploymentCheck.Configuration;
using SFA.DAS.EmploymentCheck.Requests;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;
using SFA.DAS.Testing.AutoFixture;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.EmploymentCheck.Api.UnitTests.RegisterEmploymentCheck
{
    public class WhenHandlingRegisterCheckCommand
    {
        [Test, MoqAutoData]
        public async Task Then_RegisterCheck_is_handled(
            RegisterCheckCommand requestData,
            RegisterCheckResponse expected,
            [Frozen] Mock<IInternalApiClient<EmploymentCheckConfiguration>> mockClient,
            [Greedy] RegisterCheckCommandHandler sut)
        {
            mockClient
                .Setup(client => client.PostWithResponseCode<RegisterCheckResponse>
                (It.Is<RegisterCheckRequest>(
                    request =>
                        request.PostUrl == "api/EmploymentCheck/RegisterCheck"
                        && request.Data == requestData)))
                .ReturnsAsync(new ApiResponse<RegisterCheckResponse>(expected, HttpStatusCode.OK, string.Empty));

            var actual = await sut.Handle(requestData, CancellationToken.None);

            actual.Should().Be(expected);
        }
    }
}
