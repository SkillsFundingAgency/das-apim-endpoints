using SFA.DAS.Approvals.InnerApi.Responses;
using System.Threading.Tasks;

namespace SFA.DAS.Approvals.Application.Shared.LearnerDetailsValidation
{
    public interface ILearnerDetailsValidator
    {
        Task<LearnerVerificationResponse> Validate(ValidateLearnerDetailsRequest request);
    }
}