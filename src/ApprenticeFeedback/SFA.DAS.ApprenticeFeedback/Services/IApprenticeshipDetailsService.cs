using SFA.DAS.ApprenticeFeedback.InnerApi.Requests;
using SFA.DAS.ApprenticeFeedback.Models;
using System;
using System.Threading.Tasks;

namespace SFA.DAS.ApprenticeFeedback.Services
{
    public interface IApprenticeshipDetailsService
    {
        Task<ApprenticeshipDetails> Get(Guid apprenticeId, long apprenticeshipId);
    }
}
