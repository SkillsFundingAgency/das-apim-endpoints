using MediatR;

namespace SFA.DAS.RoatpCourseManagement.Application.Standards.Queries.GetStandardInformation;

public record GetStandardInformationQuery(string LarsCode) : IRequest<GetStandardInformationQueryResult>;