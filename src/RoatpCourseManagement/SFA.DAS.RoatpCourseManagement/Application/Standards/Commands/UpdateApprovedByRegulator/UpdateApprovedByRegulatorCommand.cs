using MediatR;
using System.Net;

namespace SFA.DAS.RoatpCourseManagement.Application.Standards.Commands.UpdateApprovedByRegulator
{
    public class UpdateApprovedByRegulatorCommand : IRequest<HttpStatusCode>
    {
        public int Ukprn { get; set; }
        public int LarsCode { get; set; }
        public string UserId { get; set; }
        public bool IsApprovedByRegulator { get; set; }
    }
}
