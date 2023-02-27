using System;
using System.Threading.Tasks;
using LearnerServiceClient;
using SFA.DAS.SharedOuterApi.ConnectedServices.LearnerWebService;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.SharedOuterApi.Services
{
    public class LearnerValidationService : ILearnerValidationService
    {
        private readonly ILearnerServiceClientProvider<LearnerPortTypeClient> _lrsClientProvider;

        public LearnerValidationService(ILearnerServiceClientProvider<LearnerPortTypeClient> lrsClientProvider)
        {
            _lrsClientProvider = lrsClientProvider;
        }
        public async Task<MIAPVerifiedLearner> ValidateLearner(string uln, string firstName, string lastName)
        {
            try
            {
                var service = _lrsClientProvider.GetServiceAsync();
                var learnerVerificationResponse = await service.verifyLearnerAsync(
                    new verifyLearnerRequest(
                        new VerifyLearnerRqst(){ 
                            LearnerToVerify = new MIAPLearnerToVerify() { ULN = uln, GivenName = firstName, FamilyName = lastName }
                        }));

                await service.CloseAsync();

                return learnerVerificationResponse.VerifyLearnerResponse.VerifiedLearner;
            }
            catch (Exception ex)
            {
                //TODO: think about logging?
                throw;
            }
            
        }
    }

}