using System;
using System.Threading.Tasks;

namespace SFA.DAS.FindApprenticeshipTraining.Interfaces
{
    public interface IShortlistService
    {
        Task<int> GetShortlistItemCount(Guid? userId);
    }
}