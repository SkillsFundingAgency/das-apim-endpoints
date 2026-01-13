using MediatR;

namespace SFA.DAS.RoatpCourseManagement.Application.Standards.Queries.GetStandardInformation;

public record GetStandardInformationQuery : IRequest<GetStandardInformationQueryResult>
{
    public string LarsCode { get; }
    public GetStandardInformationQuery(string larsCode)
    {
        LarsCode = larsCode;
    }
}