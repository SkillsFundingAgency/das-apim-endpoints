using System.Collections.Generic;
using SFA.DAS.Approvals.Application;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Approvals.Services
{
    public interface IProviderCoursesService
    {
        Dictionary<string, string> GetCourses(long providerId);
    }

    public class ProviderCoursesService : IProviderCoursesService
    {
        private ServiceParameters _serviceParameters;
        private readonly ITrainingProviderService _trainingProviderService;
        private readonly IInternalApiClient<RoatpV2ApiConfiguration> _managingStandardsApiClient;
        private readonly ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration> _commitmentsV2ApiClient;

        public ProviderCoursesService(ServiceParameters serviceParameters,
            ITrainingProviderService trainingProviderService,
            IInternalApiClient<RoatpV2ApiConfiguration> managingStandardsApiClient,
            ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration> commitmentsV2ApiClient)
        {
            _trainingProviderService = trainingProviderService;
            _commitmentsV2ApiClient = commitmentsV2ApiClient;
            _managingStandardsApiClient = managingStandardsApiClient;
        }

        public Dictionary<string, string> GetCourses(long providerId)
        {
            //get type of provider

            //if main, return standards declared in managing standards
            //otherwise, return all standards from commitments api

            throw new System.NotImplementedException();
        }
    }

    
}
