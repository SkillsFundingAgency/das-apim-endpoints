using MediatR;
using System.Net;

namespace SFA.DAS.Roatp.CourseManagement.Application.Standards.Commands.UpdateConfirmRegulatedStandard
{
    public class UpdateConfirmRegulatedStandardCommand : IRequest<HttpStatusCode>
    {
        public int Ukprn { get; set; }
        public int LarsCode { get; set; }
        public bool? IsApprovedByRegulator { get; set; }
    }
}
