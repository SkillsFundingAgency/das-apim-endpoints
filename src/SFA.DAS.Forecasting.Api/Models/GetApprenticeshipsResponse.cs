using System;
using System.Collections.Generic;
using System.Linq;
using SFA.DAS.Forecasting.Application.Approvals.Queries.GetApprenticeships;

namespace SFA.DAS.Forecasting.Api.Models
{
    public class GetApprenticeshipsResponse
    {
        public int TotalApprenticeshipsFound { get; set; }

        public IEnumerable<Apprenticeship> Apprenticeships { get; set; }

        public class Apprenticeship
        {
            public long Id { get; set; }
            public long? TransferSenderId { get; set; }
            public string Uln { get; set; }
            public long ProviderId { get; set; }
            public string ProviderName { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string CourseCode { get; set; }
            public string CourseName { get; set; }
            public int CourseLevel { get; set; }
            public DateTime StartDate { get; set; }
            public DateTime EndDate { get; set; }
            public decimal? Cost { get; set; }
            public int? PledgeApplicationId { get; set; }
        }

        public static implicit operator GetApprenticeshipsResponse(GetApprenticeshipsQueryResult source)
        {
            return new GetApprenticeshipsResponse
            {
                TotalApprenticeshipsFound = source.TotalApprenticeshipsFound,
                Apprenticeships = source.Apprenticeships.Select(a => new GetApprenticeshipsResponse.Apprenticeship
                {
                    Id = a.Id,
                    TransferSenderId = a.TransferSenderId,
                    Uln = a.Uln,
                    ProviderId = a.ProviderId,
                    ProviderName = a.ProviderName,
                    FirstName = a.FirstName,
                    LastName = a.LastName,
                    CourseCode = a.CourseCode,
                    CourseName = a.CourseName,
                    CourseLevel = a.CourseLevel,
                    StartDate = a.StartDate,
                    EndDate = a.EndDate,
                    Cost = a.Cost,
                    PledgeApplicationId = a.PledgeApplicationId
                })
            };
        }
    }
}
