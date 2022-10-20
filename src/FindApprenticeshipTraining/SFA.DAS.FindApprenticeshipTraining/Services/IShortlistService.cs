using System;
using System.Threading.Tasks;

namespace SFA.DAS.FindApprenticeshipTraining.Services
{
    public interface IShortlistService
    {
        Task<int> GetShortlistItemCount(Guid? userId);
    }
}