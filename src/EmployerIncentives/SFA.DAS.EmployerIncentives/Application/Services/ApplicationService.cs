using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using SFA.DAS.EmployerIncentives.Configuration;
using SFA.DAS.EmployerIncentives.Exceptions;
using SFA.DAS.EmployerIncentives.InnerApi.Requests;
using SFA.DAS.EmployerIncentives.InnerApi.Requests.IncentiveApplication;
using SFA.DAS.EmployerIncentives.InnerApi.Responses;
using SFA.DAS.EmployerIncentives.Interfaces;
using SFA.DAS.SharedOuterApi.Infrastructure;

namespace SFA.DAS.EmployerIncentives.Application.Services
{
    public class ApplicationService : IApplicationService
    {
        private readonly IEmployerIncentivesApiClient<EmployerIncentivesConfiguration> _client;

        public ApplicationService(IEmployerIncentivesApiClient<EmployerIncentivesConfiguration> client)
        {
            _client = client;
        }

        public Task Create(CreateIncentiveApplicationRequestData requestData)
        {
            return _client.Post<CreateIncentiveApplicationRequestData>(new CreateIncentiveApplicationRequest { Data = requestData });
        }

        public Task Update(UpdateIncentiveApplicationRequestData requestData)
        {
            return _client.Put(new UpdateIncentiveApplicationRequest { Data = requestData });
        }
        public async Task<long> GetApplicationLegalEntity(long accountId, Guid applicationId)
        {
            var response = await _client.Get<long>(new GetApplicationLegalEntityRequest(accountId, applicationId));

            return response;
        }

        public async Task Confirm(ConfirmIncentiveApplicationRequest request, CancellationToken cancellationToken = default)
        {
            var response = await _client.PatchWithResponseCode(request);
            if (response.StatusCode == HttpStatusCode.Conflict)
            {
                throw new UlnAlreadySubmittedException();
            }

            if (response.StatusCode != HttpStatusCode.OK)
            {
                throw new HttpRequestContentException($"Response status code does not indicate success: {(int)response.StatusCode} ({response.StatusCode})", response.StatusCode, response.Body);
            }
        }

        public async Task<IncentiveApplicationDto> Get(long accountId, Guid applicationId)
        {
            var response = await _client.Get<IncentiveApplicationDto>(new GetApplicationRequest(accountId, applicationId));

            return response;
        }

        public async Task<PaymentApplicationsDto> GetPaymentApplications(long accountId, long accountLegalEntityId)
        {
            return await _client.Get<PaymentApplicationsDto>(new GetApplicationsRequest(accountId, accountLegalEntityId));
        }
    }
}
