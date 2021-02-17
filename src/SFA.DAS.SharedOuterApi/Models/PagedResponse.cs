using System.Collections.Generic;

namespace SFA.DAS.SharedOuterApi.Models
{
    public class PagedResponse<T>
    {
        public List<T> Data { get; set; }
        public int Page { get; set; }
        public int TotalPages { get; set; }
    }
}
