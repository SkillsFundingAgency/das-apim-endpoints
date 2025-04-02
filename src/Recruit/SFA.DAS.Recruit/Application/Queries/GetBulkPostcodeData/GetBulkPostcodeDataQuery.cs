using System.Collections.Generic;
using MediatR;

namespace SFA.DAS.Recruit.Application.Queries.GetBulkPostcodeData;

public record GetBulkPostcodeDataQuery(List<string> Postcodes) : IRequest<GetBulkPostcodeDataResult>;