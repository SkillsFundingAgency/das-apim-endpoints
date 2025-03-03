using SFA.DAS.ProviderPR.InnerApi.Requests;

namespace SFA.DAS.ProviderPR.UnitTests.InnerApi.Requests;

public sealed class GetProviderRelationshipsRequestTests
{
    [Test]
    public void ToDictionary_AddsSearchTerm_WhenSearchTermIsNotNullOrWhiteSpace()
    {
        GetProviderRelationshipsRequest providerRelationshipsRequest = new GetProviderRelationshipsRequest()
        {
            SearchTerm = "SeachTerm"
        };

        Dictionary<string, string> sut = providerRelationshipsRequest.ToDictionary();

        Assert.Multiple(() =>
        {
            Assert.That(sut.ContainsKey(nameof(GetProviderRelationshipsRequest.SearchTerm)), Is.True);
            Assert.That(sut[nameof(GetProviderRelationshipsRequest.SearchTerm)], Is.EqualTo(providerRelationshipsRequest.SearchTerm));
        });
    }

    [Test]
    public void ToDictionary_AddsHasCreateCohortPermission_WhenHasCreateCohortPermissionIsTrue()
    {
        GetProviderRelationshipsRequest providerRelationshipsRequest = new GetProviderRelationshipsRequest()
        {
            HasCreateCohortPermission = true
        };

        Dictionary<string, string> sut = providerRelationshipsRequest.ToDictionary();

        Assert.Multiple(() =>
        {
            Assert.That(sut.ContainsKey(nameof(GetProviderRelationshipsRequest.HasCreateCohortPermission)), Is.True);
            Assert.That(sut[nameof(GetProviderRelationshipsRequest.HasCreateCohortPermission)], Is.EqualTo("True"));
        });
    }

    [Test]
    public void ToDictionary_DoesNotAddsHasCreateCohortPermission_WhenHasCreateCohortPermissionIsFalse()
    {
        GetProviderRelationshipsRequest providerRelationshipsRequest = new GetProviderRelationshipsRequest()
        {
            HasCreateCohortPermission = false
        };

        Dictionary<string, string> sut = providerRelationshipsRequest.ToDictionary();

        Assert.Multiple(() =>
        {
            Assert.That(sut.ContainsKey(nameof(GetProviderRelationshipsRequest.HasCreateCohortPermission)), Is.True);
            Assert.That(sut[nameof(GetProviderRelationshipsRequest.HasCreateCohortPermission)], Is.EqualTo("False"));
        });
    }

    [Test]
    public void ToDictionary_DoesNotAddsHasCreateCohortPermission_WhenHasCreateCohortPermissionIsNull()
    {
        GetProviderRelationshipsRequest providerRelationshipsRequest = new GetProviderRelationshipsRequest()
        {
            HasCreateCohortPermission = null
        };

        Dictionary<string, string> sut = providerRelationshipsRequest.ToDictionary();

        Assert.That(sut.ContainsKey(nameof(GetProviderRelationshipsRequest.HasCreateCohortPermission)), Is.False);
    }

    [Test]
    public void ToDictionary_AddsHasRecruitmentPermission_WhenHasRecruitmentPermissionIsTrue()
    {
        GetProviderRelationshipsRequest providerRelationshipsRequest = new GetProviderRelationshipsRequest()
        {
            HasRecruitmentPermission = true
        };

        Dictionary<string, string> sut = providerRelationshipsRequest.ToDictionary();

        Assert.Multiple(() =>
        {
            Assert.That(sut.ContainsKey(nameof(GetProviderRelationshipsRequest.HasRecruitmentPermission)), Is.True);
            Assert.That(sut[nameof(GetProviderRelationshipsRequest.HasRecruitmentPermission)], Is.EqualTo("True"));
        });
    }

