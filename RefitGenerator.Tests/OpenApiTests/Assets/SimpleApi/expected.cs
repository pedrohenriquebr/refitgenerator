using System;
using Refit;

public interface ISimpleOpenApiOverviewService
{
    [Get]
    public Task<ListVersionsv2Response> ListVersionsv2();

    [Get("/v2")]
    public Task<GetVersionDetailsv2Response> GetVersionDetailsv2();
}

public class ListVersionsv2Response
{
    public List<VersionsResponse> Versions { get; set; }
}

public class VersionsResponse
{
    public string Status { get; set; }
    public DateTime Updated { get; set; }
    public string Id { get; set; }
    public List<LinksResponse> Links { get; set; }
}

public class LinksResponse
{
    public string Href { get; set; }
    public string Rel { get; set; }
}