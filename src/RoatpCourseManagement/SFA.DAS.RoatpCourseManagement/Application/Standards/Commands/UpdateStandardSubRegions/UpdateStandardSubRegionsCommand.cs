using MediatR;
using System.Collections.Generic;
using System.Net;

namespace SFA.DAS.Roatp.CourseManagement.Application.Standards.Commands.UpdateStandardSubRegions
{
    public class UpdateStandardSubRegionsCommand : IRequest<HttpStatusCode>
    {
        public int Ukprn{ get; set; }
        public int LarsCode { get; set; }
        public string UserId { get; set; }
        public List<int> SelectedSubRegions { get; set; }
    }
}
