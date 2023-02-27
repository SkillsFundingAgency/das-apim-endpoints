using LearnerServiceClient;
using System.Threading.Tasks;

namespace SFA.DAS.SharedOuterApi.Interfaces
{
    public interface ILearnerValidationService
    {
        Task<MIAPVerifiedLearner> ValidateLearner(string uln, string firstName, string lastName);
    }
}