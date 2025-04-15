using MediatR;

namespace SFA.DAS.AODP.Application.Queries.OutputFile;

public class GetPreviousOutputFilesQuery : IRequest<BaseMediatrResponse<GetPreviousOutputFilesQueryResponse>> { }