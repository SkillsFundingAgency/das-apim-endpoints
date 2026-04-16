using System;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.FindApprenticeshipTraining.InnerApi.Requests;

public class DeleteShortlistItemRequest : IDeleteApiRequest
{
    private readonly Guid _id;

    public DeleteShortlistItemRequest(Guid id)
    {
        _id = id;
    }

    public string DeleteUrl => $"shortlists/{_id}";
}