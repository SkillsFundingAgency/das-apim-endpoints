using SFA.DAS.ApprenticeFeedback.InnerApi.Requests;
using System;
using System.Threading.Tasks;

namespace SFA.DAS.ApprenticeFeedback.Services
{
    public interface IApprenticeshipDetailsService
    {
        Task<(LearnerData LearnerData, MyApprenticeshipData MyApprenticeshipData)> Get(Guid apprenticeId, long apprenticeshipId);
    }
}