    [Test]
    public void ToDictionary_DoesNotAddHasRecruitmentPermission_WhenHasRecruitmentPermissionIsFalse()
    {
        GetProviderRelationshipsRequest providerRelationshipsRequest = new GetProviderRelationshipsRequest()
        {
            HasRecruitmentPermission = false
        };

        Dictionary<string, string> sut = providerRelationshipsRequest.ToDictionary();

        Assert.That(sut.ContainsKey(nameof(GetProviderRelationshipsRequest.HasRecruitmentPermission)), Is.False);
    }

    [Test]
    public void ToDictionary_AddsHasRecruitmentWithReviewPermission_WhenHasRecruitmentWithReviewPermissionIsTrue()
    {
        GetProviderRelationshipsRequest providerRelationshipsRequest = new GetProviderRelationshipsRequest()
        {
            HasRecruitmentWithReviewPermission = true
        };

        Dictionary<string, string> sut = providerRelationshipsRequest.ToDictionary();

        Assert.Multiple(() =>
        {
            Assert.That(sut.ContainsKey(nameof(GetProviderRelationshipsRequest.HasRecruitmentWithReviewPermission)), Is.True);
            Assert.That(sut[nameof(GetProviderRelationshipsRequest.HasRecruitmentWithReviewPermission)], Is.EqualTo("True"));
        });
    }

    [Test]
    public void ToDictionary_DoesNotAddHasRecruitmentWithReviewPermission_WhenHasRecruitmentWithReviewPermissionIsFalse()
    {
        GetProviderRelationshipsRequest providerRelationshipsRequest = new GetProviderRelationshipsRequest()
        {
            HasRecruitmentWithReviewPermission = false
        };

        Dictionary<string, string> sut = providerRelationshipsRequest.ToDictionary();

        Assert.That(sut.ContainsKey(nameof(GetProviderRelationshipsRequest.HasRecruitmentWithReviewPermission)), Is.False);
    }

    [Test]
    public void ToDictionary_AddsHasNoRecruitmentPermission_WhenHasNoRecruitmentPermissionIsTrue()
    {
        GetProviderRelationshipsRequest providerRelationshipsRequest = new GetProviderRelationshipsRequest()
        {
            HasNoRecruitmentPermission = true
        };

        Dictionary<string, string> sut = providerRelationshipsRequest.ToDictionary();

        Assert.Multiple(() =>
        {
            Assert.That(sut.ContainsKey(nameof(GetProviderRelationshipsRequest.HasNoRecruitmentPermission)), Is.True);
            Assert.That(sut[nameof(GetProviderRelationshipsRequest.HasNoRecruitmentPermission)], Is.EqualTo("True"));
        });
    }

    [Test]
    public void ToDictionary_DoesNotAddHasNoRecruitmentPermission_WhenHasNoRecruitmentPermissionIsFalse()
    {
        GetProviderRelationshipsRequest providerRelationshipsRequest = new GetProviderRelationshipsRequest()
        {
            HasNoRecruitmentPermission = false
        };

        Dictionary<string, string> sut = providerRelationshipsRequest.ToDictionary();

        Assert.That(sut.ContainsKey(nameof(GetProviderRelationshipsRequest.HasNoRecruitmentPermission)), Is.False);
    }

    [Test]
    public void ToDictionary_AddsHasPendingRequest_WhenHasHasPendingRequestIsTrue()
    {
        GetProviderRelationshipsRequest providerRelationshipsRequest = new GetProviderRelationshipsRequest()
        {
            HasPendingRequest = true
        };

        Dictionary<string, string> sut = providerRelationshipsRequest.ToDictionary();

        Assert.Multiple(() =>
        {
            Assert.That(sut.ContainsKey(nameof(GetProviderRelationshipsRequest.HasPendingRequest)), Is.True);
            Assert.That(sut[nameof(GetProviderRelationshipsRequest.HasPendingRequest)], Is.EqualTo("True"));
        });
    }

    [Test]
    public void ToDictionary_DoesNotAddHasPendingRequest_WhenHasPendingRequestIsFalse()
    {
        GetProviderRelationshipsRequest providerRelationshipsRequest = new GetProviderRelationshipsRequest()
        {
            HasPendingRequest = false
        };

        Dictionary<string, string> sut = providerRelationshipsRequest.ToDictionary();

        Assert.That(sut.ContainsKey(nameof(GetProviderRelationshipsRequest.HasPendingRequest)), Is.False);
    }

