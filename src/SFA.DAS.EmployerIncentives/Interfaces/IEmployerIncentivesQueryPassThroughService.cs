using System;
using System.Threading;
using System.Threading.Tasks;
using SFA.DAS.EmployerIncentives.Infrastructure.Api;

namespace SFA.DAS.EmployerIncentives.Interfaces
{
    public interface IEmployerIncentivesQueryPassThroughService
    {
        Task<InnerApiResponse> GetAsync(Uri uri, object queryData = null, CancellationToken cancellationToken = default);
        Task<InnerApiResponse> GetAsync(string uri, object queryData = null, CancellationToken cancellationToken = default);
    }
}