using MediatR;

namespace SFA.DAS.RoatpCourseManagement.Application.Standards.Queries.GetStandardInformation
{
    public class GetStandardInformationQuery : IRequest<GetStandardInformationQueryResult>
    {
        public int LarsCode { get; }
        public GetStandardInformationQuery(int larsCode)
        {
            LarsCode = larsCode;
        }
    }
}