    [Test]
    public void ToDictionary_DoesNotAddHasPendingRequest_WhenHasPendingRequestIsNull()
    {
        GetProviderRelationshipsRequest providerRelationshipsRequest = new GetProviderRelationshipsRequest()
        {
            HasPendingRequest = null
        };

        Dictionary<string, string> sut = providerRelationshipsRequest.ToDictionary();

        Assert.That(sut.ContainsKey(nameof(GetProviderRelationshipsRequest.HasPendingRequest)), Is.False);
    }

    [Test]
    public void ToDictionary_AddsPageSize_WhenPageSizeIsMoreThanZero()
    {
        GetProviderRelationshipsRequest providerRelationshipsRequest = new GetProviderRelationshipsRequest()
        {
            PageSize = 10
        };

        Dictionary<string, string> sut = providerRelationshipsRequest.ToDictionary();

        Assert.Multiple(() =>
        {
            Assert.That(sut.ContainsKey(nameof(GetProviderRelationshipsRequest.PageSize)), Is.True);
            Assert.That(sut[nameof(GetProviderRelationshipsRequest.PageSize)], Is.EqualTo("10"));
        });
    }

    [Test]
    public void ToDictionary_DoesNotAddPageSize_WhenPageSizeIsZero()
    {
        GetProviderRelationshipsRequest providerRelationshipsRequest = new GetProviderRelationshipsRequest()
        {
            PageSize = 0
        };

        Dictionary<string, string> sut = providerRelationshipsRequest.ToDictionary();

        Assert.That(sut.ContainsKey(nameof(GetProviderRelationshipsRequest.PageSize)), Is.False);
    }

    [Test]
    public void ToDictionary_DoesNotAddPageSize_WhenPageSizeIsNull()
    {
        GetProviderRelationshipsRequest providerRelationshipsRequest = new GetProviderRelationshipsRequest()
        {
            PageSize = null
        };

        Dictionary<string, string> sut = providerRelationshipsRequest.ToDictionary();

        Assert.That(sut.ContainsKey(nameof(GetProviderRelationshipsRequest.PageSize)), Is.False);
    }

    [Test]
    public void ToDictionary_AddsPageNumber_WhenPageNumberIsMoreThanZero()
    {
        GetProviderRelationshipsRequest providerRelationshipsRequest = new GetProviderRelationshipsRequest()
        {
            PageNumber = 3
        };

        Dictionary<string, string> sut = providerRelationshipsRequest.ToDictionary();

        Assert.Multiple(() =>
        {
            Assert.That(sut.ContainsKey(nameof(GetProviderRelationshipsRequest.PageNumber)), Is.True);
            Assert.That(sut[nameof(GetProviderRelationshipsRequest.PageNumber)], Is.EqualTo("3"));
        });
    }

    [Test]
    public void ToDictionary_DoesNotAddPageNumber_WhenPageNumberIsZero()
    {
        GetProviderRelationshipsRequest providerRelationshipsRequest = new GetProviderRelationshipsRequest()
        {
            PageNumber = 0
        };

        Dictionary<string, string> sut = providerRelationshipsRequest.ToDictionary();

        Assert.That(sut.ContainsKey(nameof(GetProviderRelationshipsRequest.PageNumber)), Is.False);
    }

    [Test]
    public void ToDictionary_DoesNotAddPageNumber_WhenPageNumberIsNull()
    {
        GetProviderRelationshipsRequest providerRelationshipsRequest = new GetProviderRelationshipsRequest()
        {
            PageNumber = null
        };

        Dictionary<string, string> sut = providerRelationshipsRequest.ToDictionary();

        Assert.That(sut.ContainsKey(nameof(GetProviderRelationshipsRequest.PageNumber)), Is.False);
    }
}
