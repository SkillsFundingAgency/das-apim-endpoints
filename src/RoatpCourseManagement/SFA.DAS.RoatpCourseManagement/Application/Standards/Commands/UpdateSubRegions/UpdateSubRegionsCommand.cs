using MediatR;
using System.Net;

namespace SFA.DAS.Roatp.CourseManagement.Application.Standards.Commands.UpdateSubRegions
{
    public class UpdateSubRegionsCommand : IRequest<HttpStatusCode>
    {
        public int Ukprn{ get; set; }
        public int LarsCode { get; set; }
        public string UserId { get; set; }
        public string[] SelectedSubRegions { get; set; }
    }
}
