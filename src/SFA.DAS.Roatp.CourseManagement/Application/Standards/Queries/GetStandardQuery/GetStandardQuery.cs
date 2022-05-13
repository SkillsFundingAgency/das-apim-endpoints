using MediatR;

namespace SFA.DAS.Roatp.CourseManagement.Application.Standards.Queries.GetStandardQuery
{
    public class GetStandardQuery : IRequest<GetStandardResult>
    {
        public int LarsCode { get; }

        public GetStandardQuery(int larsCode)
        {
            LarsCode = larsCode;
        }
    }
}
