using System;
using MediatR;
using SFA.DAS.Approvals.Enums;
using SFA.DAS.Approvals.InnerApi;

namespace SFA.DAS.Approvals.Application.Apprentices.Queries.GetApprenticeshipsCSV
{
    public class GetApprenticeshipsCSVQuery : IRequest<GetApprenticeshipsCSVQueryResult>
    {
        public long ProviderId { get; set; }
        public string SearchTerm { get; set; }

        public string EmployerName { get; set; }

        public string CourseName { get; set; }

        public ApprenticeshipStatus? Status { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public Alerts? Alert { get; set; }

        public ConfirmationStatus? ApprenticeConfirmationStatus { get; set; }

        public DeliveryModel? DeliveryModel { get; set; }
    }
}
