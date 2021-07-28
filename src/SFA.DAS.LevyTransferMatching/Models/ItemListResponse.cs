using System.Collections.Generic;

namespace SFA.DAS.LevyTransferMatching.Models
{
    public abstract class ItemListResponse<TResult>
    {
        public IEnumerable<TResult> Items { get; set; }
        public int TotalItems { get; set; }
    }
}