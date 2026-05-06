using MediatR;

namespace SFA.DAS.RoatpCourseManagement.Application.Standards.Queries.GetAllStandardRegions
{
    public class GetAllStandardRegionsQuery : IRequest<GetAllStandardRegionsQueryResult>
    {
        public int Ukprn { get; }
        public string LarsCode { get; }

        public GetAllStandardRegionsQuery(int ukprn, string larsCode)
        {
            Ukprn = ukprn;
            LarsCode = larsCode;
        }
    }
}
