using System.Net;
using MediatR;
using SFA.DAS.LearnerDataJobs.InnerApi;

namespace SFA.DAS.LearnerDataJobs.Application.Commands;

public class AddLearnerDataCommand : IRequest<HttpStatusCode>
{
    public LearnerDataRequest LearnerData { get; set; }
}
