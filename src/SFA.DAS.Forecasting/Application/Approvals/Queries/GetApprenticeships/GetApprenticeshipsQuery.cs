using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace SFA.DAS.Forecasting.Application.Approvals.Queries.GetApprenticeships
{
    public class GetApprenticeshipsQuery : IRequest<GetApprenticeshipsQueryResult>
    {
        public long AccountId { get; set; }
        public string Status { get; set; }
        public int PageNumber { get; set; }
        public int PageItemCount { get; set; }
    }

    public class GetApprenticeshipsQueryResult
    {
        public int TotalApprenticeshipsFound { get; set; }

        public List<Apprenticeship> Apprenticeships { get; set; }

        public class Apprenticeship
        {
            public long Id { get; set; }
            public long TransferSenderId { get; set; }
            public string Uln { get; set; }
            public long ProviderId { get; set; }
            public string ProviderName { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string CourseCode { get; set; }
            public string CourseName { get; set; }
            public DateTime StartDate { get; set; }
            public DateTime EndDate { get; set; }
            public decimal? Cost { get; set; }
            public int? PledgeApplicationId { get; set; }
        }
    }

    public class GetApprenticeshipsQueryHandler : IRequestHandler<GetApprenticeshipsQuery, GetApprenticeshipsQueryResult>
    {
        public Task<GetApprenticeshipsQueryResult> Handle(GetApprenticeshipsQuery request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
