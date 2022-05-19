using MediatR;

namespace SFA.DAS.Roatp.CourseManagement.Application.Standards.Queries.GetProviderCourse
{
    public class GetProviderCourseQuery : IRequest<GetProviderCourseResult>
    {
        public int Ukprn { get; }
        public int LarsCode { get; }
        public int ProviderCourseId { get; }

        public GetProviderCourseQuery(int ukprn,  int larsCode, int providerCourseId)
        {
            Ukprn = ukprn;
            LarsCode = larsCode;
            ProviderCourseId = providerCourseId;
        }
    }
}