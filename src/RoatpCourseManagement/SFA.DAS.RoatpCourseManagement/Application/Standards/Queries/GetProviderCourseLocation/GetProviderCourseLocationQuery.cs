using MediatR;

namespace SFA.DAS.RoatpCourseManagement.Application.Standards.Queries.GetProviderCourseLocation
{
    public class GetProviderCourseLocationQuery : IRequest<GetProviderCourseLocationResult>
    {
        public int Ukprn { get; }
        public string LarsCode { get; }

        public GetProviderCourseLocationQuery(int ukprn, string larsCode)
        {
            Ukprn = ukprn;
            LarsCode = larsCode;
        }
    }
}