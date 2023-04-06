using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SFA.DAS.Approvals.Application;
using SFA.DAS.Approvals.Application.Shared.Enums;
using SFA.DAS.Approvals.InnerApi.CommitmentsV2Api.Requests.Courses;
using SFA.DAS.Approvals.InnerApi.CommitmentsV2Api.Responses.Courses;
using SFA.DAS.Approvals.InnerApi.ManagingStandards.Requests;
using SFA.DAS.Approvals.InnerApi.ManagingStandards.Responses;
using SFA.DAS.Approvals.Types;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Approvals.Services
{
    public interface IProviderCoursesService
    {
        Task<IEnumerable<Standard>> GetCourses(long providerId);
    }

    public class ProviderCoursesService : IProviderCoursesService
    {
        private readonly ServiceParameters _serviceParameters;
        private readonly ITrainingProviderService _trainingProviderService;
        private readonly IProviderCoursesApiClient<ProviderCoursesApiConfiguration> _providerCoursesApiClient;
        private readonly ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration> _commitmentsV2ApiClient;

        public ProviderCoursesService(ServiceParameters serviceParameters,
            ITrainingProviderService trainingProviderService,
            IProviderCoursesApiClient<ProviderCoursesApiConfiguration> providerCoursesApiClient,
            ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration> commitmentsV2ApiClient)
        {
            _serviceParameters = serviceParameters;
            _trainingProviderService = trainingProviderService;
            _commitmentsV2ApiClient = commitmentsV2ApiClient;
            _providerCoursesApiClient = providerCoursesApiClient;
        }

        public async Task<IEnumerable<Standard>> GetCourses(long providerId)
        {
            if (_serviceParameters.CallingParty == Party.Employer)
            {
                var result = await _commitmentsV2ApiClient.Get<GetAllStandardsResponse>(new GetAllStandardsRequest());
                return result.TrainingProgrammes.Select(x => new Standard(x.CourseCode, x.Name));
            }

            var providerDetails = await _trainingProviderService.GetTrainingProviderDetails(providerId);

            if (providerDetails.IsMainProvider)
            {
                var providerStandards =
                    await _providerCoursesApiClient.Get<IEnumerable<GetProviderStandardsResponse>>(
                        new GetProviderStandardsRequest(providerId));

                return providerStandards.Select(x => new Standard(x.LarsCode.ToString(), x.CourseNameWithLevel));
            }

            var result2 = await _commitmentsV2ApiClient.Get<GetAllStandardsResponse>(new GetAllStandardsRequest());
            return result2.TrainingProgrammes.Select(x => new Standard(x.CourseCode, x.Name));
        }
    }
}
