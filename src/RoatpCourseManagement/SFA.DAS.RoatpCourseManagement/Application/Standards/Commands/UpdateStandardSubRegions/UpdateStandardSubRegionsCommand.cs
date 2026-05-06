using System.Collections.Generic;
using System.Net;
using MediatR;

namespace SFA.DAS.Roatp.CourseManagement.Application.Standards.Commands.UpdateStandardSubRegions
{
    public class UpdateStandardSubRegionsCommand : IRequest<HttpStatusCode>
    {
        public int Ukprn { get; set; }
        public string LarsCode { get; set; }
        public string UserId { get; set; }
        public string UserDisplayName { get; set; }
        public List<int> SelectedSubRegions { get; set; }
    }
}
