using System;
using System.Collections.Generic;
using SFA.DAS.EmployerFinance.InnerApi.Responses;

namespace SFA.DAS.EmployerFinance.Api.Models
{
    public class GetStandardsResponse
    {
        public IEnumerable<StandardResponse> Standards { get; set; }
    }

    public class StandardResponse
    {
        public string Id { get; set; }
        public int Level { get; set; }
        public string Title { get; set; }
        public string LearningType { get; set; }

        public static implicit operator StandardResponse(GetStandardsListItem source)
        {
            return new StandardResponse
            {
                Id = source.LarsCode,
                Level = source.Level,
                Title = source.Title,
                LearningType = source.LearningType
            };
        }
    }
}