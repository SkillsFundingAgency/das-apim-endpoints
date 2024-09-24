using MediatR;
using Microsoft.Extensions.Options;
using SFA.DAS.EmployerRequestApprenticeTraining.Configuration;
using SFA.DAS.EmployerRequestApprenticeTraining.InnerApi.Requests;
using SFA.DAS.EmployerRequestApprenticeTraining.InnerApi.Responses;
using SFA.DAS.EmployerRequestApprenticeTraining.Models;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Exceptions;
using SFA.DAS.SharedOuterApi.Extensions;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.Courses;
using SFA.DAS.SharedOuterApi.Interfaces;
using System.Threading;
using System.Threading.Tasks;
using static SFA.DAS.EmployerRequestApprenticeTraining.InnerApi.Requests.PostStandardRequest;

namespace SFA.DAS.EmployerRequestApprenticeTraining.Application.Commands.PostStandard
{
    public class PostStandardCommandHandler : IRequestHandler<PostStandardCommand, PostStandardResult>
    {
        private readonly IRequestApprenticeTrainingApiClient<RequestApprenticeTrainingApiConfiguration> _requestApprenticeTrainingApiClient;
        private readonly ICoursesApiClient<CoursesApiConfiguration> _coursesApiClient;

        public PostStandardCommandHandler(
            IRequestApprenticeTrainingApiClient<RequestApprenticeTrainingApiConfiguration> requestApprenticeTrainingApiClient,
            ICoursesApiClient<CoursesApiConfiguration> coursesApiClient)
        {
            _requestApprenticeTrainingApiClient = requestApprenticeTrainingApiClient;
            _coursesApiClient = coursesApiClient;
        }

        public async Task<PostStandardResult> Handle(PostStandardCommand command, CancellationToken cancellationToken)
        {
            var coursesResponse = await _coursesApiClient.
                GetWithResponseCode<StandardDetailResponse>(new GetStandardDetailsByIdRequest(command.StandardId));

            if (coursesResponse.StatusCode != System.Net.HttpStatusCode.NotFound)
            {
                coursesResponse.EnsureSuccessStatusCode();

                var response = await _requestApprenticeTrainingApiClient
                .PostWithResponseCode<PostStandardRequestData, StandardResponse>((PostStandardRequest)coursesResponse.Body);

                response.EnsureSuccessStatusCode();

                return new PostStandardResult
                {
                    Standard = (Standard)response.Body,
                };
            }
            throw new ApiResponseException(System.Net.HttpStatusCode.NotFound, "No standard found");
        }
    }
}
