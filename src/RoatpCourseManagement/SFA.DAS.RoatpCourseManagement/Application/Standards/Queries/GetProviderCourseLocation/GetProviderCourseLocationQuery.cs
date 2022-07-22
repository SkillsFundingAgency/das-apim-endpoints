using MediatR;

namespace SFA.DAS.RoatpCourseManagement.Application.Standards.Queries.GetProviderCourseLocation
{
    public class GetProviderCourseLocationQuery : IRequest<GetProviderCourseLocationResult>
    {
        public int Ukprn { get; }
        public int LarsCode { get; }

        public GetProviderCourseLocationQuery(int ukprn,  int larsCode)
        {
            Ukprn = ukprn;
            LarsCode = larsCode;
        }
    }
}