using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.SharedOuterApi.Infrastructure;
using SFA.DAS.Vacancies.Manage.Configuration;
using SFA.DAS.Vacancies.Manage.InnerApi.Requests;
using SFA.DAS.Vacancies.Manage.Interfaces;

namespace SFA.DAS.Vacancies.Manage.Application.Recruit.Commands.CreateVacancy
{
    public class CreateVacancyCommandHandler : IRequestHandler<CreateVacancyCommand, CreateVacancyCommandResponse>
    {
        private readonly IRecruitApiClient<RecruitApiConfiguration> _recruitApiClient;

        public CreateVacancyCommandHandler (IRecruitApiClient<RecruitApiConfiguration> recruitApiClient)
        {
            _recruitApiClient = recruitApiClient;
        }
        public async Task<CreateVacancyCommandResponse> Handle(CreateVacancyCommand request, CancellationToken cancellationToken)
        {
            var apiRequest = new PostVacancyRequest(request.Id, request.PostVacancyRequestData);
            
            //todo if sandman then does a validate request. which needs that end point created in recruit

            var result = await _recruitApiClient.PostWithResponseCode<string>(apiRequest);

            if(!((int)result.StatusCode >= 200 && (int)result.StatusCode <= 299))
            {
                if (result.StatusCode.Equals(HttpStatusCode.BadRequest))
                {
                    throw new HttpRequestContentException($"Response status code does not indicate success: {(int)result.StatusCode} ({result.StatusCode})", result.StatusCode, result.ErrorContent);    
                }

                throw new Exception(
                    $"Response status code does not indicate success: {(int) result.StatusCode} ({result.StatusCode})");
            }
            
            return new CreateVacancyCommandResponse
            {
                VacancyReference = result.Body
            };
        }
    }
}