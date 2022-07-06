using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.Approvals.InnerApi.Requests;
using SFA.DAS.Approvals.InnerApi.Responses;

namespace SFA.DAS.Approvals.Application.Apprentices.Commands.ChangeEmployer.Confirm
{
    public class ConfirmCommand : IRequest
    {
        public long ProviderId { get; set; }
        public long ApprenticeshipId { get; set; }
        public long AccountLegalEntityId { get; set; }
        public int? Price { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public DateTime? EmploymentEndDate { get; set; }
        public int? EmploymentPrice { get; set; }
        public DeliveryModel? DeliveryModel { get; set; }
        public UserInfo UserInfo { get; set; }
    }

    public class ConfirmCommandHandler : IRequestHandler<ConfirmCommand>
    {
        public Task<Unit> Handle(ConfirmCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
