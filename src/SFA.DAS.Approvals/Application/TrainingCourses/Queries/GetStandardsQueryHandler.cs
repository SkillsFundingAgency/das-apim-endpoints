using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace SFA.DAS.Approvals.Application.TrainingCourses.Queries
{
    public class GetStandardsQueryHandler : IRequestHandler<GetStandardsQuery, GetStandardsResult>
    {
        public async Task<GetStandardsResult> Handle(GetStandardsQuery request, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }
    }
}