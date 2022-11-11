using System;
using Refit;

public interface ISimpleOpenApiOverviewService
{
    [Get]
    public Task<ListVersionsv2Response> ListVersionsv2();
    [Get("/v2")]
    public Task<GetVersionDetailsv2Response> GetVersionDetailsv2();
}

public class ListVersionsv2
{
    public List<VersionsItem> Versions { get; set; }
}

public class VersionsItem
{
    public string Status { get; set; }

    public string Updated { get; set; }

    public string Id { get; set; }

    public List<LinksItem> Links { get; set; }
}

public class LinksItem
{
    public string Href { get; set; }

    public string Rel { get; set; }
}

public class GetVersionDetailsv2
{
    public VersionResponse Version { get; set; }
}

public class VersionResponse
{
    public string Status { get; set; }

    public string Updated { get; set; }

    public List<MediaTypesItem> MediaTypes { get; set; }

    public string Id { get; set; }

    public List<LinksItem> Links { get; set; }
}

public class MediaTypesItem
{
    public string Base { get; set; }

    public string Type { get; set; }
}

public class LinksItem
{
    public string Href { get; set; }

    public string Rel { get; set; }
}