using AutoFixture.NUnit3;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SFA.DAS.LearnerDataJobs.Application.Commands;
using SFA.DAS.LearnerDataJobs.Application.Queries;
using SFA.DAS.LearnerDataJobs.InnerApi;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;
using SFA.DAS.Testing.AutoFixture;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SFA.DAS.LearnerDataJobs.UnitTests.Application.Queries
{
    public class GetLearnerByIdQueryHandlerTests
    {
        [Test, MoqAutoData]
        public async Task Then_GetLearnerByIdQuery_Returns_ApprenticeshipId(
       GetLearnerByIdQuery command,
       [Frozen] Mock<IInternalApiClient<LearnerDataInnerApiConfiguration>> client,
       [Greedy] GetLearnerByIdQueryHandler handler)
        {

            var expectedUrl =
                $"providers/{command.ukprn}/learners/{command.Id}";

            client.Setup(x =>
                    x.GetWithResponseCode<GetLearnerDataByIdResponse>(
                        It.Is<GetLearnerByIdRequest>(p => p.GetUrl == expectedUrl)))
                 .ReturnsAsync(new ApiResponse<GetLearnerDataByIdResponse>(new GetLearnerDataByIdResponse() { ApprenticeshipId=1 }, HttpStatusCode.OK,""));
            

            var result = await handler.Handle(command, CancellationToken.None);

            result.ApprenticeshipId.Should().NotBeNull();
        }

        [Test, MoqAutoData]
        public async Task Then_GetLearnerByIdQuery_Returns_Invalid_ApprenticeshipId(
       GetLearnerByIdQuery command,
       [Frozen] Mock<IInternalApiClient<LearnerDataInnerApiConfiguration>> client,
       [Greedy] GetLearnerByIdQueryHandler handler)
        {

            var expectedUrl =
                $"providers/{command.ukprn}/learners/{command.Id}";
            client.Setup(x =>
                    x.GetWithResponseCode<GetLearnerDataByIdResponse>(
                        It.Is<GetLearnerByIdRequest>(p => p.GetUrl == expectedUrl)))
                 .ReturnsAsync(new ApiResponse<GetLearnerDataByIdResponse>(new GetLearnerDataByIdResponse(), HttpStatusCode.NotFound, ""));



            var result = await handler.Handle(command, CancellationToken.None);

            result.ApprenticeshipId.Should().BeNull();
        }
    }
}
