using MediatR;

namespace SFA.DAS.RoatpCourseManagement.Application.Standards.Queries.GetProviderCourse
{
    public class GetProviderCourseQuery : IRequest<GetProviderCourseResult>
    {
        public int Ukprn { get; }
        public string LarsCode { get; }

        public GetProviderCourseQuery(int ukprn, string larsCode)
        {
            Ukprn = ukprn;
            LarsCode = larsCode;
        }
    }
}